using UnityEngine;

public class AudioSequenceController : MonoBehaviour
{
    [SerializeField] private float audioFadeIn;
    [SerializeField] private float audioFadeOut;
    [SerializeField] private float audioPlayDelay;

    private AudioType[] defaultStopExceptions = new AudioType[] { AudioType.ST_Game, AudioType.ST_MainMenu };
    private AudioType[] defaultResumePauseAudio = new AudioType[] { };

    private AudioTrack stGameAudioTrack;

    private void Start()
    {
        stGameAudioTrack = (AudioTrack)AudioController.Instance.AudioTable[AudioType.ST_Game];
    }

    private void Update()
    {
        if (CheckGameSoundtrackEndCondition())
            GameSoundtrackEnded();
    }

    private bool CheckGameSoundtrackEndCondition()
    {
        return stGameAudioTrack.Source.clip != null
            && stGameAudioTrack.Source != null
            && Mathf.Approximately(stGameAudioTrack.Source.time, stGameAudioTrack.Source.clip.length)
            && GameStateController.Instance.CurrentGameState == GameStateController.GameState.Main;
    }

    private void GameSoundtrackEnded()
    {
        GameStateController.Instance.EndGame();
    }

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

        if (!MainMenuBusController.IsGameAudioChanged)
            AudioController.Instance.ResumeAudio(AudioType.ST_Game, audioFadeIn, audioPlayDelay);
        else
            AudioController.Instance.PlayAudio(AudioType.ST_Game, audioFadeIn, audioPlayDelay);

        MainMenuBusController.IsGameAudioChanged = false;
    }

    public void SequenceFirstTimeLaunchSounds()
    {
        AudioController.Instance.PauseAudio(AudioType.ST_MainMenu, audioFadeOut, audioPlayDelay);
        AudioController.Instance.PlayAudio(AudioType.ST_Game, audioFadeIn, audioPlayDelay);
        MainMenuBusController.IsGameAudioChanged = false;
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
