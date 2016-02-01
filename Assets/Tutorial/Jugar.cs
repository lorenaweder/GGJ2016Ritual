using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Jugar : MonoBehaviour {

	void Update () {

		if(Input.anyKey){
			SceneManager.LoadScene ("Main");
		}

	}
}
