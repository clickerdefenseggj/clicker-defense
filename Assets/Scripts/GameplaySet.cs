using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameplaySet : Set {

    public Text ScoreText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (ScoreText)
            ScoreText.text = "Score: " + Player.Inst.GetScore();
	}

    public void OnFinishLevel()
    {
        new GameSparks.Api.Requests.LogEventRequest().SetEventKey("SUBMIT_SCORE").SetEventAttribute("SCORE", Player.Inst.GetScore()).Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Score Posted Successfully...");
            }
            else {
                Debug.Log("Error Posting Score...");
            }
        });

        App.inst.IsRunning = false;

        SetManager.OpenSet<LeaderboardSet>();

        CloseSet();
    }
}
