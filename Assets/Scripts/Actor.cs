using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Actor : MonoBehaviour {

	public float health;
	float maxHealth;
	public float attack;
	public float defense;

	public float orientation;

	public Transform transform;
	public SpriteRenderer[] runes;
	public Projectile projectile;

	public TextMesh healthText, minusText;

	public Shield fireShield, waterShield, airShield, earthShield;
	public Transform playerHitEnemy;


	public AttackHitInfo hitInfo {get; private set;}
	Elements currentAttackType;
	ADParams currentActionParams;
	Elements currentDefenseType;
	Dictionary<Elements, Shield> shields = new Dictionary<Elements, Shield>();

	protected ActorStates currentState = ActorStates.IDLE;

	const int MAX_LETTERS = 2;
	Queue<Letters> word = new Queue<Letters>();
	Letters letterToQueue;
	Buttons buttonPressed = Buttons.NONE;

	System.Action attackOrDefenseAction;
	Dictionary<int, ActionType> availableActions = new Dictionary<int, ActionType>(16);

	PlayerInput playerInput;

	public Transform healthBar;
	public SpriteRenderer spriteRenderer;
	public Sprite idle, rune1a, rune1d, rune2;

	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		maxHealth = health;
		healthText.text = health.ToString();
		minusText.text = "";

		currentActionParams = new ADParams();

		attackOrDefenseAction = DoNothing;
		hitInfo = new AttackHitInfo(Elements.NONE, 0);

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.FIRE.GetHashCode(), new ActionType(Actions.ATTACK, Elements.FIRE));
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.FIRE.GetHashCode(), new ActionType(Actions.DEFEND, Elements.FIRE));

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.WATER.GetHashCode(), new ActionType(Actions.ATTACK, Elements.WATER));
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.WATER.GetHashCode(), new ActionType(Actions.DEFEND, Elements.WATER));

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.AIR.GetHashCode(), new ActionType(Actions.ATTACK, Elements.AIR));
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.AIR.GetHashCode(), new ActionType(Actions.DEFEND, Elements.AIR));

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.EARTH.GetHashCode(), new ActionType(Actions.ATTACK, Elements.EARTH));
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.EARTH.GetHashCode(), new ActionType(Actions.DEFEND, Elements.EARTH));

		shields.Add(Elements.FIRE, fireShield);
		shields.Add(Elements.WATER, waterShield);
		shields.Add(Elements.AIR, airShield);
		shields.Add(Elements.EARTH, earthShield);
	}

	void Start()
	{
		RemoveLetters();
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
			RemoveLetters();
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
		if(word.Count < MAX_LETTERS)
		{
			buttonPressed = GetButton();
			if(buttonPressed == Buttons.NONE)
				return;

			letterToQueue = GetLetterForInput(buttonPressed, word.Count);
			if(letterToQueue == Letters.NONE)
				return;

			word.Enqueue(letterToQueue);
			AddLetter(word.Count -1, letterToQueue);

			if(word.Peek() == Letters.ATTACK)
				spriteRenderer.sprite = rune1a;
			else
				spriteRenderer.sprite = rune1d;
		}
		else
		{
			Letters l1, l2;
			l1 = word.Dequeue();
			l2 = word.Dequeue();
			int combo = l1.GetHashCode() + l2.GetHashCode();
			ActionType action;

			if(availableActions.TryGetValue(combo, out action))
			{
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
				Debug.Log("Option not available");
				RemoveLetters();
			}
		}
	}

	void AddLetter(int position, Letters letter)
	{
		runes[position].color = new Color(1f,1f,1f,1f);
		runes[position].sprite = Game.runeByLetter[letter];
		Debug.Log("Llenar letra visualmente: " + position);
	}

	void RemoveLetters()
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
			break;
		case Buttons.LEFT:
			if(order == 0)
				return Letters.DEFEND;
			else
				return Letters.WATER;
			break;
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
		Debug.Log("Animation playing");
		UpdateProjectile(deltaTime);
		currentActionParams.animationTime -= deltaTime;
		if(currentActionParams.animationTime <= 0)
		{
			currentActionParams.animationTime = 0;
			currentState = ActorStates.RECOVERY;
			spriteRenderer.sprite = idle;
			return;
		}
	}

	void UpdateShowing(float deltaTime)
	{
		Debug.Log("Showing my runes");
		currentActionParams.connectTime -= deltaTime;
		if(currentActionParams.connectTime <= 0)
		{
			currentActionParams.connectTime = 0;
			currentState = ActorStates.ATTACKING;
			attackOrDefenseAction();
		}
	}

	public void HideProjectile()
	{
		projectile.Hide();
	}

	void UpdateProjectile(float deltaTime)
	{
		projectile.rigidBody.AddForce(projectile.transform.right * orientation * currentActionParams.projectileSpeed);
	}

	public void TakeHit(AttackHitInfo hit)
	{
		if(hit.element == Elements.NONE)
			return;

		Debug.Log("<color=red>Take Hit, " + currentDefenseType+"</color>");

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
			Game.gameManager.Lost(orientation);
			return;
		}

		Game.soundManager.PlayPlayerHit();

	}

	void DoNothing(){}

	void UpdateHealthBar()
	{
//		healthBar.scale
		float n1 = health / maxHealth;
		float val = n1*5.45f;
		healthBar.localScale = new Vector3(val, healthBar.localScale.y, healthBar.localScale.z);
	}

	// ATTACKS

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

	// DEFENSES

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