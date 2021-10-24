using UnityEngine;

public class ControlPanelUI : MonoBehaviour
{
    public void MainMenuPlayButtonOnClick()
    {
        GameStateController.Instance.ResumeGame();
    }

    public void MainMenuExitButtonOnClick()
    {
        Application.Quit();
    }

    public void GamePauseButtonOnClick()
    {
        GameStateController.Instance.PauseGame();
    }
}
