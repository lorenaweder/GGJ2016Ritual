using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CambiarPlayer2 : MonoBehaviour {

	// Use this for initialization
	void Update () {

		if(Input.anyKey){
			SceneManager.LoadScene ("Tutorial2");
		}

	}
}
