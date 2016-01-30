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
}
