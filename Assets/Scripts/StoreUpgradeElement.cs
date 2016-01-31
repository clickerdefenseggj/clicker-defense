using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StoreUpgradeElement : MonoBehaviour
{
    public Image image;
    public Text titleText;

    public Upgrade upgrade;

	// Use this for initialization
	public void Initialize(Upgrade upgrade)
    {
        this.upgrade = upgrade;

        titleText.text = upgrade.name;
	}

    public void OnClicked()
    {

    }
}
