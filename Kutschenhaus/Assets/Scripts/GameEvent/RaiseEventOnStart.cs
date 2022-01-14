using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEventOnStart : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;

    // Start is called before the first frame update
    void Start()
    {
        gameEvent.Raise();
    }

}
