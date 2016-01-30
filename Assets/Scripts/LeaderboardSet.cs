using UnityEngine;
using System.Collections;

public class LeaderboardSet : Set {

    public RectTransform ScoresAnchor;

	// Use this for initialization
	void Start () {
        new GameSparks.Api.Requests.LeaderboardDataRequest().SetLeaderboardShortCode("HIGHSCORE_LEADERBOARD").SetEntryCount(100).Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Found Leaderboard Data...");
                foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                {
                    int rank = (int)entry.Rank;
                    string playerName = entry.UserName;
                    string score = entry.JSONData["SCORE"].ToString();
                    Debug.Log("Rank:" + rank + " Name:" + playerName + " \n Score:" + score);

                    ScoreItem scoreItem = App.Create("ScoreItem").GetComponent<ScoreItem>();
                    scoreItem.RankText.text = rank.ToString();
                    scoreItem.NameText.text = playerName;
                    scoreItem.ScoreText.text = score;
                    scoreItem.transform.SetParent(ScoresAnchor);
                }
            }
            else {
                Debug.Log("Error Retrieving Leaderboard Data...");
            }
        });
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBackPressed()
    {
        Player.Inst.Reset();

        App.inst.SpawnController.Reset();

        App.inst.IsRunning = true;

        SetManager.OpenSet<GameplaySet>();

        CloseSet();
    }
}
