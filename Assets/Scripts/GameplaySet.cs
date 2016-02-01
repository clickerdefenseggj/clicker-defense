using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameplaySet : Set {
    public Text healthText;
    public Text ScoreText;
    public Image HealthFillbar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (ScoreText)
            ScoreText.text = "Score: " + Player.Inst.GetScore();

        if (HealthFillbar)
            HealthFillbar.fillAmount = Player.Inst.CurrentHealth / Player.Inst.MaxHealth;

        if (healthText)
            healthText.text = Player.Inst.CurrentHealth + "/" + Player.Inst.MaxHealth;

    }
}
