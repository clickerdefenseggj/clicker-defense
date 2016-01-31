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
            { EnemyType.Bandit, new EnemyTemplate() { name = "Bandit", scoreValue = 15, goldValue = 10, maxHealth = 200, speed = 6.0f, damage = 5, attackRate = 1  } },
            { EnemyType.Giant, new EnemyTemplate() { name = "Giant", scoreValue = 50, goldValue = 40, maxHealth = 650, speed = 2.0f, damage = 20, attackRate = 2  } },
            { EnemyType.Goblin, new EnemyTemplate() { name = "Goblin", scoreValue = 30, goldValue = 15, maxHealth = 100, speed = 8.0f, damage = 3, attackRate = 1 } }
        };

    public static List<AchievementTemplate> achievementTemplates =
        new List<AchievementTemplate>()
        {
            { new AchievementTemplate() { title = "Get out of here!", description = "Kill an enemy", icon = "kill1", type = "kill", number = 1, awarded = false} },
            { new AchievementTemplate() { title = "Deterring the Masses", description = "Kill 10 enemy units", icon = "kill10", type = "kill", number = 10, awarded = false} },
            { new AchievementTemplate() { title = "Expelling the Masses", description = "Kill 25 enemy units", icon = "kill25", type = "kill", number = 25, awarded = false} },
            { new AchievementTemplate() { title = "Anihillating the Masses", description = "Kill 50 enemy units", icon = "kill50", type = "kill", number = 50, awarded = false} },
            { new AchievementTemplate() { title = "The big five oh", description = "Score 50 points", icon = "score50", type = "score", number = 50, awarded = false} },
            { new AchievementTemplate() { title = "What's this score thing?", description = "Score 100 points", icon = "score100", type = "score", number = 100, awarded = false} },
            { new AchievementTemplate() { title = "Not just a number", description = "Score 250 points", icon = "score250", type = "score", number = 250, awarded = false} },
            { new AchievementTemplate() { title = "The 500 club", description = "Score 500 points", icon = "score500", type = "score", number = 500, awarded = false} },
            { new AchievementTemplate() { title = "A thousand point-y things", description = "Score 1000 points", icon = "score1000", type = "score", number = 1000, awarded = false} }
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
                            new CinematicSet.Sentence() { Words = "Guard: Uhh.... Your Majesty?", OwningTextBox = CinematicSet.Speaker.Guard},
                            new CinematicSet.Sentence() { Words = "King: What is it?!  Can't you see we're being attacked!", OwningTextBox = CinematicSet.Speaker.King},
                            new CinematicSet.Sentence() { Words = "Guard: We are out of cannonballs...", OwningTextBox = CinematicSet.Speaker.Guard},
                            new CinematicSet.Sentence() { Words = "King: What do you mean, we're out of cannonballs?!", OwningTextBox = CinematicSet.Speaker.King},
                            new CinematicSet.Sentence() { Words = "Guard: Well, we had to make room for all the stuff you are hoarding. There was no room for them.", OwningTextBox = CinematicSet.Speaker.Guard},
                            new CinematicSet.Sentence() { Words = "King: What do you suggest we do then?", OwningTextBox = CinematicSet.Speaker.King},
                            new CinematicSet.Sentence() { Words = "Guard: There's really only one option...", OwningTextBox = CinematicSet.Speaker.Guard},
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
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "Where are they getting these things from?!", OwningTextBox = CinematicSet.Speaker.Enemy}
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
               int upAmount = 10;
               p.MaxHealth += upAmount;
               p.CurrentHealth += upAmount;
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
        if (player.Cash >= cost && apply != null && apply(player))
        {
            player.Cash -= cost;
            player.SetUpgradeLevel(name, player.GetUpgradeLevel(name) + 1);
            return true;
        }
        return false;
    }
}
