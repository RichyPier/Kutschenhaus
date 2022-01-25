using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Text coinOutput;
    GameManager gM;
    int coins;

    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        coins = gM.GetCoins();
        UpdateCoins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins()
    {
        coins++;
        UpdateCoins();
    }

    void UpdateCoins()
    {
        coinOutput.text = coins.ToString();
    }
}
