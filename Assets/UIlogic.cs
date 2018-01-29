using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.SceneManagement;


public class UIlogic : MonoBehaviour {
	
	public GameObject startUI, restartUI;
	public GameObject startPoint, pointOne, pointTwo, pointThree, pointFour, pointFive, pointSix, restartPoint;
	public GameObject player;
	public GameObject eventSystem;


	void Start()
	{
		// Update 'player' to be the camera's parent gameobject, i.e. 'GvrEditorEmulator' instead of the camera itself.
		// Required because GVR resets camera position to 0, 0, 0.
		player = player.transform.parent.gameObject;

		iTween.MoveTo (player,
			iTween.Hash (
				"position", startPoint.transform.position,
				"time", 2,
				"easetype", "easeOutQuad"
			)
		);
	}


	// Begin the puzzle sequence.
	public void startgame ()
	{
		// Disable the start UI.
		startUI.SetActive(false);

		// Move the player to the play position.
		iTween.MoveTo(player,
			iTween.Hash(
				"position", pointOne.transform.position,
				"time", 20,
				"easetype", "easeOutQuad"
			)
			);

	}
		public void stoptwo ()

		{// Move the player to the play position.
			iTween.MoveTo(player,
				iTween.Hash(
					"position", pointTwo.transform.position,
					"time", 6,
				"easetype", "easeOutQuad"	)
			);

		}
		public void stopthree ()

		{// Move the player to the play position.
			iTween.MoveTo(player,
				iTween.Hash(
					"position", pointThree.transform.position,
					"time", 6,
				"easetype", "easeOutQuad"	)
			);

		}

		public void stopfour ()

		{// Move the player to the play position.
			iTween.MoveTo(player,
				iTween.Hash(
					"position", pointFour.transform.position,
					"time", 6,
				"easetype", "easeOutQuad"	)
			);

		}
		public void stopfive ()

		{// Move the player to the play position.
			iTween.MoveTo(player,
				iTween.Hash(
					"position", pointFive.transform.position,
					"time", 6,
				"easetype", "easeOutQuad"	)
			);

		}

		public void stopsix ()

		{// Move the player to the play position.
			iTween.MoveTo(player,
				iTween.Hash(
					"position", pointSix.transform.position,
					"time", 6,
				"easetype", "easeOutQuad"	)
			);

		}

	public void endpoint ()

	{// Move the player to the play position.

		// Enable the restart UI.
		restartUI.SetActive (true);

		iTween.MoveTo(player,
			iTween.Hash(
				"position", restartPoint.transform.position,
				"time", 5,
				"easetype", "easeInOutQuad"	)
		);

	}

	// Reset the puzzle sequence.
	public void restarttrip()
	{
		// Enable the start UI.
		startUI.SetActive(true);

		// Disable the restart UI.
		restartUI.SetActive(false);

		// Move the player to the start position.
		//player.transform.position = startPoint.transform.position;

		UnityEngine.SceneManagement.SceneManager.LoadScene ("Woman");

	}
		
	public void Goback()
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene ("Woman");		
	}
			
	}


