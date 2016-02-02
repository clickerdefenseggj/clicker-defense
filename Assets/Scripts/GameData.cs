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
            { new AchievementTemplate() { title = "Expelling the Masses", description = "Kill 25 enemy units", icon = "kill25", type = "kill", number = 25, awarded = false} },
            { new AchievementTemplate() { title = "This Is Sparta", description = "Kill 300 enemy units", icon = "kill300", type = "kill", number = 300, awarded = false} },
            { new AchievementTemplate() { title = "The big five oh", description = "Score 50 points", icon = "score50", type = "score", number = 50, awarded = false} },
            { new AchievementTemplate() { title = "What's this score thing?", description = "Score 100 points", icon = "score100", type = "score", number = 100, awarded = false} },
            { new AchievementTemplate() { title = "Not just a number", description = "Score 250 points", icon = "score250", type = "score", number = 250, awarded = false} },
            { new AchievementTemplate() { title = "The 500 club", description = "Score 500 points", icon = "score500", type = "score", number = 500, awarded = false} },
            { new AchievementTemplate() { title = "A thousand point-y things", description = "Score 1000 points", icon = "score1000", type = "score", number = 1000, awarded = false} },
            { new AchievementTemplate() { title = "5000 Points On The Wall", description = "Score 5000 points", icon = "score5000", type = "score", number = 5000, awarded = false} },
            { new AchievementTemplate() { title = "Over 9000", description = "Score OVER 9000 points", icon = "score5000", type = "score", number = 5000, awarded = false} }

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
                            new CinematicSet.Sentence() { Words = "Guard: Well, we had to make room for all the stuff you are hoarding.", OwningTextBox = CinematicSet.Speaker.Guard},
                            new CinematicSet.Sentence() { Words = "King: What do you suggest we do then?", OwningTextBox = CinematicSet.Speaker.King},
                            new CinematicSet.Sentence() { Words = "Guard: There's really only one option...", OwningTextBox = CinematicSet.Speaker.Guard},
                        },
                        LetterDelay = 0.02f,
                        PauseGame = true
                    }
                }
            },
            // RANDOM EXCLAMATIONS
            {
                CinematicSet.Type.RandomExclamation, new List<CinematicSet.Conversation>() {
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "king: I MIGHT NEED THAT LATER!  STOP!", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "guard: How about this, vile scum!", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "king: Not the gumdrop buttons!", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "Enemy: Where are they getting all this junk?!", OwningTextBox = CinematicSet.Speaker.Enemy}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "King: But that's a collector's item!", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "King: *quiet sobbing*", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "Enemy: Have at you!", OwningTextBox = CinematicSet.Speaker.Enemy}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "Enemy: I will strike you down!", OwningTextBox = CinematicSet.Speaker.Enemy}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "guard: I'm getting too old for this...", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "King: Don't touch my stuff!", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "King: But I need my things...", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "King: Can't you see those are imporant?!", OwningTextBox = CinematicSet.Speaker.King}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "guard: Wow, some of these things are heavy!", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "guard: And I'll ha and a hi-yah and I'll throw stuff at them sir!", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "guard: Where are all these guys coming from?!", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "guard: Will this never end?!", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                    new CinematicSet.Conversation() {
                        Sentences = new List<CinematicSet.Sentence>()
                        {
                            new CinematicSet.Sentence() { Words = "guard: I'm going to have to sleep at some point...", OwningTextBox = CinematicSet.Speaker.Guard}
                        },
                        LetterDelay = 0.02f,
                        PauseGame = false
                    },
                }
            }
        };

    public static List<Upgrade> upgrades = new List<Upgrade>()
    {
       new Upgrade(
           "castle",
           "repair\n10 dmg",
           cost: l => 100,
           filter: p => p.CurrentHealth < p.MaxHealth,
           apply: p => p.CurrentHealth = Mathf.Min(p.CurrentHealth + 10f, p.MaxHealth)),
       new Upgrade(
           "castle",
           "fortify\n+10vhp",
           cost: l => l * 200,
           apply: p =>
           {
               int upAmount = 10;
               p.MaxHealth += upAmount;
               p.CurrentHealth += upAmount;
           },
           maxLevel: 10),
       new Upgrade(
           "ammo",
           "damage\n+25",
           cost: lv => 250 + (lv - 1) * 1000,
           apply: p => p.DamageBonus += 25, 
           maxLevel: 20),
       new Upgrade(
           "ammo",
           "junk / min\n+5",
           cost: lv => 250 * (int)Mathf.Pow(5, lv - 1),
           apply: p => p.JunkPerMinute += 5,
           maxLevel: 20),
       new Upgrade(
           "ammo",
           "max junk\n+2",
           cost: lv => 500 * (int)Mathf.Pow(5, lv - 1),
           apply: p => p.MaxJunk += 2,
           maxLevel: 20),
       new Upgrade(
           "income",
           "+10% more cash",
           cost: lv => 500 + lv * 500,
           apply: p => p.CashMultiplier += .1f),
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
    public Action<Player> apply;

    public int maxLevel;

    public Upgrade(string category, string name, Func<int, int> cost = null, Func<Player, bool> filter = null, Action<Player> apply = null, int maxLevel = 0)
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

        return true;
    }

    public bool CanPurchase(Player player)
    {
        if (!IsAvailable(player))
            return false;

        if (filter != null && !filter(player))
            return false;

        return apply != null;
    }

    public bool Purchase(Player player)
    {
        if (!CanPurchase(player))
            return false;

        int cost = GetCost(player);
        if (player.Cash >= cost)
        {
            apply(player);

            player.Cash -= cost;
            player.SetUpgradeLevel(name, player.GetUpgradeLevel(name) + 1);
            return true;
        }
        return false;
    }
}
