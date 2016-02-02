using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuSet : Set
{
    public GameObject NamePopup;
    public Text NameText;
    public Button LoadButton;
    public Image LoadingContainer;

	// Use this for initialization
	void Start ()
    {
        if (Player.Inst.Load())
            NamePopup.SetActive(false);
        else
            NamePopup.SetActive(true);

        /*if(Player.Inst.Load() == false)
        {
            LoadButton.gameObject.SetActive(false);
        }*/
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey("escape"))
        {
            SetManager.OpenSet<ExitConfirmPopup>();
        }
    }

    public void OnStartPressed()
    {
        App.inst.StartGameplay();
        CloseSet();
    }

    public void OnSubmitName()
    {
        Player.Inst.CreateLocalSave(NameText);
        NamePopup.SetActive(false);
    }

    public void OnExitPressed()
    {
        SetManager.OpenSet<ExitConfirmPopup>();
    }

    public void OnLoadPressed()
    {
        Player.Inst.Load();
        App.inst.StartGameplay();
        CloseSet();
    }

    public void OnLeaderboardPressed()
    {
        SetManager.OpenSet<LeaderboardSet>();
        CloseSet();
    }

}
