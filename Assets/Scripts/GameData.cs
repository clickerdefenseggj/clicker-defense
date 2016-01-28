using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameData
{
    public static Dictionary<string, UnitTemplate> unitTemplates =
        new Dictionary<string, UnitTemplate>()
        {
            { "bandit", new UnitTemplate() { scoreValue = 5, goldValue = 10, maxHealth = 200  } },
            { "archer", new UnitTemplate() { scoreValue = 7, goldValue = 15, maxHealth = 170  } },
            { "giant", new UnitTemplate() { scoreValue = 12, goldValue = 40, maxHealth = 650  } }
        };
}
