using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
     public  AudioSource _src;
    public static float[] _samples = new float[512];
    public static int Samples = 512;

    public static float[] _frequencyBand = new float[8];
    public static float[] _bandBuffer = new float[8];

    float[] _bufferDecrease = new float[8];

    float[] _frequencyBandHighest = new float[8];
    public static float[] _audioBand = new float[8];
    public static float[] _audioBandBuffer = new float[8];

    public static float Amplitude, AmplitudeBuffer;
    private float _AmplitudeHighest;
    // Start is called before the first frame update
    void Awake()
    {
        _src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    private void CreateAudioBands() {
        for (int i = 0; i < 8; i++)
        {
            if (_frequencyBand[i] > _frequencyBandHighest[i]) {
                _frequencyBandHighest[i] = _frequencyBand[i];
            }
            _audioBand[i] = (_frequencyBand[i] / _frequencyBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _frequencyBandHighest[i]);
        }
    }

    private void GetAmplitude() {

        float curAmp = 0;
        float curAmpBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            curAmp += _audioBand[i];
            curAmpBuffer += _audioBandBuffer[i];
        }
        if (curAmp > _AmplitudeHighest) {
            _AmplitudeHighest = curAmp;
        }
        Amplitude = curAmp / _AmplitudeHighest;
        AmplitudeBuffer = curAmpBuffer / _AmplitudeHighest;
    }

    private void BandBuffer() {
        for (int i = 0; i < 8; i++)
        {
            if (_frequencyBand[i] > _bandBuffer[i]) {
                _bandBuffer[i] = _frequencyBand[i];
                _bufferDecrease[i] = 0.005f;
            }

            if (_frequencyBand[i] < _bandBuffer[i]) {
                _bandBuffer[i]-=_bufferDecrease[i];
                _bufferDecrease[i] *= 1.2f;
            }

        }
    }

    private void GetSpectrumAudioSource() {
        _src.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }

    private void MakeFrequencyBands() {
        /* 
         * [0]  -20 - 60 hertz
         * [1] - 60 - 250 hertz
         * [2] - 250 - 500 hertz
         * [3] - 500 - 2000 hertz
         * [4] - 2000 - 4000 hertz
         * [5] - 4000 - 6000 hertz
         * [6] - 6000 - 20000 hertz
        */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float avarege = 0;
            int sampleCount = (int)Mathf.Pow(2, i)*2;
            if (i == 7) sampleCount += 2;

            for (int j = 0; j < sampleCount; j++)
            {
                avarege += _samples[count] * (count + 1);
                count++;
            }

            avarege /= count;
            _frequencyBand[i] = avarege * 10;
        }


    }
}
