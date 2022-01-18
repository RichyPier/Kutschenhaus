using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    [Header("Wheels")]
    [SerializeField] WheelJoint2D rearWheel;
    [SerializeField] WheelJoint2D frontWheel;

    [Header("rearLight")]
    [SerializeField] SpriteRenderer rearLight;
    [SerializeField] Color rearLightOff;
    [SerializeField] Color rearLightOn;

    [Header("Exhaust")]
    [SerializeField] ParticleSystem exhaust;
    const float exhaustIdle = 0.15f;

    [Header("Parts(Color)")]
    [SerializeField] SpriteRenderer[] parts;

    [Header("Motor Values")]
    [SerializeField] float speed;
    [SerializeField] float boostSpeed;
    [SerializeField] bool allWheelDrive;
    [SerializeField] float maxMotorForce;
    

    [SerializeField] GameObject lights;
    [SerializeField] MotorSound motorSound;
    [SerializeField] AnalogDisplay rpm;
    [SerializeField] AnalogDisplay boost;

    TruckInput truckInput;
    JointMotor2D motor;
    SaveManager saveManager;

    float movement;
    bool truckIsDriving;

    Vector2 startPosition;
    Vector2 respawnPosition;

    bool controllerConnected;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        SetSavedColors();
        motor = new JointMotor2D();
        motor.maxMotorTorque = maxMotorForce;
        SetAllWheelDrive(allWheelDrive);
        lights.SetActive(false);

        truckInput = GetComponent<TruckInput>();
    //  FindObjectOfType<CameraFollow>().objectToFollow = transform;
    //  FindObjectOfType<EnvironmentController>().Truck = this;
    }

    // Update is called once per frame
    void Update()
    {
        var movementInput = truckInput.GetMovementInput();
        movement = movementInput * speed;
        RearLight();

        if (!controllerConnected)
            ConnectToOtherController();
    }

    void ConnectToOtherController()
    {
        var cameraFollow = FindObjectOfType<CameraFollow>();
        var environmentController = FindObjectOfType<EnvironmentController>();

        if (cameraFollow != null || environmentController != null)
        {
            cameraFollow.objectToFollow = transform;
            environmentController.Truck = this;
            controllerConnected = true;
        }
    }

    private void FixedUpdate()
    {
        var main = exhaust.main;
        main.startLifetime = ( Mathf.Abs(movement) / 200) + exhaustIdle;

        var motorSpeed = movement;

        if (movement >= speed)
        { 
            motorSpeed = movement + boostSpeed;
            boost.DisplayValue = boostSpeed;
        }
        else
        {
            boost.DisplayValue = 0f;
        }

        motor.motorSpeed = motorSpeed;
        float inputMotorSound = Mathf.InverseLerp(0, speed, motorSpeed);
        motorSound.UpdateMotorSound(inputMotorSound);
        rpm.DisplayValue = movement;
        rearWheel.motor = motor;
        if (allWheelDrive) frontWheel.motor = motor;
    }

    public void SetAllWheelDrive(bool allWheel)
    {
        allWheelDrive = allWheel;
        frontWheel.useMotor = allWheelDrive;
    }

    void RearLight()
    {
        if (Mathf.Abs(movement) > 0f && !truckIsDriving)
        {
            rearLight.color = rearLightOff;
            truckIsDriving = true;
        }
        else if (movement == 0 && truckIsDriving)
        {
            rearLight.color = rearLightOn;
            truckIsDriving = false;
        }
    }

    public void SwitchLights(bool on)
    {
        lights.SetActive(on);
    }

    void SetSavedColors()
    {
        var colors = saveManager.GetTruckColors();

        for (int i = 0; i < colors.Length; i++)
        {
            parts[i].color = colors[i];
        }
    }

}
