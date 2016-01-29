using UnityEngine;
using System.Collections;
public class MainMenuSet : Set
{
	// Use this for initialization
	void Start ()
    {
        
    
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void OnStartPressed()
    {
        new GameSparks.Api.Requests.DeviceAuthenticationRequest().SetDisplayName("DisplayName").Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Device Authenticated...");
            }
            else {
                Debug.Log("Error Authenticating Device...");
            }
        });

        CloseSet();
    }
}
