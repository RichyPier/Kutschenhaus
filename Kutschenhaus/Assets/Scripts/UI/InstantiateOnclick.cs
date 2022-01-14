using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstantiateOnclick : MonoBehaviour
{
    [SerializeField] string resourceName;
    [SerializeField] Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(InstantiateResource);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateResource()
    {
        var resource = Resources.Load<GameObject>(resourceName);
        Instantiate(resource, parent);
    }
}
