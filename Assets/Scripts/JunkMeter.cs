using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JunkMeter : MonoBehaviour {
    public GameObject label;
    public GameObject layout;

    public Image fillImage;
    public Text counter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (App.inst)
        {
            label.SetActive(!App.inst.UseCannonball && App.inst.IsRunning);
            layout.SetActive(!App.inst.UseCannonball && App.inst.IsRunning);

            if (Player.Inst)
            {
                counter.text = string.Format("{0}/{1}", Player.Inst.CurrentJunk, Player.Inst.MaxJunk);

                float secondsPerJunk = 60f / Player.Inst.JunkPerMinute;
                float seconds = Player.Inst.JunkRegenSeconds;
                fillImage.fillAmount = seconds / secondsPerJunk;
            }
        }
	}
}
