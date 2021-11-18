using UnityEngine;
using UnityEngine.UI;

public class ControlPanelUI : MonoBehaviour
{
    [SerializeField] private PathAudioImporter audioImporter;
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
            audioImportLoadingBarText.text = audioImporter.Progress * 100 + " %";
            audioImportLoadingBar.fillAmount = audioImporter.Progress;
        }
        else
        {
            audioImportLoadingBarText.text = MainMenuBusController.AUDIO_IMPORTED + audioImporter.ClipName;
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
        audioImporter.Import();
    }

    public void GamePauseButtonOnClick()
    {
        GameStateController.Instance.PauseGame();
    }
}
