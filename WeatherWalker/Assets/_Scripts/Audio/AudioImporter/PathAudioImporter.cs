using UnityEngine;

public class PathAudioImporter : MonoBehaviour
{
    [SerializeField] private string path;
    [SerializeField] private AudioImporter importer;
    [SerializeField] private AudioObject audioObject;

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
        importer.Import(path);
    }

    private void OnLoaded(AudioClip clip)
    {
        audioObject.Clips[0] = clip;
        MainMenuBusController.IsGameAudioLoaded = true;
    }
}
