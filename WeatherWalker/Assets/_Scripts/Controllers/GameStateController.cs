using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance;

    [SerializeField] private bool log = false;
    [SerializeField] private AnimationControllerUI animationControllerUI;
    [SerializeField] private AudioSequenceController audioSequenceController;
    [SerializeField] private GetInferenceFromModel getInferenceFromModel;

    private bool isFirstTimeLaunch = true;

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
        audioSequenceController.SequenceGameStartSounds();

        if (log)
            Debug.Log("Current game state: " + currentGameState);
    }

    public void ResumeGame()
    {
        if (currentGameState == GameState.Main)
            return;

        // Remove!!
        getInferenceFromModel.MakePrediction();
        //

        currentGameState = GameState.Main;

        if (isFirstTimeLaunch)
        {
            audioSequenceController.SequenceFirstTimeLaunchSounds();
            isFirstTimeLaunch = false;
        }
        else
        {
            audioSequenceController.SequenceGameResumeSounds();
        }
        animationControllerUI.CloseMainMenu();

        if (log)
            Debug.Log("Current game state: " + currentGameState);
    }

    public void PauseGame()
    {
        if (currentGameState == GameState.Pause)
            return;

        currentGameState = GameState.Pause;

        audioSequenceController.SequenceGamePauseSounds();
        animationControllerUI.OpenMainMenu();

        if (log)
            Debug.Log("Current game state: " + currentGameState);
    }
}
