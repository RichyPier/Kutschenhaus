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
}

public class TruckUI : MonoBehaviour
{
    // Variables from Class TruckParts are in this Array
    [SerializeField] TruckParts[] parts;

    private void Start()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            int number = i;
            parts[i].button.GetComponent<Button>().onClick.AddListener(() => { SetPartActive(number); });
        }
        
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

    public void SetPartActive(int number)
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
}
