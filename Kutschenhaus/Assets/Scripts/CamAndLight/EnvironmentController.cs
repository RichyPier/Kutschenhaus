using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class EnvironmentController : MonoBehaviour
{
    public TruckController Truck { get; set; }
    [SerializeField] Light2D globalLight;

    [SerializeField] float lightValueByNight;
    [SerializeField] float lightValueByDay;
    [SerializeField] float dayDuration;
    [SerializeField] float nightDuration;

    [SerializeField] float sunsetDuration;
    [SerializeField] float sunriseDuration;

    bool isNight;
    float timer;
    float t; // Time value

    // Start is called before the first frame update
    void Start()
    {
        globalLight.intensity = lightValueByDay;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!isNight && timer >= dayDuration)
        {
            // sunset
            Sunset();
        }
        else if (isNight && timer >= nightDuration)
        {
            // sunrise
            Sunrise();
        }
    }

    void Sunset()
    {
        var lightValue = Mathf.Lerp(lightValueByDay, lightValueByNight, t);

        globalLight.intensity = lightValue;

        t += sunsetDuration * Time.deltaTime;

        if (t > 1)
        {
            isNight = true;
            if (Truck != null) Truck.SwitchLights(true);
            timer = 0;
            t = 0;
        }
    }

    void Sunrise()
    {
        var lightValue = Mathf.Lerp(lightValueByNight, lightValueByDay, t);

        globalLight.intensity = lightValue;

        t += sunriseDuration * Time.deltaTime;

        if (t > 1)
        {
            isNight = false;
            if (Truck != null) Truck.SwitchLights(false);
            timer = 0;
            t = 0;
        }
    }
}
