using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Main : MonoBehaviour {

	public void PlayGame () {
		SceneManager.LoadScene ("TrainingLevel");
	}

	public void OptionsMenu () {

	}

	public void QuitApplication () {
		Debug.Log ("Quitting...");
		Application.Quit();
	}
}
