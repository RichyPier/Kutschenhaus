using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow { get; set; }
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectToFollow) Follow();
    }

    private void Follow()
    {
        transform.position = new Vector3(objectToFollow.position.x, transform.position.y, transform.position.z) + offset;
    }
}
