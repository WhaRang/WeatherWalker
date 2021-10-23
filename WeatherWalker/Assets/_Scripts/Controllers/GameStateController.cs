using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance;

    public enum GameState
    {
        None = 0,
        Main = 1,
        Pause = 2
    }

    private GameState currentGameState = GameState.None;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        currentGameState = GameState.Pause;
        Debug.Log("Current game state: " + currentGameState);
    }

    public void ResumeGame()
    {
        if (currentGameState == GameState.Main)
            return;

        currentGameState = GameState.Main;
        AnimationControllerUI.Instance.CloseMainMenu();

        Debug.Log("Current game state: " + currentGameState);
    }

    public void PauseGame()
    {
        if (currentGameState == GameState.Pause)
            return;

        currentGameState = GameState.Pause;
        AnimationControllerUI.Instance.OpenMainMenu();

        Debug.Log("Current game state: " + currentGameState);
    }
}
