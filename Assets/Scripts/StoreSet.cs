using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StoreSet : Set
{
    public Text TitleText;
    public RectTransform Content;

    public StoreCategoryElement categoryPrefab;
    public StoreUpgradeElement upgradePrefab;

    void Start()
    {
        App.inst.IsRunning = false;

        var upgrades = GameData.GetAvailibleUpgrades();
        var groups = upgrades.GroupBy(u => u.category);

        foreach (var child in Content)
        {
            Destroy(((Transform)child).gameObject);
        }

        foreach (var grouping in groups)
        {
            var cat = Instantiate(categoryPrefab);
            cat.transform.SetParent(Content, false);
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

    public void OnCloseClicked()
    {
        App.inst.IsRunning = true;

        CloseSet();
        SetManager.OpenSet<WaveNumberSet>();
    }
}