using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public static bool gameOver = false;
	public Sprite emptyRune, attackRune, defenseRune, fireRune, waterRune, airRune, earthRune;
	public Sprite fireAttack, waterAttack, airAttack, earthAttack;
	public Actor[] players;

	public ADParams fireAttackParams, waterAttackParams, airAttackParams, earthAttackParams;
	public ADParams fireDefenseParams, waterDefenseParams, airDefenseParams, earthDefenseParams;

	public static Dictionary<Elements, ADParams> attackParamsByType;
	public static Dictionary<Elements, ADParams> defenseParamsByType;
	public static Dictionary<Letters, Sprite> runeByLetter;
	public static Dictionary<Elements, Sprite> attackByType;

	public static SoundManager soundManager;
	public static GameManager gameManager;

	void Awake()
	{	
	
		attackParamsByType = new Dictionary<Elements, ADParams>();
		defenseParamsByType = new Dictionary<Elements, ADParams>();
		runeByLetter = new Dictionary<Letters, Sprite>();
		attackByType = new Dictionary<Elements, Sprite>();

		runeByLetter.Add(Letters.NONE, emptyRune);
		runeByLetter.Add(Letters.ATTACK, attackRune);
		runeByLetter.Add(Letters.DEFEND, defenseRune);
		runeByLetter.Add(Letters.FIRE, fireRune);
		runeByLetter.Add(Letters.WATER, waterRune);
		runeByLetter.Add(Letters.AIR, airRune);
		runeByLetter.Add(Letters.EARTH, earthRune);

		attackByType.Add(Elements.FIRE, fireAttack);
		attackByType.Add(Elements.WATER, waterAttack);
		attackByType.Add(Elements.AIR, airAttack);
		attackByType.Add(Elements.EARTH, earthAttack);

		attackParamsByType.Add(Elements.FIRE, fireAttackParams);
		attackParamsByType.Add(Elements.WATER, waterAttackParams);
		attackParamsByType.Add(Elements.AIR, airAttackParams);
		attackParamsByType.Add(Elements.EARTH, earthAttackParams);

		defenseParamsByType.Add(Elements.FIRE, fireDefenseParams);
		defenseParamsByType.Add(Elements.WATER, waterDefenseParams);
		defenseParamsByType.Add(Elements.AIR, airDefenseParams);
		defenseParamsByType.Add(Elements.EARTH, earthDefenseParams);

	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			gameManager.QuitToMenu();
			return;
		}

		if(gameOver)
			return;
		for(int i = 0; i < players.Length; ++i)
		{
			players[i].CustomUpdate(Time.deltaTime);
		}
	}
}
