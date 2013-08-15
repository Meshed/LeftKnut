using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        float buttonWidth = 100f;
        float buttonHight = 40f;
        float halfButtonWidth = buttonHight / 2f;
        float halfScreenWitdh = Screen.width / 2f;

        if (GUI.Button(new Rect(halfScreenWitdh - halfButtonWidth, Screen.height - buttonHight - 40, buttonWidth, buttonHight), "Main Menu"))
        {
            Application.LoadLevel("MainMenu");
        }
    }
}
