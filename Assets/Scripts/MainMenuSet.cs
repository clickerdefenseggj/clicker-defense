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
	
	}

    public void OnStartPressed()
    {
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

        SetManager.OpenSet<GameplaySet>();
        SetManager.OpenSet<WaveNumberSet>();

        CloseSet();
    }

    public void OnSubmitName()
    {
        Player.Inst.SetName(NameText);

        NamePopup.SetActive(false);
    }

}
