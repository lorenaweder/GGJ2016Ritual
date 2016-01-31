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
		SceneManager.LoadScene("Menu");
	}

	public void Lost(float player)
	{
		Debug.Log("dafuwasdasdkjhsadkjhsadjkashdkjhaskdjhkasjdhka");
		Game.gameOver = true;
		if(player > 1)
			SceneManager.LoadScene("WinP2");
		else
			SceneManager.LoadScene("WinP1");
	}
}
