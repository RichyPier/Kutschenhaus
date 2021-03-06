using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TruckInput : MonoBehaviour
{
    enum InputType { Controller, Touch};

    [SerializeField] InputType inputType;

    [SerializeField] public GameObject brakeButton;
    [SerializeField] public GameObject gasButton;
    [SerializeField] float gasPedalDelay;
    //   [SerializeField] float brakePedalDelay;

    float movementInput;
    bool gas;
//    bool brake;

    // Start is called before the first frame update
    void Start()
    {
        if (inputType == InputType.Controller)
        {
            brakeButton.SetActive(false);
            gasButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputType == InputType.Controller)
            movementInput = Input.GetAxis("Horizontal");

        if (gas)
        {
            if (movementInput < 1f) movementInput += gasPedalDelay * Time.deltaTime;
        }
//       if (brake)
//       {
//           if (movementInput < 1f) movementInput -= brakePedalDelay * Time.deltaTime;
//       }
    }

    public float GetMovementInput()
    {
        return movementInput;
    }

    public void Brake(bool pressed)
    {
       if (pressed)
       {
           movementInput = -1f;
       }
       else
       {
           movementInput = 0f;
       }
//         if (pressed)
//       {
//           brake = true;
//       }
//         else
//       {
//           movementInput = -1f;
//           brake = false;
//       }
   }

    public void Gas(bool pressed)
    {
        if (pressed)
        {
            gas = true;
        } 
        else
        {
            movementInput = 0f;
            gas = false;
        }
    }
}
