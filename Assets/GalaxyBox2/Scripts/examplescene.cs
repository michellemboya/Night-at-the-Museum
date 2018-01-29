using UnityEngine;
using System.Collections;

public class examplescene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        current = (Material)Resources.Load("Skybox2_1/Skybox2_1");
        RenderSettings.skybox = current; 
        GameObject.Find("Main Camera").AddComponent<SmoothMouseLook>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (current == null)
            return;

        current.SetFloat("_Rotation", rotation);
    }

    Material current;

    float rotation;

	void OnGUI() {
		int x = 50;
		int y = 50;
		int dy = 40;
		int cnt = 0;
		int sx = 300;
		int sy = 30;
        for (int i=1;i<=11;i++)

		if (GUI.Button(new Rect(x, y+dy*cnt++, sx, sy), "Skybox " +i)) {
                current = (Material)Resources.Load("Skybox2_" + i + "/Skybox2_" + i);
                RenderSettings.skybox = current;
		}

        cnt = 0;
        float ssx = 150;
        GUI.Label(new Rect(x,Screen.height-dy*cnt++-y, 300,sy), "Double-click and drag to rotate view");

        cnt++;
        GUI.Label(new Rect(x, Screen.height - cnt * dy - y, ssx, dy), "Rotation:");
        rotation = GUI.HorizontalSlider(new Rect(x + ssx, Screen.height - cnt * dy - y, 2 * sx, dy), rotation, 0, 360);

    }
}
