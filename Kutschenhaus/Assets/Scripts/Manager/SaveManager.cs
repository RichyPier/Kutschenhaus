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
    public int coins;

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

    public int GetCoins()
    {
        return saveData.coins;
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
