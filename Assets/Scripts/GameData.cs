using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnemyType
{
    Bandit,
    Archer,
    Giant
}

public static class GameData
{
    public static Dictionary<EnemyType, EnemyTemplate> enemyTemplates =
        new Dictionary<EnemyType, EnemyTemplate>()
        {
            { EnemyType.Bandit, new EnemyTemplate() { scoreValue = 5, goldValue = 10, maxHealth = 200  } },
            { EnemyType.Archer, new EnemyTemplate() { scoreValue = 7, goldValue = 15, maxHealth = 170  } },
            { EnemyType.Giant, new EnemyTemplate() { scoreValue = 12, goldValue = 40, maxHealth = 650  } }
        };
}
