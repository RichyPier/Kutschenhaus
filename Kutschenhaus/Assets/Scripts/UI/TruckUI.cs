using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class TruckParts
{
    public string name;
    public GameObject[] singlePart;
    public GameObject panel;
    public GameObject button;
    public GameObject upgradeButton;
    public Image upgradeBar;
    public int basePrice;
    [HideInInspector] public int upgradeLevel;
    [HideInInspector] public int upgradePrice;
}

public class TruckUI : MonoBehaviour
{
    // Variables from Class TruckParts are in this Array
    [SerializeField] TruckParts[] parts;
    [SerializeField] Text coinText;
    [SerializeField] int maxUpgrade;
    SaveManager saveManager;
    int coins;

    private void Start()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            int number = i;
            parts[i].button.GetComponent<Button>().onClick.AddListener(() => { SetPartActive(number); });

            if (parts[i].upgradeButton != null)
            parts[i].upgradeButton.GetComponent<Button>().onClick.AddListener(() => { Upgrade(number); });
        }

        saveManager = FindObjectOfType<SaveManager>();
        UpdateUpgradeLevel();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        SetPartActive(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPartActive(int number)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            foreach (var part in parts[i].singlePart)
            {
                var outLine = part.GetComponent<Outline>();
                outLine.enabled = i == number;
            }
            parts[i].panel.SetActive(i == number);
            parts[i].button.GetComponent<Outline>().enabled = i == number;
        }
    }

    void UpdateUpgradeLevel()
    {
        var upgradeLevel = saveManager.GetUpgradeLevel();

        coins = saveManager.GetCoins();
        coinText.text = coins + "$";

        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].upgradeButton != null)
            {
                parts[i].upgradeLevel = upgradeLevel[i];
                parts[i].upgradePrice = (parts[i].upgradeLevel + 1) * parts[i].basePrice;
                parts[i].upgradeButton.GetComponentInChildren<Text>().text = string.Format("Upgrade\n{0}$", parts[i].upgradePrice);

                float step = 1 / ((float)maxUpgrade + 1);
                Debug.Log(step);
                parts[i].upgradeBar.fillAmount = step + (step * parts[i].upgradeLevel);

                if (parts[i].upgradeLevel == maxUpgrade)
                {
                    var canvasGroup = parts[i].upgradeButton.GetComponent<CanvasGroup>();
                    canvasGroup.interactable = false;
                    canvasGroup.alpha = 0.5f;
                }
            }
        }
    }

    void Upgrade(int number)
    {
        if (parts[number].upgradePrice <= coins)
        {
            parts[number].upgradeLevel++;
            saveManager.SetCoins(coins - parts[number].upgradePrice);
            saveManager.SetUpgradeLevel(number, parts[number].upgradeLevel);
            UpdateUpgradeLevel();
        }
    }
}
