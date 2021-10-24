public class AudioJob
{
    public enum AudioAction
    {
        None,
        Start,
        Stop,
        Restart,
        Pause,
        Resume
    }

    public AudioAction Action { get; private set; }
    public AudioType Type { get; private set; }
    public float Fade { get; private set; }
    public float Delay { get; private set; }
    public bool Is3D { get; private set; }
    public UnityEngine.Vector3 PlayPos { get; private set; }

    public AudioJob(AudioAction _action, AudioType _type, float _fade, float _delay,
        bool _play3D, UnityEngine.Vector3 _playPos)
    {
        Action = _action;
        Type = _type;
        Fade = _fade;
        Delay = _delay;
        Is3D = _play3D;
        PlayPos = _playPos;
    }
}
