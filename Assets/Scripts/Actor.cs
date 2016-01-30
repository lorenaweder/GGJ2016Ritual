using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public float orientation;
	public float health;
	public float attack;
	public float defense;

	public Transform transform;
	public SpriteRenderer[] runes;
	public Projectile projectile;
	float attackProjectileForce = 7f;

	public Shield fireShield, waterShield, airShield, earthShield;
	public Transform playerHitEnemy;

	protected float recoveryTime = 0f;
	protected float connectTime = 0f;
	protected float animationTime = 0f;

	public AttackHitInfo ? hitInfo {get; private set;}
	protected Elements currentAttackType;
	protected Elements currentDefenseType;
	Dictionary<Elements, Shield> shields = new Dictionary<Elements, Shield>();

	protected ActorStates currentState = ActorStates.IDLE;

	const int MAX_LETTERS = 2;
	Queue<Letters> word = new Queue<Letters>();
	Letters letterToQueue;
	Buttons buttonPressed = Buttons.NONE;

	System.Action attackOrDefenseAction;
	Dictionary<int, System.Action> availableActions = new Dictionary<int, System.Action>(16);

	PlayerInput playerInput;

	void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		attackOrDefenseAction = DoNothing;

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.FIRE.GetHashCode(), InitFireAttack);
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.FIRE.GetHashCode(), InitFireDefense);

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.WATER.GetHashCode(), InitWaterAttack);
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.WATER.GetHashCode(), InitWaterDefense);

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.AIR.GetHashCode(), InitAirAttack);
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.AIR.GetHashCode(), InitAirDefense);

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.EARTH.GetHashCode(), InitEarthAttack);
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.EARTH.GetHashCode(), InitEarthDefense);

		shields.Add(Elements.FIRE, fireShield);
		shields.Add(Elements.WATER, waterShield);
		shields.Add(Elements.AIR, airShield);
		shields.Add(Elements.EARTH, earthShield);

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
		if(recoveryTime > 0)
		{
			recoveryTime -= deltaTime;
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
		}
		else
		{
			Letters l1, l2;
			l1 = word.Dequeue();
			l2 = word.Dequeue();
			int combo = l1.GetHashCode() + l2.GetHashCode();
			System.Action action = DoNothing;

			if(availableActions.TryGetValue(combo, out action))
				action();
			else
			{
				Debug.Log("Option not available");
				RemoveLetters();
			}
		}
	}

	void AddLetter(int position, Letters letter)
	{
		runes[position].sprite = Game.runeByLetter[letter];
		Debug.Log("Llenar letra visualmente: " + position);
	}

	void RemoveLetters()
	{
		for(int i=0; i<runes.Length;++i)
		{
			runes[i].sprite = null;
		}
		Debug.Log("Quitar letras visualmente");
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
		animationTime -= deltaTime;
		if(animationTime <= 0)
		{
			animationTime = 0;
			currentState = ActorStates.RECOVERY;
			return;
		}
	}

	void UpdateShowing(float deltaTime)
	{
		Debug.Log("Showing my runes");
		connectTime -= deltaTime;
		if(connectTime <= 0)
		{
			connectTime = 0;
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
		projectile.rigidBody.AddForce(projectile.transform.right * orientation * attackProjectileForce);
	}

	public void TakeHit(AttackHitInfo? hit)
	{
		if(hit == null)
			return;
			
		switch(currentDefenseType)
		{
		case Elements.NONE:
			// entra todo daño - defensa
			break;
		}
	}

	void DoNothing(){}


	// ATTACKS

	void InitFireAttack()
	{
		currentState = ActorStates.SHOWING;
		currentAttackType = Elements.FIRE;
		connectTime = 0.5f;
		attackOrDefenseAction = FireAttack;
	}

	void FireAttack()
	{
		Debug.Log("Perform FIRE ATTACK");
		projectile.Show();
		projectile.animator.SetTrigger("fire");
		animationTime = 1f;
		recoveryTime = .7f;
	}

	void InitWaterAttack()
	{
		currentState = ActorStates.SHOWING;
		currentAttackType = Elements.WATER;
		connectTime = 0.5f;
		attackOrDefenseAction = WaterAttack;
	}

	void WaterAttack()
	{
		Debug.Log("Perform WATER ATTACK");
		projectile.Show();
		projectile.animator.SetTrigger("water");
		animationTime = 1f;
		recoveryTime = .7f;
	}

	void InitAirAttack()
	{
		currentState = ActorStates.SHOWING;
		currentAttackType = Elements.AIR;
		connectTime = 0.5f;
		attackOrDefenseAction = AirAttack;
	}

	void AirAttack()
	{
		Debug.Log("Perform AIR ATTACK");
		projectile.Show();
		projectile.animator.SetTrigger("air");
		animationTime = 1f;
		recoveryTime = .7f;
	}

	void InitEarthAttack()
	{
		currentState = ActorStates.SHOWING;
		currentAttackType = Elements.EARTH;
		connectTime = 0.5f;
		attackOrDefenseAction = EarthAttack;
	}

	void EarthAttack()
	{
		Debug.Log("Perform EARTH ATTACK");
		projectile.Show();
		projectile.animator.SetTrigger("earth");
		animationTime = 1f;
		recoveryTime = .7f;
	}


	// DEFENSES

	void LowerPreviousShield()
	{
		if(currentDefenseType == Elements.NONE)
			return;
		shields[currentDefenseType].ResetShield();
	}

	void InitFireDefense()
	{
		LowerPreviousShield();
		currentState = ActorStates.SHOWING;
		connectTime = 0.5f;
		attackOrDefenseAction = FireShieldUp;
	}

	void FireShieldUp()
	{
		currentDefenseType = Elements.FIRE;
		RiseShield();
		animationTime = .5f;
		recoveryTime = .7f;
	}

	void InitWaterDefense()
	{
		LowerPreviousShield();
		currentState = ActorStates.SHOWING;
		connectTime = 0.5f;
		attackOrDefenseAction = WaterShieldUp;
	}

	void WaterShieldUp()
	{
		currentDefenseType = Elements.WATER;
		RiseShield();
		animationTime = .5f;
		recoveryTime = .7f;
	}

	void InitAirDefense()
	{
		LowerPreviousShield();
		currentState = ActorStates.SHOWING;
		connectTime = 0.5f;
		attackOrDefenseAction = AirShieldUp;
	}

	void AirShieldUp()
	{
		currentDefenseType = Elements.AIR;
		RiseShield();
		animationTime = .5f;
		recoveryTime = .7f;
	}

	void InitEarthDefense()
	{
		LowerPreviousShield();
		currentState = ActorStates.SHOWING;
		connectTime = 0.5f;
		attackOrDefenseAction = EarthShieldUp;
	}

	void EarthShieldUp()
	{
		currentDefenseType = Elements.EARTH;
		RiseShield();
		animationTime = .5f;
		recoveryTime = .7f;
	}


	void RiseShield()
	{
		shields[currentDefenseType].enabled = true;
		shields[currentDefenseType].RiseShield();
	}

	// HITS

}