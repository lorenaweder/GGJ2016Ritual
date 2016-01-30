using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	const int MAX_LETTERS = 2;
	Queue<Letters> word = new Queue<Letters>();

	Letters letterToQueue;
	Buttons buttonPressed = Buttons.NONE;

	PlayerInput playerInput;


	ActionType actionToExecute;

	Dictionary<int, int> availableActions = new Dictionary<int, int>(16);
//	ActionType fireAttack 	= new ActionType(Letters.ATTACK, Letters.FIRE);
//	ActionType fireDefend 	= new ActionType(Letters.DEFEND, Letters.FIRE);
//	ActionType waterAttack 	= new ActionType(Letters.ATTACK, Letters.WATER);
//	ActionType waterDefend 	= new ActionType(Letters.DEFEND, Letters.WATER);

	void Start()
	{
//		Debug.Log(fireAttack.GetHashCode());
//		availableActions.Add(fireAttack.GetHashCode(), 0);
//		availableActions.Add(fireDefend.GetHashCode(), 0);
//		availableActions.Add(waterAttack.GetHashCode(), 0);
//		availableActions.Add(waterDefend.GetHashCode(), 0);

		playerInput = GetComponent<PlayerInput>();

		Debug.Log(Letters.ATTACK.GetHashCode());
		Debug.Log(Letters.FIRE.GetHashCode());

		availableActions.Add(Letters.ATTACK.GetHashCode() + Letters.FIRE.GetHashCode(), 0);
	}

	void Update()
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
			Debug.Log("Agregado: " + letterToQueue);
		}
		else
		{
			int action = word.Dequeue().GetHashCode() + word.Dequeue().GetHashCode();
			int result;

			if(availableActions.TryGetValue(action, out result))
				Debug.Log("Debería ejecutarse: " + result);
			else
				Debug.Log("Option not available");
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

	void ExecuteAction(ActionType action)
	{
		
	}
}
