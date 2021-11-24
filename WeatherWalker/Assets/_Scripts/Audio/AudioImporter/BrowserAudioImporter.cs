using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserAudioImporter : MonoBehaviour
{
    [SerializeField] private Browser browser;
    [SerializeField] private AudioImporter importer;
    [SerializeField] private AudioObject audioObject;
    [SerializeField] private CharacterUIHolder characterUIHolder;

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
        browser.FileSelected += OnFileSelected;
    }

    public void OpenBrowser()
    {
        browser.gameObject.SetActive(true);
        characterUIHolder.DeactivateCharacter();
    }

    public void CloseBrowser()
    {
        browser.gameObject.SetActive(false);
        characterUIHolder.ActivateCharacter();
    }

    private void OnFileSelected(string path)
    {
        MainMenuBusController.IsGameAudioLoadingStarted = true;
        StartCoroutine(Import(path));
    }

    IEnumerator Import(string path)
    {
        importer.Import(path);

        while (!importer.isInitialized && !importer.isError)
            yield return null;

        if (importer.isError)
            Debug.LogError(importer.error);


    }

    private void OnLoaded(AudioClip clip)
    {
        audioObject.Clips[0] = clip;
        ClipName = clip.name;
        MainMenuBusController.IsGameAudioLoaded = true;
        MainMenuBusController.IsGameAudioChanged = true;
        CloseBrowser();
    }
}
