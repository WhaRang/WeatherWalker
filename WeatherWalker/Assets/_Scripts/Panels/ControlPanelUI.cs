using UnityEngine;

public class ControlPanelUI : MonoBehaviour
{
    [SerializeField] private PathAudioImporter audioImporter;

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

    public void GamePauseButtonOnClick()
    {
        GameStateController.Instance.PauseGame();
    }

    public void MainMenuImportAudioButtonOnClick()
    {
        audioImporter.Import();
    }
}
