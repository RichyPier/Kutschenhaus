using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject truckPrefab;
    [SerializeField] Vector3 truckStartPosition;
    [SerializeField] SaveManager saveManager;
    
    TruckInput truckInput;
    Winch winch;

    GameObject truck;
    Vector3 lastCheckpointPosition;

    int coins;

    private void Start()
    {
        coins = saveManager.GetCoins();
    }

    void SpawnTruck(Vector3 spawnPosition)
    {
        if (truck != null)
            Destroy(truck);

        truck = Instantiate(truckPrefab, spawnPosition, Quaternion.identity);
    }

    public void SpawnTruckCheckpoint()
    {
        SpawnTruck(lastCheckpointPosition);
    }

    public void SpawnTruckStartPosition()
    {
        Debug.Log("Spawn Truck");
        SpawnTruck(truckStartPosition);
    }

    public void RestartCurrentLevel()
    {
        LoadScenesAdditive(UnloadAllExcept("ManagementScene"));
    }

    List<string> UnloadAllExcept( string stillExistingScene)
    {
        int sceneCount = SceneManager.sceneCount;
        List<string> unloadedScenes = new List<string>();

        for (int i = 0; i < sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var scenename = scene.name;
            if(!scenename.Equals(stillExistingScene))
            {
                SceneManager.UnloadSceneAsync(scenename);
                unloadedScenes.Add( scenename);
            }
        }
        return unloadedScenes;
    }

    void LoadScenesAdditive(List<string> scenes)
    {
        foreach (var scene in scenes)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }
    }

    public void StartMainMenu()
    {
        UnloadAllExcept("ManagementScene");
        Destroy(truck);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }

    public void SetCheckpointPosition()
    {
        Debug.Log("SetCheckpoint");
        lastCheckpointPosition = truck.transform.GetChild(0).transform.position;
    }

    public int GetCoins()
    {
        return coins;
    }

    public void AddCoin()
    {
        coins++;
        saveManager.SetCoins(coins);
    }

    public void GameOver()
    {
        ShowWindow("UIResources/GameOverWindowPanel");
    }

    public void Victory()
    {
        ShowWindow("UIResources/VictoryWindowPanel");
    }

    void ShowWindow(string resourceName)
    {
        var resource = Resources.Load<GameObject>(resourceName);
        var canvas = GameObject.Find("PauseUICanvas");
        Instantiate(resource, canvas.transform);
        truckInput.brakeButton.SetActive(false);
        truckInput.gasButton.SetActive(false);
        // OK1: Slider slider = canvas.GetComponentInChildren<Slider>();
        // OK1: slider.interactable = false;
        winch.slider.interactable = false;
        // OK3: Winch.Instance.slider.interactable = false; // dazu in Winch.cs die Awake Methode und public static Winch Instance
    }

}
