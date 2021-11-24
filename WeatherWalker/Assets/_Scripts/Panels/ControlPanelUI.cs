using UnityEngine;
using UnityEngine.UI;

public class ControlPanelUI : MonoBehaviour
{
    [SerializeField] private BrowserAudioImporter browserAudioImporter;
    [SerializeField] private Text audioImportLoadingBarText;
    [SerializeField] private Image audioImportLoadingBar;

    private void Update()
    {
        UpdateMainMenuAudioImportUI();
    }

    private void UpdateMainMenuAudioImportUI()
    {
        if (!MainMenuBusController.IsGameAudioLoadingStarted)
            audioImportLoadingBarText.text = MainMenuBusController.NO_AUDIO_IMPORTED;
        else if (!MainMenuBusController.IsGameAudioLoaded)
        {
            audioImportLoadingBarText.text = browserAudioImporter.Progress * 100 + " %";
            audioImportLoadingBar.fillAmount = browserAudioImporter.Progress;
        }
        else
        {
            audioImportLoadingBarText.text = MainMenuBusController.AUDIO_IMPORTED + browserAudioImporter.ClipName;
            audioImportLoadingBar.fillAmount = 1.0f;
        }
    }

    public void ResetMainMenuAudioImpotUI()
    {
        MainMenuBusController.IsGameAudioLoadingStarted = false;
        MainMenuBusController.IsGameAudioLoaded = false;
        audioImportLoadingBarText.text = MainMenuBusController.NO_AUDIO_IMPORTED;
        audioImportLoadingBar.fillAmount = 0.0f;
    }

    public void MainMenuPlayButtonOnClick()
    {
        if (MainMenuBusController.IsGameAudioLoaded)
            GameStateController.Instance.ResumeGame();
        else
            Debug.Log("Game audio is not loaded");
    }

    public void MainMenuExitButtonOnClick()
    {
        Application.Quit();
    }

    public void MainMenuImportAudioButtonOnClick()
    {
        ResetMainMenuAudioImpotUI();
        browserAudioImporter.OpenBrowser();
    }

    public void GamePauseButtonOnClick()
    {
        GameStateController.Instance.PauseGame();
    }
}
