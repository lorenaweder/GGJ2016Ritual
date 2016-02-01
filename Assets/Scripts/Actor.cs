using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Actor : MonoBehaviour {

	public float health;
	public float attack;
	public float defense;
	public Transform healthBar;

	public float orientation;

	public new Transform transform;
	public SpriteRenderer[] runes;
	public Projectile projectile;

	public TextMesh healthText, minusText;

	public Shield fireShield, waterShield, airShield, earthShield;

	public AttackHitInfo hitInfo {get; private set;}
	Elements currentAttackType;
	ADParams currentActionParams;
	Elements currentDefenseType;
	Dictionary<Elements, Shield> shields = new Dictionary<Elements, Shield>(4);

	protected ActorStates currentState = ActorStates.IDLE;

	const int MAX_LETTERS = 2;
	List<Letters> word = new List<Letters>(MAX_LETTERS);
	Letters letterToQueue;
	Buttons buttonPressed = Buttons.NONE;

	System.Action attackOrDefenseAction;
	PlayerActions playerActions;

	PlayerInput playerInput;

	float maxHealth;
	float initialHealthBarScale;

	public SpriteRenderer spriteRenderer;
	public Sprite idle, rune1a, rune1d, rune2;

	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		initialHealthBarScale = healthBar.localScale.x;
		maxHealth = health;
		healthText.text = health.ToString();
		minusText.text = "";

		currentActionParams = new ADParams();

		attackOrDefenseAction = DoNothing;
		hitInfo = new AttackHitInfo(Elements.NONE, 0);

		shields.Add(Elements.FIRE, fireShield);
		shields.Add(Elements.WATER, waterShield);
		shields.Add(Elements.AIR, airShield);
		shields.Add(Elements.EARTH, earthShield);
	}

	void Start()
	{
		playerActions = Game.playerActions;
		RemoveLettersFromScreen();
	}

	public void CustomUpdate(float deltaTime)
	{
		switch(currentState)
		{
		case ActorStates.IDLE:
			UpdateIdle(deltaTime);
			break;
		case ActorStates.SHOWING:
			UpdateShowing(deltaTime);
			break;
		case ActorStates.ATTACKING:
			UpdateAttacking(deltaTime);
			break;
		case ActorStates.RECOVERY:
			UpdateRecovery(deltaTime);
			break;
		}
	}

	void UpdateRecovery(float deltaTime)
	{
		if(currentActionParams.recoveryTime > 0)
		{
			currentActionParams.recoveryTime -= deltaTime;
		}
		else
		{
			RemoveLettersFromScreen();
			currentState = ActorStates.IDLE;
		}
	}

	void UpdateIdle(float deltaTime)
	{
		playerInput.CheckInput();
		CheckWords();
	}

	void CheckWords()
	{
		buttonPressed = GetButton();
		if(buttonPressed == Buttons.NONE)
			return;

		letterToQueue = GetLetterForInput(buttonPressed, word.Count);
		if(letterToQueue == Letters.NONE)
			return;

		word.Add(letterToQueue);
		AddLetterToScreen(word.Count -1, letterToQueue);

		// hc
		if(word[word.Count -1] == Letters.ATTACK)
			spriteRenderer.sprite = rune1a;
		else
			spriteRenderer.sprite = rune1d;

		int combo = 0;
		ActionType action;

		for(int i=0; i < word.Count ; ++i)
		{
			combo += word[i].GetHashCode();
		}

		if(playerActions.available.TryGetValue(combo, out action))
		{
			word.Clear();

			// hc
			switch(action.action)
			{
			case Actions.ATTACK:
				InitAttack(action.element);
				break;
			case Actions.DEFEND:
				InitDefense(action.element);
				break;
			}
			spriteRenderer.sprite = rune2;
		}
		else
		{
			if(word.Count == MAX_LETTERS)
			{
				word.Clear();
				RemoveLettersFromScreen();
			}
		}
	}

	void AddLetterToScreen(int position, Letters letter)
	{
		runes[position].color = new Color(1f,1f,1f,1f);
		runes[position].sprite = Game.runeByLetter[letter];
	}

	void RemoveLettersFromScreen()
	{
		for(int i=0; i<runes.Length;++i)
		{
			runes[i].color = new Color(1f, 1f, 1f, .4f);
			runes[i].sprite = Game.runeByLetter[Letters.NONE];
		}
	}

	Letters GetLetterForInput(Buttons button, int order)
	{
		switch(button)
		{
		case Buttons.NONE:
			return Letters.NONE;
		case Buttons.RIGHT:
			if(order == 0)
				return Letters.ATTACK;
			else
				return Letters.FIRE;
		case Buttons.LEFT:
			if(order == 0)
				return Letters.DEFEND;
			else
				return Letters.WATER;
		case Buttons.UP:
			if(order == 1)
				return Letters.AIR;
			break;
		case Buttons.DOWN:
			if(order == 1)
				return Letters.EARTH;
			break;
		}
		return Letters.NONE;
	}

	Buttons GetButton()
	{
		if(playerInput.upPressed)
			return Buttons.UP;

		if(playerInput.downPressed)
			return Buttons.DOWN;

		if(playerInput.rightPressed)
			return Buttons.RIGHT;

		if(playerInput.leftPressed)
			return Buttons.LEFT;

		return Buttons.NONE;
	}

	void UpdateAttacking(float deltaTime)
	{
		UpdateProjectile(deltaTime);
		currentActionParams.animationTime -= deltaTime;
		if(currentActionParams.animationTime <= 0)
		{
			currentActionParams.animationTime = 0;
			currentState = ActorStates.RECOVERY;
			spriteRenderer.sprite = idle;
		}
	}

	void UpdateShowing(float deltaTime)
	{
		currentActionParams.connectTime -= deltaTime;
		if(currentActionParams.connectTime <= 0)
		{
			currentActionParams.connectTime = 0;
			currentState = ActorStates.ATTACKING;
			attackOrDefenseAction();
		}
	}

	void UpdateProjectile(float deltaTime)
	{
		projectile.rigidBody.AddForce(projectile.transform.right * orientation * currentActionParams.projectileSpeed);
	}

	public void HideProjectile()
	{
		projectile.Hide();
	}

	public void TakeHit(AttackHitInfo hit)
	{
		if(hit.element == Elements.NONE)
			return;

		bool shieldDestroyed = false;
		float shieldElementals = 1.25f;
		if(currentDefenseType != Elements.NONE)
		{
			shieldElementals = shields[currentDefenseType].TakeDamage(hit.element, out shieldDestroyed);
		}

		float critChance = 1;

		float totalDamage = (hit.attack / defense) * shieldElementals * critChance;
		health-= totalDamage;
		minusText.text = totalDamage.ToString();
		healthText.text = health.ToString();

		UpdateHealthBar();

		if(shieldDestroyed)
		{
			currentDefenseType = Elements.NONE;
		}

		if(health <= 0)
		{
			health = 0;
			Game.screenManager.Lost(orientation);
			return;
		}

		Game.soundManager.PlayPlayerHit();

	}

	void DoNothing(){}

	void UpdateHealthBar()
	{
		float newScaleX = (health / maxHealth) * initialHealthBarScale;
		if(newScaleX < 0) newScaleX = 0;
		healthBar.localScale = new Vector3(newScaleX, healthBar.localScale.y, healthBar.localScale.z);
	}

	// ATTACK

	public void CleanHitInfo()
	{
		hitInfo.element = Elements.NONE;
		hitInfo.attack = 0;
	}
		
	void InitAttack(Elements element)
	{
		currentState = ActorStates.SHOWING;
		attackOrDefenseAction = AttackAction;
		currentAttackType = element;
		currentActionParams.connectTime = Game.attackParamsByType[currentAttackType].connectTime;
	}

	void AttackAction()
	{
		projectile.Show(currentAttackType);

		projectile.rigidBody.AddForce(projectile.transform.right * orientation * 10f, ForceMode2D.Impulse);

		hitInfo.attack = attack;
		hitInfo.element = currentAttackType;

		currentActionParams.projectileSpeed = Game.attackParamsByType[currentAttackType].projectileSpeed;
		currentActionParams.animationTime = Game.attackParamsByType[currentAttackType].animationTime;
		currentActionParams.recoveryTime = Game.attackParamsByType[currentAttackType].recoveryTime;

		Game.soundManager.PlayElementSound(currentAttackType);
	}

	// DEFENSE

	void LowerPreviousShield()
	{
		if(currentDefenseType == Elements.NONE)
			return;
		shields[currentDefenseType].ResetShield();
		currentDefenseType = Elements.NONE;
	}

	void InitDefense(Elements element)
	{
		LowerPreviousShield();
		currentState = ActorStates.SHOWING;
		currentActionParams.connectTime = Game.defenseParamsByType[element].connectTime;
		currentActionParams.element = element;
		attackOrDefenseAction = DefenseAction;
	}

	void DefenseAction()
	{
		currentDefenseType = currentActionParams.element;
		currentActionParams.animationTime = Game.defenseParamsByType[currentDefenseType].animationTime;
		currentActionParams.recoveryTime = Game.defenseParamsByType[currentDefenseType].recoveryTime;
		RiseShield();
	}

	void RiseShield()
	{
		shields[currentDefenseType].enabled = true;
		shields[currentDefenseType].RiseShield();
	}
}