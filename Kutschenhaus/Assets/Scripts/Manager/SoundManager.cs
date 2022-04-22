using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]

public class Sound
{
    public string name;
    public AudioClip clip;

    [HideInInspector]
    public AudioSource source;

    [Range(0f, 1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float startPitch;
    [Range(0f, 0.5f)]
    public float randomPitch;
    public bool loop;
    public bool isMusic;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;

    [Header("Music Volume")]
    [SerializeField] AudioMixerGroup musicMixerGroup;

    [Header("Sfx Volume")]
    [SerializeField] AudioMixerGroup fxMixerGroup;

    [Header("Sounds & Music")]
    [SerializeField] Sound[] sounds;

    SaveManager saveManager;

    // Start is called before the first frame update
    void Start()
    {
        saveManager = FindObjectOfType<SaveManager>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.pitch = s.startPitch;

            if (s.isMusic)
                s.source.outputAudioMixerGroup = musicMixerGroup;
            else
                s.source.outputAudioMixerGroup = fxMixerGroup;

        }
    }

    public void PlaySound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.name.Equals(soundName))
            {
                if (s.randomPitch != 0f)
                {
                    s.source.pitch = s.startPitch + UnityEngine.Random.Range(-s.randomPitch, s.randomPitch);
                }
                s.source.Play();
                break;
            }
        }
    }

    public void StopSound(string soundName)
    {
        foreach (Sound s in sounds)
        {
            if (s.name.Equals(soundName))
            {
                s.source.Stop();
                break;
            }
        }
    }

    public void StopAllSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public void SetSfxVolume(float value)
    {
        saveManager.SetSfxVolume(value);
        SetVolume(fxMixerGroup.name, value);
    }

    public void SetMusicVolume(float value)
    {
        saveManager.SetMusicVolume(value);
        SetVolume(musicMixerGroup.name, value);
    }

    void SetVolume(string mixerGroupName, float value)
    {
        float dB = Mathf.Log10(value) * 20;
        mixer.SetFloat(mixerGroupName, dB);
    }

    void InitVolume()
    {
        SetVolume(musicMixerGroup.name, saveManager.GetMusicVolume());
        SetVolume(fxMixerGroup.name, saveManager.GetSfxVolume());
    }

}
