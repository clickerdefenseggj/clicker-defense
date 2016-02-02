using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections.Generic;

[Serializable]
class PlayerData
{
    public string name;
    /*public int currentLevel;
    public int cash;
    public int score;
    public int numberKilled;
    public float maxHealth;
    public float currentHealth;
    public float damageBonus;
    public float junkPerMinute;
    public int maxJunk;
    public AchievementTemplate achievements;*/
}

public class Player : MonoBehaviour
{
    public static Player Inst { get { return m_Inst; } }
    static Player m_Inst;

    public string Name;
    public int Cash;
    private int Score;
    public int NumberKilled = 0;

    public float MaxHealth = 100;
    public float CurrentHealth;

    public float DamageBonus = 0f;

    public float CashMultiplier = 1f;

    public float JunkPerMinute = 5;
    public int CurrentJunk;
    public int MaxJunk = 5;

    public float JunkRegenSeconds = 0f;

    public class item
    {
        public int id;
        public string value;
    }

    public Dictionary<string, int> UpgradeLevels = new Dictionary<string, int>();

    public AchievementTemplate Achievements;

    // TODO: Player preferences

    void Awake()
    {
        if (m_Inst == null)
            m_Inst = this;
    }

    // Use this for initialization
    void Start ()
    {
        CurrentHealth = MaxHealth;
        CurrentJunk = MaxJunk;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (CurrentJunk < MaxJunk)
        {
            JunkRegenSeconds += Time.deltaTime;

            float secondsPerJunk = 60f / JunkPerMinute;
            while (CurrentJunk < MaxJunk && JunkRegenSeconds >= secondsPerJunk)
            {
                JunkRegenSeconds -= secondsPerJunk;
                CurrentJunk++;
            }
        }
        
        if (CurrentJunk >= MaxJunk)
        {
            JunkRegenSeconds = 0f;
        }
	}

    public void Reset()
    {
        Score = 0;
        CurrentHealth = MaxHealth;
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    public int GetScore()
    {
        return Score;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0 && App.inst.IsRunning)
        {
            App.inst.EndLevel();

            App.inst.IsRunning = false;
            App.inst.SpawnController.ClearEnemies();    
        }
    }

    public void CreateLocalSave(Text nameText)
    {
        Name = nameText.text;
        Achievements = new AchievementTemplate();
        Save();
    }

    public void SetUpgradeLevel(string name, int level)
    {
        UpgradeLevels[name] = level;
    }

    public int GetUpgradeLevel(string name)
    {
        int value;
        UpgradeLevels.TryGetValue(name, out value);
        return value;
    }

    public void AddCash(int cashValue)
    {
        Cash += Mathf.FloorToInt(cashValue * CashMultiplier);
    }

    public void Save()
    {
        string saveLocation = Application.persistentDataPath + "/playerInfo.dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveLocation);

        PlayerData data = new PlayerData();
        data.name = Name;
        /*data.currentLevel = App.inst.SpawnController.CurrentWave;
        data.cash = Cash;
        data.score = Score;
        data.numberKilled = NumberKilled;
        data.maxHealth = MaxHealth;
        data.currentHealth = CurrentHealth;
        data.damageBonus = DamageBonus;
        data.junkPerMinute = JunkPerMinute;
        data.maxJunk = MaxJunk;
        data.achievements = Achievements;*/

    bf.Serialize(file, data);
        file.Close();
    }

    // Returns true if file was successfully loaded
    public bool Load()
    {
        string saveLocation = Application.persistentDataPath + "/playerInfo.dat";

        if (File.Exists(saveLocation))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveLocation, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            Name = data.name;

            /*if(data.currentLevel != 0)
                App.inst.SpawnController.CurrentWave = data.currentLevel;

            Cash = data.cash;
            Score = data.score;
            NumberKilled = data.numberKilled;
            MaxHealth = data.maxHealth;
            CurrentHealth = data.currentHealth;
            DamageBonus = data.damageBonus;
            JunkPerMinute = data.junkPerMinute;
            MaxJunk = data.maxJunk;
            Achievements = data.achievements;*/


            return true;
        }
        else
        {
            return false;
        }

    }
}
