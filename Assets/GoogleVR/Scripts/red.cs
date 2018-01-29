using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class red : MonoBehaviour {

	public GameObject redbarrel, redtext;

	// Use this for initialization
	void OnredbarrelClicked () {

		ShowWord();
	}
	
	// Update is called once per frame
	public void ShowWord () {

		redbarrel.SetActive(!redbarrel.activeSelf);
		redtext.SetActive(!redtext.activeSelf);
		
	}
}
