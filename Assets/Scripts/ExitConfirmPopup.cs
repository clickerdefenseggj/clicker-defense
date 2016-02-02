using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ExitConfirmPopup : Set {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) { App.inst.Quit(); }

    }

    public void OnYesPressed()
    {
        App.inst.Quit();
    }

    public void OnNoPressed()
    {
        CloseSet();
    }
}
