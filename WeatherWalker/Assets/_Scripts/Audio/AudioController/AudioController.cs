using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField] private bool log;
    [SerializeField] private AudioTrack[] tracks;

    public Hashtable AudioTable { get; private set; }

    private Hashtable jobTable;


    private void Awake()
    {
        if (!Instance)
            Configure();
    }

    private void OnDisable()
    {
        Dispose();
    }

    public void PlayAudio(AudioType type, float fade = 0.0f, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioJob.AudioAction.Start, type, fade, delay, false, Vector3.zero));
    }

    public void PlayAudio3D(AudioType type, Vector3 playPos, float fade = 0.0f, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioJob.AudioAction.Start, type, fade, delay, true, playPos));
    }

    public void StopAudio(AudioType type, float fade = 0.0f, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioJob.AudioAction.Stop, type, fade, delay, false, Vector3.zero));
    }

    public void PauseAudio(AudioType type, float fade = 0.0f, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioJob.AudioAction.Pause, type, fade, delay, false, Vector3.zero));
    }

    public void ResumeAudio(AudioType type, float fade = 0.0f, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioJob.AudioAction.Resume, type, fade, delay, false, Vector3.zero));
    }

    public void RestartAudio(AudioType type, float fade = 0.0f, float delay = 0.0f)
    {
        AddJob(new AudioJob(AudioJob.AudioAction.Restart, type, fade, delay, false, Vector3.zero));
    }

    public void StopAllAudioExcept(AudioType[] exceptions)
    {
        foreach (AudioType audioType in System.Enum.GetValues(typeof(AudioType)))
        {
            if (audioType == AudioType.None)
                continue;

            bool isException = false;
            foreach (AudioType exception in exceptions)
            {
                if (audioType == exception)
                {
                    isException = true;
                    break;
                }
            }

            if (!isException)
            {
                Log(audioType + " is not exception");
                AddJob(new AudioJob(AudioJob.AudioAction.Stop, audioType, 0.0f, 0.0f, false, Vector3.zero));
            }
        }
    }

    public bool IsAudioTrackBusyFor(AudioType type)
    {
        if (jobTable.ContainsKey(type))
            return true;

        AudioTrack track = (AudioTrack)AudioTable[type];
        if (track.Source.isPlaying)
            return true;

        foreach (DictionaryEntry entry in jobTable)
        {
            AudioType audioType = (AudioType)entry.Key;
            AudioTrack audioTrackInUse = (AudioTrack)AudioTable[audioType];
            AudioTrack audioTrackNeeded = (AudioTrack)AudioTable[type];

            if (audioTrackInUse.Source == audioTrackNeeded.Source)
                return true;
        }

        return false;
    }

    public void NotifyVolumeByTypeChanged(VolumeType type)
    {
        foreach (AudioTrack track in tracks)
        {
            if (track.VolumeType == type && track.Source.isPlaying)
            {
                foreach (AudioObject obj in track.AudioObjects)
                {
                    track.Source.volume = obj.GetVolume()
                        * VolumeByTypeLinker.GetVolumeByType(track.VolumeType);
                }
            }
        }
    }

    private void AddJob(AudioJob job)
    {
        if (!AudioTable.ContainsKey(job.Type))
            return;

        RemoveConflictingJobs(job.Type);

        IEnumerator jobRunner = RunAudioJob(job);
        jobTable.Add(job.Type, jobRunner);

        StartCoroutine(jobRunner);
        Log("Starting job on [" + job.Type + "] with operation " + job.Action);
    }

    private IEnumerator RunAudioJob(AudioJob job)
    {
        if (Time.timeScale == 0.0f)
            yield return new WaitForSecondsRealtime(job.Delay);
        else
            yield return new WaitForSeconds(job.Delay);

        AudioTrack track = (AudioTrack)AudioTable[job.Type];
        foreach (AudioObject obj in track.AudioObjects)
        {
            if (obj.Type == job.Type)
            {
                if (job.Action == AudioJob.AudioAction.Stop
                    || job.Action == AudioJob.AudioAction.Pause
                    || job.Action == AudioJob.AudioAction.Resume)
                    track.Source.clip = obj.GetPrevClip();
                else
                    track.Source.clip = obj.GetClip();

                track.Source.volume = obj.GetVolume()
                    * VolumeByTypeLinker.GetVolumeByType(track.VolumeType);
                track.Source.pitch = obj.GetPitch();
                track.Source.loop = obj.Loop;
            }
        }

        switch (job.Action)
        {
            case AudioJob.AudioAction.Start:
                if (job.Is3D)
                {
                    track.Source.transform.position = job.PlayPos;
                    track.Source.spatialBlend = 1.0f;
                }
                else
                    track.Source.spatialBlend = 0.0f;
                track.Source.Play();
                break;

            case AudioJob.AudioAction.Stop:
                if (job.Fade == 0.0f)
                    track.Source.Stop();
                break;

            case AudioJob.AudioAction.Restart:
                track.Source.Stop();
                track.Source.Play();
                break;

            case AudioJob.AudioAction.Pause:
                if (job.Fade == 0.0f)
                    track.Pause();
                break;

            case AudioJob.AudioAction.Resume:
                track.Resume();
                break;

            default:
                break;
        }

        if (job.Fade != 0.0f)
        {
            float initial =
                job.Action == AudioJob.AudioAction.Start
                || job.Action == AudioJob.AudioAction.Restart
                || job.Action == AudioJob.AudioAction.Resume
                ? 0.0f : track.Source.volume;
            float target = initial == 0.0f ? track.Source.volume : 0.0f;
            float duration = job.Fade;
            float timer = 0.0f;

            while (timer <= duration)
            {
                track.Source.volume = Mathf.Lerp(initial, target, timer / duration);

                if (Time.timeScale == 0.0f)
                    timer += Time.unscaledDeltaTime;
                else
                    timer += Time.deltaTime;

                yield return null;
            }

            if (job.Action == AudioJob.AudioAction.Stop)
                track.Source.Stop();

            if (job.Action == AudioJob.AudioAction.Pause)
                track.Pause();
        }

        jobTable.Remove(job.Type);
        Log("Job count: " + jobTable.Count);

        yield return null;
    }

    private void RemoveConflictingJobs(AudioType type)
    {
        if (jobTable.ContainsKey(type))
            RemoveJob(type);

        AudioType conflictAudio = AudioType.None;
        foreach (DictionaryEntry entry in jobTable)
        {
            AudioType audioType = (AudioType)entry.Key;
            AudioTrack audioTrackInUse = (AudioTrack)AudioTable[audioType];
            AudioTrack audioTrackNeeded = (AudioTrack)AudioTable[type];

            if (audioTrackInUse.Source == audioTrackNeeded.Source)
                conflictAudio = audioType;
        }

        if (conflictAudio != AudioType.None)
            RemoveJob(conflictAudio);
    }

    private void RemoveJob(AudioType type)
    {
        if (!jobTable.ContainsKey(type))
        {
            LogWarning("[" + type + "] job is not running");
            return;
        }

        IEnumerator runingJob = (IEnumerator)jobTable[type];
        StopCoroutine(runingJob);
        jobTable.Remove(type);
    }

    private void Configure()
    {
        Instance = this;

        ConfigureAudioTable();
        ConfigureJobTable();
    }

    private void ConfigureAudioTable()
    {
        AudioTable = new Hashtable();

        foreach (AudioTrack track in tracks)
        {
            foreach (AudioObject obj in track.AudioObjects)
            {
                if (AudioTable.ContainsKey(obj.Type))
                    LogWarning("[" + obj.Type + "]" + "has already been registered");
                else
                {
                    AudioTable.Add(obj.Type, track);
                    Log("Registered audio [" + obj.Type + "]");
                }
            }
        }
    }

    private void ConfigureJobTable()
    {
        jobTable = new Hashtable();
    }

    private void Dispose()
    {
        foreach (DictionaryEntry entry in jobTable)
        {
            IEnumerator job = (IEnumerator)entry.Value;
            StopCoroutine(job);
        }
    }

    private void Log(string msg)
    {
        if (!log)
            return;

        Debug.Log("[AudioController]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!log)
            return;

        Debug.LogWarning("[AudioController]: " + msg);
    }
}
