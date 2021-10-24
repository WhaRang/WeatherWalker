using UnityEngine;

public class AudioDataTest : MonoBehaviour
{
    [SerializeField] private AudioClip testAudioClip;

    private const float VALENCE_DEFAULT_MULT = -209.7f;
    private const float VALENCE_HEIGHT_AVG_MULT = 0.004548f;
    private const float VALENCE_PEAK_AVG_MULT = 0.0005603f;
    private const float VALENCE_BPM_MULT = 2.29f;

    private float[] audioClipSamples;


    private void Start()
    {
        audioClipSamples = new float[testAudioClip.samples * testAudioClip.channels];

        int offsetSamples = 0;
        bool gotSamplesData = testAudioClip.GetData(audioClipSamples, offsetSamples);

        if (gotSamplesData)
        {
            Debug.Log("Success!");
            Debug.Log("Samples number: " + audioClipSamples.Length);

            int bpm = UniBpmAnalyzer.AnalyzeBpm(testAudioClip);
            int halfWaveNum = AudioProcessor.Instance.GetHalfWaveNumber(testAudioClip);

            float peakAvg = AudioProcessor.Instance.GetPeakAverage(testAudioClip);
            float heightAvg = AudioProcessor.Instance.GetHeightAverage(testAudioClip);
            float dbValue = AudioProcessor.Instance.GetDBvalue(testAudioClip);

            Debug.Log("BPM: " + bpm);
            Debug.Log("Half wave number: " + halfWaveNum);
            Debug.Log("Peak average: " + peakAvg);
            Debug.Log("Average height: " + heightAvg);
            Debug.Log("DB value: " + dbValue);
        }
    }


    private void Update()
    {
        
    }
}
