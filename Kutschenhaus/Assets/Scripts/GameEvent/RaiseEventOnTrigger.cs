using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEventOnTrigger : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] string objectTagName;
    [SerializeField] bool destroySelf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(objectTagName))
        {
            Debug.Log("Trigger: " + other.name);
            gameEvent.Raise();
            if (destroySelf)
            {
                GetComponent<EasyTransition>().StartTransition();
            }
        }
    }
}
