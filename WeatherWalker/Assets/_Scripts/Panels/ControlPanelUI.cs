using UnityEngine;

public class ControlPanelUI : MonoBehaviour
{
    public void MainMenuPlayButtonOnClick()
    {
        GameStateController.Instance.StartGame();
    }

    public void GamePauseButtonOnClick()
    {
        GameStateController.Instance.PauseGame();
    }
}
