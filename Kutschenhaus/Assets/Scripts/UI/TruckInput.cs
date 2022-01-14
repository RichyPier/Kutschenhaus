using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TruckInput : MonoBehaviour
{
    enum InputType { Controller, Touch};

    [SerializeField] InputType inputType;

    [SerializeField] GameObject brakeButton;
    [SerializeField] GameObject gasButton;
    [SerializeField] float gasPedalDelay;

    float movementInput;
    bool gas;

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
    }

    public float GetMovementInput()
    {
        return movementInput;
    }

    public void Brake(bool pressed)
    {
        if (pressed) movementInput = -1f;
        else movementInput = 0f;
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
