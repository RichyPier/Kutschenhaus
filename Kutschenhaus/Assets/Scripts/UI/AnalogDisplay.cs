using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AnalogDisplay : MonoBehaviour
{
    [SerializeField] float zeroAngle;
    [SerializeField] float maxAngle;
    [SerializeField] float maxValue;
    [SerializeField] float animationSpeed;

    float displayValue;

    public float DisplayValue
    {
        get { return displayValue; }

        set
        {
            if (value >= 0 && value <= maxValue)
            {
                displayValue = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(value)} must be between 0 and {maxValue}"
                    );
            }
        }
    }

    public float test;
    public float Test { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetRotation = Quaternion.Euler(0, 0, -Mathf.Abs(GetRotation()));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, animationSpeed * Time.deltaTime);
    }

    public void SetValue(float val)
    {
        if (val >= 0 && val <= maxValue)
        {
            displayValue = val;
        }
    }

    float GetRotation()
    {
        float totalAngle = maxAngle - zeroAngle;

        float value = displayValue / maxValue;

        return zeroAngle - value * totalAngle;
    }
}
