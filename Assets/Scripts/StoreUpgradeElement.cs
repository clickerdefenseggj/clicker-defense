using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreUpgradeElement : MonoBehaviour
{
    public Image image;
    public Text titleText;
    public Text costText;
    public Button button;

    public Upgrade upgrade;

	// Use this for initialization
	public void Initialize(Upgrade upgrade)
    {
        this.upgrade = upgrade;

        if (upgrade != null)
        {
            titleText.text = upgrade.name;
        }
        UpdateState();
	}

    public void UpdateState()
    {
        if (upgrade != null)
        {
            int cost = upgrade.GetCost(Player.Inst);
            costText.text = cost.ToString();
            button.interactable = Player.Inst.Cash >= cost;
        }
    }

    public void OnClicked()
    {
        if (upgrade != null)
        {
            if (upgrade.Purchase(Player.Inst))
            {
                UpdateState();
            }
        }
    }
}
