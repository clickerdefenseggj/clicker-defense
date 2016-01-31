using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
}
