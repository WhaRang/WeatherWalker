using UnityEngine;

public class AudioTrack : MonoBehaviour
{
    [SerializeField] private AudioObject[] audioObjects;
    [SerializeField] private bool followMainCamera;
    [SerializeField] private VolumeType volumeType;

    public AudioObject[] AudioObjects => audioObjects;
    public VolumeType VolumeType => volumeType;

    public bool IsPaused { get; private set; }

    public AudioSource Source
    {
        get; private set;
    }

    private void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (followMainCamera)
            transform.position = Camera.main.transform.position;
    }

    public void Pause()
    {
        if (IsPaused)
            return;

        IsPaused = true;
        Source.Pause();
    }

    public void Resume()
    {
        if (!IsPaused)
            return;

        IsPaused = false;
        Source.UnPause();
    }
}
