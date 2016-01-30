using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public Sprite attackRune, defenseRune, fireRune, waterRune, airRune, earthRune;
	public Actor[] players;
	public static Dictionary<Letters, Sprite> runeByLetter = new Dictionary<Letters, Sprite>();

	void Start()
	{
		runeByLetter.Add(Letters.ATTACK, attackRune);
		runeByLetter.Add(Letters.DEFEND, defenseRune);
		runeByLetter.Add(Letters.FIRE, fireRune);
		runeByLetter.Add(Letters.WATER, waterRune);
		runeByLetter.Add(Letters.AIR, airRune);
		runeByLetter.Add(Letters.EARTH, earthRune);
	}

	void Update()
	{
		for(int i = 0; i < players.Length; ++i)
		{
			players[i].CustomUpdate(Time.deltaTime);
		}
	}

}
