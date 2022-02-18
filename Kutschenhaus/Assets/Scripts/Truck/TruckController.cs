using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WheelsUpgrades
{
    public PhysicsMaterial2D physicsMaterial;
    public float size;
    public float stability;
}

[System.Serializable]
public class MotorUpgrades
{
    public float speed;
    public float boostSpeed;
    public float maxMotorForce;
}

[System.Serializable]
public class ChassisUpgrades
{
    public float dampintRatio;
    public float frequency;
    public float wheelMassFront;
    public float wheelMassRear;
}

[System.Serializable]
public class SpecialsUpgrades
{
    public bool allWheelDrive;
    public bool winch;
    public bool snowChains;
    public bool amphibious;
}


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

    [Header("Upgrade Level - 1 Wheels, 2 Chassis, 3 Motor, 4 Specials")]
    [SerializeField] WheelsUpgrades[] wheelsUpgrade;
    [SerializeField] ChassisUpgrades[] chassisUpgrade;
    [SerializeField] MotorUpgrades[] motorUpgrade;
    [SerializeField] SpecialsUpgrades[] specialsUpgrade;
    
[Header("Lights & Sound & Display")]
    [SerializeField] GameObject lights;
    [SerializeField] MotorSound motorSound;
    [SerializeField] AnalogDisplay rpm;
    [SerializeField] AnalogDisplay boost;

    TruckInput truckInput;
    JointMotor2D motor;
    SaveManager saveManager;
    
    float speed;
    float boostSpeed;
    bool allWheelDrive;
    float maxMotorForce;
    float wheelStability;

    float movement;
    bool truckIsDriving;

    Vector2 startPosition;
    Vector2 respawnPosition;

    bool controllerConnected;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();
        SetUpgrades();
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
        main.startLifetime = (Mathf.Abs(movement) / 200) + exhaustIdle;

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

    void SetUpgrades()
    {
        var upgrades = saveManager.GetUpgradeLevel();

        // 1=Wheel Upgrades
        rearWheel.gameObject.GetComponent<CircleCollider2D>().sharedMaterial = wheelsUpgrade[upgrades[1]].physicsMaterial;
        frontWheel.gameObject.GetComponent<CircleCollider2D>().sharedMaterial = wheelsUpgrade[upgrades[1]].physicsMaterial;
        wheelStability = wheelsUpgrade[upgrades[1]].stability;

        float size = wheelsUpgrade[upgrades[1]].size;
        rearWheel.transform.localScale = new Vector2(size, size);
        frontWheel.transform.localScale = new Vector2(size, size);


        // 2=Chassis Upgrades
        var suspension = new JointSuspension2D();
        suspension.dampingRatio = chassisUpgrade[upgrades[2]].dampintRatio;
        suspension.frequency = chassisUpgrade[upgrades[2]].frequency;
        rearWheel.suspension = suspension;
        frontWheel.suspension = suspension;

        rearWheel.connectedBody.mass = chassisUpgrade[upgrades[2]].wheelMassRear;
        frontWheel.connectedBody.mass = chassisUpgrade[upgrades[2]].wheelMassFront;

        // 3=Motor Upgrades
        speed = motorUpgrade[upgrades[3]].speed;
        boostSpeed = motorUpgrade[upgrades[3]].boostSpeed;
        maxMotorForce = motorUpgrade[upgrades[3]].maxMotorForce;

        // 4=Specials Upgrades
        allWheelDrive = specialsUpgrade[upgrades[4]].allWheelDrive;

        // ToDo: snow chains and winch and amphibious
    }

}
