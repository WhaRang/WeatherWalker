using UnityEngine;

public class AudioSpectrum : MonoBehaviour
{
    public static float spectrumValue { get; private set; }

    private const float DEFAULT_AUDIO_SPECTRUM_SCALE = 100.0f;

    private const int DEFAULT_AUDIO_SPECTRUM_SIZE = 128;

    private float[] audioSpectrum;

    private void Start()
    {
        audioSpectrum = new float[DEFAULT_AUDIO_SPECTRUM_SIZE];
    }

    private void Update()
    {
        AudioListener.GetSpectrumData(audioSpectrum, 0, FFTWindow.Hamming);

        if (audioSpectrum != null && audioSpectrum.Length > 0)
        {
            spectrumValue = audioSpectrum[0] * DEFAULT_AUDIO_SPECTRUM_SCALE;
        }
    }
}