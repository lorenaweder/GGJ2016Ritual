using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CambiarGameMechanics : MonoBehaviour {
	void Update () {

		if(Input.anyKey){
			SceneManager.LoadScene ("Tutorial3");
		}

	}
}
