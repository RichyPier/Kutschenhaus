using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MotorSound : MonoBehaviour
{
    private enum Waveform { Sinus, Triangle, Sawtooth, SawtoothInvert, Square};

    [SerializeField] Waveform waveform;
    [SerializeField] [Range(0, 200)] float minFrequency;
    [SerializeField] [Range(0, 300)] float maxFrequency;
    [SerializeField] [Range(0, 1)] float minVolume;
    [SerializeField] [Range(0, 1)] float maxVolume;
    [SerializeField] [Range(0, 1)] float gain;
    [SerializeField] [Range(0, 1)] float volume;
    [SerializeField] [Range(0, 1)] float input;

    [SerializeField] AudioSource audiosource;
    [SerializeField] float frequency;
    int sampleRate;
    float phase;
    float increment;

    // Start is called before the first frame update
    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;
        Debug.Log(sampleRate);
    }

    // Update is called once per frame
    void Update()
    {
        frequency = Mathf.Lerp(minFrequency, maxFrequency, input);
        volume = Mathf.Lerp(minVolume, maxVolume, input);
        audiosource.volume = volume;
    }

    public void UpdateMotorSound(float input)
    {
        input = Mathf.Clamp(input, 0, 1);
        this.input = input;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0f * Mathf.PI / sampleRate;
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            switch (waveform)
            {
                case Waveform.Sinus:
                    data[i] = gain * Mathf.Sin(phase);
                    break;

                case Waveform.Triangle:
                    data[i] = gain * (Mathf.Sin(phase) + (Mathf.Sin(2 * phase) / 2) + (Mathf.Sin(3 * phase) / 3) + (Mathf.Sin(4 * phase) / 4) - (Mathf.Sin(2 * phase) / 2));
                    break;

                case Waveform.Sawtooth:
                    data[i] = gain * (Mathf.Sin(phase) + (Mathf.Sin(2 * phase) / 2) + (Mathf.Sin(3 * phase) / 3) + (Mathf.Sin(4 * phase) / 4) + (Mathf.Sin(5 * phase) / 5) + (Mathf.Sin(6 * phase) / 6)+ (Mathf.Sin(7 * phase) /7));
                    break;

                case Waveform.SawtoothInvert:
                    data[i] = gain * (Mathf.Sin(phase) - (Mathf.Sin(2 * phase) / 2) + (Mathf.Sin(3 * phase) / 3) - (Mathf.Sin(4 * phase) / 4) + (Mathf.Sin(5 * phase) / 5) - (Mathf.Sin(6 * phase) / 6) + (Mathf.Sin(7 * phase) / 7));
                    break;

                case Waveform.Square:
                    data[i] = gain * (Mathf.Sin(phase) + (Mathf.Sin(3 * phase) / 3) + (Mathf.Sin(5 * phase) / 5) + (Mathf.Sin(7 * phase) / 7) + (Mathf.Sin(9 * phase) / 9) + (Mathf.Sin(11 * phase) / 11));
                    break;
            }

            if (channels == 2)
                data[i + 1] = data[i];

            phase = Mathf.Repeat(phase,  Mathf.PI * 2);
        }
    }
}
