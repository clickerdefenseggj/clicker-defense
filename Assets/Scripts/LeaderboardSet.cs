using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeaderboardSet : Set {

    public RectTransform ScoresAnchor;

    public Button BackButton;
    public Button MainMenuButton;
    public Button PlayAgainButton;

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
                    scoreItem.transform.localScale = Vector3.one;
                }
            }
            else {
                Debug.Log("Error Retrieving Leaderboard Data...");
            }
        });
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu)){
            GoToMainMenu();
        }

    }

    public void GoToMainMenu()
    {
        App.inst.Reset();

        SetManager.OpenSet<MainMenuSet>();

        CloseSet();
    }

    public void OnPlayAgainPressed()
    {
        SetManager.OpenSet<GameplaySet>();

        App.inst.Reset();

        SetManager.OpenSet<GameplaySet>();

        CloseSet();
    }

    public void OnMainMenuPressed()
    {
        GoToMainMenu();
    }

    public void OnBackPressed()
    {
        SetManager.OpenSet<MainMenuSet>();

        CloseSet();
    }
}
