using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Makechild : MonoBehaviour {
	public GameObject camcam, movingcube;
	public bool deatachchild;
	public float delay = 30f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void makechild () {
		camcam.transform.parent = movingcube.transform;
		StartCoroutine(LoadLevelAfterDelay(delay));

}
	IEnumerator LoadLevelAfterDelay(float delay)
	{
		yield return new WaitForSeconds (delay);
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Network");
	}
	void Update ()
	{

		if (deatachchild == true) {
			camcam.transform.parent = null;
		}
	}
	public void OnbuttonClicked() {

			deatachchild = true;
		} 

	}