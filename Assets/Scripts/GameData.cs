using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum EnemyType
{
    Bandit,
    Giant,
    Goblin
}

public static class GameData
{
    public static Dictionary<EnemyType, EnemyTemplate> enemyTemplates =
        new Dictionary<EnemyType, EnemyTemplate>()
        {
            { EnemyType.Bandit, new EnemyTemplate() { name = "Bandit", scoreValue = 5, goldValue = 10, maxHealth = 200, speed = 2.0f, damage = 5, attackRate = 1  } },
            { EnemyType.Giant, new EnemyTemplate() { name = "Giant", scoreValue = 12, goldValue = 40, maxHealth = 650, speed = 1.0f, damage = 20, attackRate = 2  } },
            { EnemyType.Goblin, new EnemyTemplate() { name = "Goblin", scoreValue = 7, goldValue = 15, maxHealth = 100, speed = 3.5f, damage = 3, attackRate = 1 } }
        };

    public static Dictionary<string, AchievementTemplate> achievementTemplates =
        new Dictionary<string, AchievementTemplate>()
        {
            { "kill10enemies", new AchievementTemplate() { title = "Deterring the Masses", description = "Kill 10 enemy units", icon = "kill10"} },
            { "kill25enemies", new AchievementTemplate() { title = "Expelling the Masses", description = "Kill 25 enemy units", icon = "kill25"} },
            { "kill50enemies", new AchievementTemplate() { title = "Anihillating the Masses", description = "Kill 50 enemy units", icon = "kill50"} }
        };

    public static Dictionary<CinematicSet.Type, List<CinematicSet.Conversation>> Cinematics =
        new Dictionary<CinematicSet.Type, List<CinematicSet.Conversation>>()
        {
            // HOARDER CONVERSATION
            {
                CinematicSet.Type.HoarderConversation, new List<CinematicSet.Conversation> () 
                { 
                    new CinematicSet.Conversation() 
                    {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "Uhh.... Your Majesty?", OwningTextBox = CinematicSet.Speaker.Guard},
                            new CinematicSet.Sentence() { Words = "What is it?!  Can't you see we're being attacked!", OwningTextBox = CinematicSet.Speaker.King},
                            new CinematicSet.Sentence() { Words = "We are out of cannonballs...", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.1f,
                    }
                }
            },
            // RANDOM EXCLAMATIONS
            {
                CinematicSet.Type.RandomExclamation, new List<CinematicSet.Conversation>() {
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "I MIGHT NEED THAT LATER!  STOP!!", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.2f
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "How about this, vile scum!", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.1f,
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "Not the gumdrop buttons!", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.05f,
                    }
                }
            }
        };

    public static List<Upgrade> upgrades = new List<Upgrade>()
    {
       new Upgrade(
           "castle",
           "repair",
           cost: l => 100,
           apply: p =>
           {
               if (p.CurrentHealth < p.MaxHealth)
               {
                   p.CurrentHealth = Mathf.Min(p.CurrentHealth + 10f, p.MaxHealth);
                   return true;
               }
               return false;
           }),
       new Upgrade(
           "castle",
           "fortify",
           cost: l => l * 200,
           apply: p =>
           {
               p.MaxHealth += 10;
               return true;
           },
           maxLevel: 10),
       new Upgrade(
           "ammo",
           "area of effect"),
       new Upgrade(
           "ammo",
           "damage"),
    };

    public static List<Upgrade> GetAvailibleUpgrades()
    {
        var player = Player.Inst;
        return upgrades.Where(u => u.IsAvailable(player)).ToList();
    }
}

public class Upgrade
{
    public string name;
    public string category;
    public Func<Player, bool> filter;
    public Func<int, int> cost;
    public Func<Player, bool> apply;

    public int maxLevel;

    public Upgrade(string category, string name, Func<Player, bool> filter = null, Func<int, int> cost = null, Func<Player, bool> apply = null, int maxLevel = 0)
    {
        this.category = category;
        this.name = name;

        this.filter = filter;
        this.cost = cost;
        this.apply = apply;

        this.maxLevel = maxLevel;
    }

    public int GetCost(Player player)
    {
        return cost != null ? cost(player.GetUpgradeLevel(name) + 1) : 0;
    }

    public bool IsAvailable(Player player)
    {
        if (maxLevel > 0 && player.GetUpgradeLevel(name) >= maxLevel)
            return false;

        if (filter != null && !filter(player))
            return false;

        return true;
    }

    public bool Purchase(Player player)
    {
        int cost = GetCost(player);
        if (player.Cash > cost && apply != null && apply(player))
        {
            player.Cash -= cost;
            player.SetUpgradeLevel(name, player.GetUpgradeLevel(name) + 1);
            return true;
        }
        return false;
    }
}
