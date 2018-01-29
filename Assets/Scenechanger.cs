using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scenechanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void LoadWoman()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Woman");
		
	}
	public void LoadMusic()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Music");

	}
	public void LoadCompanies()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Companies");

	}
	public void LoadGraph()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Graph");

	}
	public void LoadNetwork()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Network");

	}
	public void LoadMuseum()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Museum");

	}
}

