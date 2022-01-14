using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainUI : MonoBehaviour
{
    [SerializeField] GameObject[] windows;
    [SerializeField] GameObject loadingBarPanel;
    [SerializeField] Image loadingBar;


    // Start is called before the first frame update
    void Start()
    {
        loadingBarPanel.SetActive(false);
        ShowWindow(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel(string levelName)
    {
        loadingBarPanel.SetActive(true);

        AsyncOperation[] asyncOperations = new AsyncOperation[2];
        asyncOperations[0] = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        asyncOperations[1] = SceneManager.UnloadSceneAsync("MainMenu");
        
        StartCoroutine(LoadScenesAsync(asyncOperations));
    }

    public void ShowWindow(int windowNumber)
    {
        for (int i = 0; i < windows.Length; i++)
        {
            if (windowNumber == i)
            {
                windows[i].SetActive(true);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
    }

    IEnumerator LoadScenesAsync(AsyncOperation[] asyncOperations)
    {
        float progress = 0f;

        for (int i = 0; i < asyncOperations.Length; i++)
        {
            while (!asyncOperations[i].isDone)
            {
            progress += asyncOperations[i].progress;
            loadingBar.fillAmount = progress / asyncOperations.Length;
            yield return null;
            }

        }
    }
}
