using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveNumberSet : Set
{
    public Text WaveNumberText;

    float CurrentTimeShown = 0;
    float TimeToShow = 4.0f;

    // Use this for initialization
    void Start()
    {
        if (WaveNumberText)
            WaveNumberText.text = "ROUND " + App.inst.SpawnController.CurrentWave.ToString();

        App.PauseGameplayMusic();

        SoundManager.PlayClip("sfx/wave_start");

        App.inst.IsRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimeShown += Time.deltaTime;

        if (CurrentTimeShown > TimeToShow)
        {
            App.inst.ChooseRanomSkybox();
            App.inst.IsRunning = true;
            App.PlayGameplayMusic();
            App.inst.SpawnController.Reset();

            if (App.inst.SpawnController.CurrentWave == 1)
            {
                App.inst.PlayLevel1Cinematic();
            }

            CloseSet();
        }
    }

    public void OnDestroy()
    {
    }
}
