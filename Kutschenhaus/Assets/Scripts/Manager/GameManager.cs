using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject truckPrefab;
    [SerializeField] Vector3 truckStartPosition;
    GameObject truck;
    Vector3 lastCheckpointPosition;

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

    List<string> UnloadAllExcept(string stillExistingScene)
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
                unloadedScenes.Add(scenename);
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
