using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BotonPlay : MonoBehaviour {

    public void ClickPlay()
    {
		SceneManager.LoadScene ("Tutorial");
        print("Empece a jugar");
    }
}
