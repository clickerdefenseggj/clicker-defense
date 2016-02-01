using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuSet : Set
{
    public GameObject NamePopup;
    public Text NameText;

	// Use this for initialization
	void Start ()
    {
        if (Player.Inst.Load())
            NamePopup.SetActive(false);
        else
            NamePopup.SetActive(true);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey("escape"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }

    public void OnStartPressed()
    {
        if (App.currBgm)
            SoundManager.StopClip(App.currBgm);
        App.currBgm = SoundManager.PlayBgm("bgm/gameplay_music");

        new GameSparks.Api.Requests.DeviceAuthenticationRequest().SetDisplayName(Player.Inst.Name).Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Device Authenticated...");
            }
            else {
                Debug.Log("Error Authenticating Device...");
            }
        });

        App.inst.IsRunning = true;

        if (App.gameplaySet == null)
            App.gameplaySet = SetManager.OpenSet<GameplaySet>();
        SetManager.OpenSet<WaveNumberSet>();

        CloseSet();
    }

    public void OnSubmitName()
    {
        Player.Inst.SetName(NameText);

        NamePopup.SetActive(false);
    }

    public void OnExitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

}
