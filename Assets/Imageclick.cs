using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imageclick : MonoBehaviour {

	public GameObject womanimage, womantext;

	// Use this for initialization
	void OnwomanimageClicked () {

		ShowWord();
	}

	// Update is called once per frame
	public void ShowWord () {

		womanimage.SetActive(!womanimage.activeSelf);
		womantext.SetActive(!womantext.activeSelf);

	}
}
