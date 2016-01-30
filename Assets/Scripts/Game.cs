using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {

	public Actor[] players;

	void Update()
	{
		for(int i = 0; i < players.Length; ++i)
		{
			players[i].CustomUpdate(Time.deltaTime);
		}
	}

}
