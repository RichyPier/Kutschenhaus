using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField] TruckController truck;
    [SerializeField] float speed;
    [SerializeField] TruckInput truckInput;

    BuoyancyEffector2D water;
    bool isSwimming;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (water != null)
        {
            var movementInput = truckInput.GetMovementInput();
            transform.Rotate(Vector3.left, movementInput * speed * Time.timeScale);

            water.flowAngle = 90 - movementInput * speed * Time.timeScale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterBuoyancy"))
        {
            truck.IsSwimming = true;
            water = collision.gameObject.GetComponent<BuoyancyEffector2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterBuoyancy"))
        {
            truck.IsSwimming = false;
            water = null;
        }
    }
}
