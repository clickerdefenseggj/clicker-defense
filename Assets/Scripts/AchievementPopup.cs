using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AchievementPopup : MonoBehaviour {

    public GameObject PopupWindow;
    public Text Title;
    public Text Description;

    private bool IsDisplayed = false;
    private float TimeToDisplay = 3;
    private float CurrentDisplayTime = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(IsDisplayed)
        {
            CurrentDisplayTime += Time.deltaTime;

            if(CurrentDisplayTime >= TimeToDisplay)
            {
                PopupWindow.SetActive(false);
                IsDisplayed = false;
                CurrentDisplayTime = 0;
            }
        }

        // Check for achievements
        foreach(var achievement in GameData.achievementTemplates)
        {
            switch(achievement.type)
            {
                case "kill":
                    if(!achievement.awarded)
                    {
                        if(!achievement.awarded && Player.Inst.NumberKilled >= achievement.number)
                        {
                            Title.text = achievement.title;
                            Description.text = achievement.description;
                            PopupWindow.SetActive(true);
                            IsDisplayed = true;
                            achievement.awarded = true;
                        }
                    }
                    break;
                case "score":

                    if (!achievement.awarded && Player.Inst.GetScore() >= achievement.number)
                    {
                        Title.text = achievement.title;
                        Description.text = achievement.description;
                        PopupWindow.SetActive(true);
                        IsDisplayed = true;
                        achievement.awarded = true;
                    }
                    break;
                default:
                    break;

            }
        }

	}
}
