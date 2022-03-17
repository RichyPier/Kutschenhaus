using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseEventOnCollision : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] string objectTagName;

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(objectTagName))
        {
            Debug.Log("Trigger: " + other.gameObject.name);
            gameEvent.Raise();
        }
    }
}
