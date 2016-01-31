using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StoreSet : Set
{
    public Text titleText;
    public RectTransform content;
    public Text cashCountText;

    public StoreCategoryElement categoryPrefab;
    public StoreUpgradeElement upgradePrefab;

    void Start()
    {
        App.inst.IsRunning = false;

        var upgrades = GameData.GetAvailibleUpgrades();
        var groups = upgrades.GroupBy(u => u.category);

        foreach (var child in content)
        {
            Destroy(((Transform)child).gameObject);
        }

        foreach (var grouping in groups)
        {
            var cat = Instantiate(categoryPrefab);
            cat.transform.SetParent(content, false);
            cat.Initialize(grouping.Key);

            var anchor = cat.content;
            foreach (var upgrade in grouping)
            {
                var up = Instantiate(upgradePrefab);
                up.transform.SetParent(anchor, false);
                up.Initialize(upgrade);
            }
        }
    }

    void Update()
    {
        if (Player.Inst)
        {
            cashCountText.text = Player.Inst.Cash.ToString();
        }
    }

    public void OnCloseClicked()
    {
        App.inst.IsRunning = true;

        CloseSet();
        SetManager.OpenSet<WaveNumberSet>();
    }
}