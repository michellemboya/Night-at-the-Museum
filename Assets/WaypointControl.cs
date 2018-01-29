using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointControl : MonoBehaviour {

	public GameObject player;

	public float height = 2;
	public bool teleport = true;
	public float maxMoveDistance = 150;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void Focus(GameObject waypoint) {
		waypoint.transform.localScale *= 1.25f;
		waypoint.transform.GetChild(0).gameObject.SetActive(true);
	}

	public void Unfocus(GameObject waypoint) {
		waypoint.transform.localScale /= 1.25f;
		waypoint.transform.GetChild(0).gameObject.SetActive(false);
	}

	public void Move(GameObject waypoint) {
		if (!teleport) {
			iTween.MoveTo(player,
				iTween.Hash(
					"position", new Vector3(waypoint.transform.position.x, waypoint.transform.position.y + height / 2, waypoint.transform.position.z),
					"time", .2F,
					"easetype", "linear"
				)
			);
		} else {
			player.transform.position = new Vector3(waypoint.transform.position.x,
				waypoint.transform.position.y + height / 2,
				waypoint.transform.position.z);
		}

	}

}

