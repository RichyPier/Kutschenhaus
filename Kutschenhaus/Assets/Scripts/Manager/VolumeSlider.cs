using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] bool isMusic;
    Slider slider;
    SaveManager saveManager;
    SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        saveManager = FindObjectOfType<SaveManager>();
        soundManager = FindObjectOfType<SoundManager>();

        slider.onValueChanged.AddListener(SetVolume);
        StartValue();
    }

    void SetVolume(float volume)
    {
        if (isMusic)
        {
            soundManager.SetMusicVolume(volume);
        }
        else
        {
            soundManager.SetSfxVolume(volume);
        }
    }

    void StartValue()
    {
        if (isMusic)
        {
            slider.value = saveManager.GetMusicVolume();
        }
        else
        {
            slider.value = saveManager.GetSfxVolume();
        }
    }

}
