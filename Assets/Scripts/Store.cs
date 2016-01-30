using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class Store
{
    public List<Upgrade> upgrades = new List<Upgrade>()
    {
       new Upgrade(
           "castle",
           "repair"),
       new Upgrade(
           "castle",
           "fortify"),
       new Upgrade(
           "ammo",
           "area of effect"),
       new Upgrade(
           "ammo",
           "damage"),
    };

    public List<Upgrade> GetAvailibleUpgrades()
    {
        var player = Player.Inst;
        return upgrades.Where(u => u.IsAvailable(player)).ToList();
    }
}

public class Upgrade
{
    public Func<Player, bool> filter;
    public string name;
    public string category;

    public Upgrade(string category, string name, Func<Player, bool> filter = null)
    {
        this.category = category;
        this.name = name;
        this.filter = filter;
    }

    public bool IsAvailable(Player player)
    {
        if (filter != null && !filter(player))
            return false;



        return true;
    }
}
