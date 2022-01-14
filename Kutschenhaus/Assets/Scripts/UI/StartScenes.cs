using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScenes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("ManagementScene", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("StartScene");
    }
}
