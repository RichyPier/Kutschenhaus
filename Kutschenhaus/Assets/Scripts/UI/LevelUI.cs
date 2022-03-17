using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUI : MonoBehaviour
{
    [SerializeField] Text coinOutput;
    [SerializeField] GameObject redScreen;
    [SerializeField] Gradient damageGradient;
    [SerializeField] Image healthbar;
    GameManager gM;
    int coins;

    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();
        coins = gM.GetCoins();
        UpdateCoins();
        UpdateDamage(1);
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

    public void ShowRedScreen()
    {
        redScreen.SetActive(true);
        Debug.Log("Red Screen");
        Invoke("HideRedScreen", 0.2f);
    }

    void HideRedScreen()
    {
        redScreen.SetActive(false);
    }

    public void UpdateDamage( float damage)
    {
        healthbar.fillAmount = damage;
        healthbar.color = damageGradient.Evaluate(damage);
    }
}
