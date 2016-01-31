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

    // TODO: Player upgrades
    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

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
    }
	
	// Update is called once per frame
	void Update ()
    {
	
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
        }
    }

    public void SetName(Text nameText)
    {
        Name = nameText.text;
        Save();
    }

    public void SetUpgradeLevel(string name, int level)
    {
        upgradeLevels[name] = level;
    }

    public int GetUpgradeLevel(string name)
    {
        int value;
        upgradeLevels.TryGetValue(name, out value);
        return value;
    }

    public void Save()
    {
        string saveLocation = Application.persistentDataPath + "/playerInfo.dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveLocation);

        PlayerData data = new PlayerData();
        data.name = Name;

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

            return true;
        }
        else
        {
            return false;
        }

    }
}
