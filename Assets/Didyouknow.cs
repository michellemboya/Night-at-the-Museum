using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Didyouknow : MonoBehaviour {

	public GameObject bar, didyoutext;

	// Use this for initialization
	void OnbarClicked () {

		ShowWord();
	}

	// Update is called once per frame
	public void ShowWord () {

		bar.SetActive(!bar.activeSelf);
		didyoutext.SetActive(!didyoutext.activeSelf);

	}
}
