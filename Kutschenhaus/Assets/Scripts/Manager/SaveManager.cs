using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;

[System.Serializable]

public class SaveData
{
    public Color[] truckColors;

    [Space(10)]
    [Header("0=Color, 1=Wheels, 2=Chassis, 3=Motor, 4=Special")]
    public int[] upgradeLevel;
    
    [Space(10)]
    public int coins;

    [Space(10)]
    public int adPrivacy = -1;

    [Range(0, 1)]
    public float musicVolume;
    [Range(0, 1)]
    public float sfxVolume;

    // ToDo: other saved data
}

public class SaveManager : MonoBehaviour
{
    [SerializeField] SaveData saveData;
    string path;

    // Start is called before the first frame update
    void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "savegame.json");
        Load();
    }

    public Color[] GetTruckColors()
    {
        return saveData.truckColors;
    }

    public void SetTruckColors(Color[] colors)
    {
        saveData.truckColors = colors;
    }
    public void SetCoins(int coins)
    {
        saveData.coins = coins;
    }

    public void SetAdPrivacy(int adPrivacy)
    {
        saveData.adPrivacy = adPrivacy;
    }

    public int GetAdPrivacy()
    {
        return saveData.adPrivacy;
    }

    public int GetCoins()
    {
        return saveData.coins;
    }

    public void SetUpgradeLevel(int partNumber, int level)
    {
        saveData.upgradeLevel[partNumber] = level;
    }

    public int[] GetUpgradeLevel()
    {
        return saveData.upgradeLevel;
    }

    public void SetSfxVolume(float volume)
    {
        saveData.sfxVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        saveData.musicVolume = volume;
    }

    public float GetSfxVolume()
    {
        return saveData.sfxVolume;
    }

    public float GetMusicVolume()
    {
        return saveData.musicVolume;
    }

    void Load()
    {
        if (File.Exists(path))
        {
            Debug.Log(path);
            string jsonStr = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(jsonStr, saveData);
        }
    }

    void Save()
    {
        string jsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, jsonString);
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
