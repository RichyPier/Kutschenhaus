using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] float relativeMovement;
    [SerializeField] Transform cam;

    [SerializeField] bool onlyHorizontal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition = new Vector2();
        newPosition = cam.position * relativeMovement;
        if (onlyHorizontal) newPosition.y = transform.position.y;


        transform.position = newPosition;
    }
}
