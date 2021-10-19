using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProcessor : MonoBehaviour
{
    public static AudioProcessor Instance { get; private set; } = null;

    private const float DEFAULT_DELTA = 0.01f;
    private const float DB_REF_VALUE = 0.1f;

    private bool showDebug = true;


    private void Awake()
    {
        Instance = this;
    }


    public float GetPeakAverage(AudioClip clip)
    {
        var samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        int peakNum = 0;
        float peakSum = 0.0f;

        for (int i = 2; i < samples.Length; i++)
        {
            float first = samples[i - 2];
            float second = samples[i - 1];
            float third = samples[i];

            if (first >= 0.0f && second > 0.0f && third >= 0.0f
                && second > first && second > third)
            {
                peakNum++;
                peakSum += second;

                if (showDebug)
                {
                    //if (peakNum % 10000 == 0)
                       //Debug.Log("[Audio processor] Peak number " + peakNum + ": " + first + " " + second + " " + third);
                }
            }
        }

        if (showDebug)
        {
            Debug.Log("[Audio processor] Peak number: " + peakNum);
            Debug.Log("[Audio processor] Peak sum: " + peakSum);
        }

        return peakSum / peakNum;
    }


    public float GetHeightAverage(AudioClip clip)
    {
        int sampleNum = clip.samples * clip.channels;

        var samples = new float[sampleNum];
        clip.GetData(samples, 0);

        double heighSum = 0.0d;

        foreach (float sample in samples)
        {
            heighSum += Mathf.Abs(sample);
        }

        if (showDebug)
        {
            Debug.Log("[Audio processor] Sample number: " + sampleNum);
            Debug.Log("[Audio processor] Sample sum: " + heighSum);
        }

        return (float)(heighSum / sampleNum);
    }

    public int GetHalfWaveNumber(AudioClip clip)
    {
        bool halfWaveFound = false;
        int sampleNum = clip.samples * clip.channels;

        var samples = new float[sampleNum];
        clip.GetData(samples, 0);

        int halfWaveNum = 0;

        for (int i = 1; i < sampleNum; i++)
        {
            if (!halfWaveFound)
            {
                if (samples[i - 1] <= 0.0f && samples[i] > 0.0f)
                    halfWaveFound = true;

                if (samples[i - 1] >= 0.0f && samples[i] < 0.0f)
                    halfWaveFound = true;
            }
            else
            {
                if (samples[i - 1] <= 0.0f && samples[i] > 0.0f)
                {
                    halfWaveNum++;
                    halfWaveFound = false;
                }

                if (samples[i - 1] >= 0.0f && samples[i] < 0.0f)
                {
                    halfWaveNum++;
                    halfWaveFound = false;
                }
            }
        }

        return halfWaveNum;
    }


    public float GetDBvalue(AudioClip clip)
    {
        int samplesNum = clip.samples * clip.channels;

        var samples = new float[samplesNum];
        clip.GetData(samples, 0);

        float sum = 0.0f;

        for (int i = 0; i < samplesNum; i++)
        {
            sum += samples[i] * samples[i];
        }

        float rmsValue = Mathf.Sqrt(sum / samplesNum);
        float dbValue = 20 * Mathf.Log10(rmsValue / DB_REF_VALUE);

        return dbValue;
    }

}
