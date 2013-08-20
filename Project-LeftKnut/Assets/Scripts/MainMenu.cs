using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

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
        float halfButtonWidth = buttonHight/2f;
        float halfScreenWitdh = Screen.width/2f;

        if (GUI.Button(new Rect(halfScreenWitdh - halfButtonWidth, Screen.height - (buttonHight*3) - (20*3), buttonWidth, buttonHight), "Play Game"))
        {
            Application.LoadLevel("Level");
        }
        if (GUI.Button(new Rect(halfScreenWitdh - halfButtonWidth, Screen.height - (buttonHight*2) - (20*2), buttonWidth, buttonHight), "Credits"))
        {
            Application.LoadLevel("Credits");
        }
        if (GUI.Button(new Rect(halfScreenWitdh - halfButtonWidth, Screen.height - buttonHight - 20, buttonWidth, buttonHight), "Exit"))
        {
            Application.Quit();
        }
    }
}
