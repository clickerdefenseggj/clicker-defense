using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PauseSet : Set {

	// Use this for initialization
	void Start () {
        App.inst.Pause();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) { Close(); }

    }

    public void OnResumePressed()
    {
        Close();
    }

    public void OnSavePressed()
    {
        Player.Inst.Save();
    }

    public void OnMenuPressed()
    {
        App.inst.Reset();
        SetManager.OpenSet<MainMenuSet>();

        StopCoroutine(App.inst.PlayLevel1CinematicCoroutine());

        CloseSet();

    }

    public void OnExitPressed()
    {
        SetManager.OpenSet<ExitConfirmPopup>();
    }

    void Close()
    {
        App.inst.Unpause();
        CloseSet();
    }
}
