using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TruckColorMenu : MonoBehaviour
{
    [SerializeField] Gradient colors;
    [SerializeField] RawImage gradientImage;
    [SerializeField] List<Image> truckParts;
    [SerializeField] Slider colorSlider;
    [SerializeField] GameObject truckPartButtons;
    int currentPartNumber;
    SaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        
        CreateGradientImage();
        for (int i = 0; i < truckPartButtons.transform.childCount; i++)
        {
            var number = i;
            truckPartButtons.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { currentPartNumber = number; });
        }
    }

    private void OnEnable()
    {
        saveManager = FindObjectOfType<SaveManager>();
        SetSavedColors();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void CreateGradientImage()
    {
        var texture = new Texture2D((int)gradientImage.texture.width, (int)gradientImage.texture.height);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                var color = colors.Evaluate(Mathf.InverseLerp(0, texture.width, x));
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        gradientImage.texture = texture;
    }

    public void SetColor()
    {
        truckParts[currentPartNumber].color = colors.Evaluate(colorSlider.value);

    }

    void SetSavedColors()
    {
        var colors = saveManager.GetTruckColors();

        for (int i = 0; i < colors.Length; i++)
        {
            truckParts[i].color = colors[i];
        }
    }

    void SaveColors()
    {
        var colors = new Color[13];
        for (int i = 0; i < colors.Length; i ++)
        {
            colors[i] = truckParts[i].color;
        }

        saveManager.SetTruckColors(colors);
    }

    private void OnDisable()
    {
        SaveColors();
    }
}
