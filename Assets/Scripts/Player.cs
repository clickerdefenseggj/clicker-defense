﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

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
    private int Cash;
    private int Score;
    
    // TODO: Player upgrades

    // TODO: Player preferences

    void Awake()
    {
        if (m_Inst == null)
            m_Inst = this;
    }

    public void Reset()
    {
        Score = 0;
    }

    public void AddScore(int score)
    {
        Score += score;
    }

    public int GetScore()
    {
        return Score;
    }

    public void SetName(Text nameText)
    {
        Name = nameText.text;
        Save();
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
