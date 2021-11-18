using UnityEngine;

[System.Serializable]
public class AudioObject: MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioType type;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float initialVolume;

    [Range(0.0f, 0.5f)]
    [SerializeField] private float volumeRandomization;

    [Range(0.1f, 3.0f)]
    [SerializeField] private float initialPitch;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float pitchRandomization;

    [SerializeField] private bool loop;
    [SerializeField] private bool playlist;

    public AudioClip[] Clips => clips;
    public AudioType Type => type;
    public bool Loop => loop;
    public bool Playlist => playlist;

    private AudioClip prevClip;
    private int currClipIndex = 0;

    public AudioClip GetPrevClip()
    {
        return prevClip;
    }

    public AudioClip GetClip()
    {
        if (playlist)
        {
            prevClip = clips[currClipIndex];

            currClipIndex++;
            if (currClipIndex >= clips.Length)
                currClipIndex = 0;

            return prevClip;
        }

        prevClip = clips[Random.Range(0, clips.Length)];
        return prevClip;
    }

    public float GetVolume()
    {
        return initialVolume + Random.Range(-volumeRandomization, volumeRandomization);
    }

    public float GetPitch()
    {
        return initialPitch + Random.Range(-pitchRandomization, pitchRandomization);
    }
}
