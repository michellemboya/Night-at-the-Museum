using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	void Update()
	{
		// ...also rotate around the World's Y axis
		transform.Rotate(Vector3.up * Time.deltaTime * 20, Space.World);
	}
}