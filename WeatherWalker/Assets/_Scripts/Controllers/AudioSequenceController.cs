using UnityEngine;

public class AudioSequenceController : MonoBehaviour
{
    [SerializeField] private float audioFadeIn;
    [SerializeField] private float audioFadeOut;
    [SerializeField] private float audioPlayDelay;

    private AudioType[] defaultStopExceptions = new AudioType[] { AudioType.ST_Game, AudioType.ST_MainMenu };
    private AudioType[] defaultResumePauseAudio = new AudioType[] { };

    public void SequenceGamePauseSounds()
    {
        PauseAudioNeeded();

        AudioController.Instance.PauseAudio(AudioType.ST_Game, audioFadeOut, audioPlayDelay);
        AudioController.Instance.ResumeAudio(AudioType.ST_MainMenu, audioFadeOut, audioPlayDelay);
    }

    public void SequenceGameResumeSounds()
    {
        ResumeAudioNeeded();

        AudioController.Instance.PauseAudio(AudioType.ST_MainMenu, audioFadeOut, audioPlayDelay);
        AudioController.Instance.ResumeAudio(AudioType.ST_Game, audioFadeIn, audioPlayDelay);
    }

    public void SequenceFirstTimeLaunchSounds()
    {
        AudioController.Instance.PauseAudio(AudioType.ST_MainMenu, audioFadeOut, audioPlayDelay);
        AudioController.Instance.PlayAudio(AudioType.ST_Game, audioFadeIn, audioPlayDelay);

    }

    public void SequenceGameStartSounds()
    {
        AudioController.Instance.PlayAudio(AudioType.ST_MainMenu, audioFadeIn, audioPlayDelay);
    }

    public void PauseAudioNeeded()
    {
        foreach (AudioType audioType in defaultResumePauseAudio)
        {
            if (AudioController.Instance.IsAudioTrackBusyFor(audioType))
                AudioController.Instance.PauseAudio(audioType, audioFadeOut, audioPlayDelay);
        }
    }

    public void StopAudioNeeded()
    {
        AudioController.Instance.StopAllAudioExcept(defaultStopExceptions);
    }

    public void ResumeAudioNeeded()
    {
        foreach (AudioType audioType in defaultResumePauseAudio)
            AudioController.Instance.ResumeAudio(audioType, audioFadeIn, audioPlayDelay);
    }
}
