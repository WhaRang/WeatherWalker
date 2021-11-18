using UnityEngine;

public class PathAudioImporter : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private AudioImporter importer;
    [SerializeField] private AudioObject audioObject;

    public string ClipName { get; private set; } = "";

    public float Progress
    {
        get
        {
            return importer.progress;
        }
    }

    private void Awake()
    {
        importer.Loaded += OnLoaded;
    }

    public void Import()
    {
        MainMenuBusController.IsGameAudioLoadingStarted = true;
        importer.Import(path);
    }

    private void OnLoaded(AudioClip clip)
    {
        audioObject.Clips[0] = clip;
        ClipName = clip.name;
        MainMenuBusController.IsGameAudioLoaded = true;
    }
}
