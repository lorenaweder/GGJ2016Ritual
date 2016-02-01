using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	void Start()
	{
		Game.gameManager = this;
	}

	public void QuitToMenu()
	{
		CleanUp();
		SceneManager.LoadScene("Menu");
	}

	public void Lost(float player)
	{
		Game.gameOver = true;
		CleanUp();
		if(player > 1)
			SceneManager.LoadScene("WinP2");
		else
			SceneManager.LoadScene("WinP1");
	}

	void CleanUp()
	{
		Game.attackParamsByType = null;
		Game.defenseParamsByType  = null;
		Game.runeByLetter  = null;
		Game.attackByType = null;
	}
}
