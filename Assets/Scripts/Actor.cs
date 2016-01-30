using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {

	public int health;
	public int attack;
	public int defense;

	public SpriteRenderer[] runes;
	public Animator attackProjectile;

	protected float recoveryTime = 0f;
	protected float connectTime = 0f;
	protected float animationTime = 0f;

	protected Elements currentAttackType;
	protected Elements currentDefenseType;

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
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.WATER.GetHashCode(), DoNothing);

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.AIR.GetHashCode(), InitAirAttack);
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.AIR.GetHashCode(), DoNothing);

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.EARTH.GetHashCode(), InitEarthAttack);
		availableActions.Add(Letters.DEFEND.GetHashCode() + Letters.EARTH.GetHashCode(), DoNothing);
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
		animationTime -= deltaTime;
		if(animationTime <= 0)
		{
			animationTime = 0;
			currentState = ActorStates.RECOVERY;
		}	
	}

	void UpdateShowing(float deltaTime)
	{
		Debug.Log("Showing my runes");
		connectTime -= deltaTime;
		if(connectTime <= 0)
		{
			// lanzar ataque posta
			connectTime = 0;
			currentState = ActorStates.ATTACKING;
			attackOrDefenseAction();
		}
	}

	public virtual void TakeHit(Elements attackElement, int attack)
	{
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
		connectTime = 0.2f;
		attackOrDefenseAction = FireAttack;
	}

	void FireAttack()
	{
		Debug.Log("Perform FIRE ATTACK");
		attackProjectile.SetTrigger("fire");
		animationTime = .5f;
		recoveryTime = .7f;
	}

	void InitWaterAttack()
	{
		currentState = ActorStates.SHOWING;
		currentAttackType = Elements.WATER;
		connectTime = 0.2f;
		attackOrDefenseAction = WaterAttack;
	}

	void WaterAttack()
	{
		Debug.Log("Perform WATER ATTACK");
		attackProjectile.SetTrigger("water");
		animationTime = .5f;
		recoveryTime = .7f;
	}

	void InitAirAttack()
	{
		currentState = ActorStates.SHOWING;
		currentAttackType = Elements.AIR;
		connectTime = 0.2f;
		attackOrDefenseAction = AirAttack;
	}

	void AirAttack()
	{
		Debug.Log("Perform AIR ATTACK");
		attackProjectile.SetTrigger("air");
		animationTime = .5f;
		recoveryTime = .7f;
	}

	void InitEarthAttack()
	{
		currentState = ActorStates.SHOWING;
		currentAttackType = Elements.EARTH;
		connectTime = 0.2f;
		attackOrDefenseAction = EarthAttack;
	}

	void EarthAttack()
	{
		Debug.Log("Perform EARTH ATTACK");
		attackProjectile.SetTrigger("earth");
		animationTime = .5f;
		recoveryTime = .7f;
	}


	// DEFENSES

	void InitFireDefense()
	{
		
	}
}