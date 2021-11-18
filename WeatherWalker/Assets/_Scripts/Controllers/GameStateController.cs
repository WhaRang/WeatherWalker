using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance { get; private set; }

    [SerializeField] private bool log = false;
    [SerializeField] private AnimationControllerUI animationControllerUI;
    [SerializeField] private ControlPanelUI controlPanelUI;
    [SerializeField] private AudioSequenceController audioSequenceController;
    [SerializeField] private GetInferenceFromModel getInferenceFromModel;
    [SerializeField] private BackgroundControllersSequencer backgroundControllersSequencer;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private MusicGenre musicGenreToStart;

    private const string IDLE_ANIMATION = "Idle";
    private const string WALK_ANIMATION = "Walk";

    private bool isFirstTimeLaunch = true;

    private int characterIdleAnimationHash;
    private int characterWalkAnimationHash;

    public enum GameState
    {
        None = 0,
        Start = 1,
        Main = 2,
        Pause = 3
    }

    public GameState CurrentGameState { get; private set; } = GameState.None;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        characterIdleAnimationHash = Animator.StringToHash(IDLE_ANIMATION);
        characterWalkAnimationHash = Animator.StringToHash(WALK_ANIMATION);
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        UpdateGame();
    }

    private void UpdateGame()
    {
        switch (CurrentGameState)
        {
            case GameState.Start:
                UpdateGameStart();
                break;

            case GameState.Main:
                UpdateMainGame();
                break;

            case GameState.Pause:
                UpdatePausedGame();
                break;
        }
    }

    private void UpdateGameStart() { }

    private void UpdatePausedGame() { }

    private void UpdateMainGame()
    {
        int index = (int)musicGenreToStart;
        index--;
        backgroundControllersSequencer.UpdateControllersSequencer(index);
    }

    public void StartGame()
    {
        CurrentGameState = GameState.Start;
        audioSequenceController.SequenceGameStartSounds();

        int index = (int)musicGenreToStart;
        index--;
        backgroundControllersSequencer.ActivateControllerNeeded(index);

        if (log)
            Debug.Log("Current game state: " + CurrentGameState);
    }

    public void EndGame()
    {
        PauseGame();

        CurrentGameState = GameState.Start;
        isFirstTimeLaunch = true;

        controlPanelUI.ResetMainMenuAudioImpotUI();

        if (log)
            Debug.Log("Current game state: " + CurrentGameState);
    }

    public void ResumeGame()
    {
        if (CurrentGameState == GameState.Main)
            return;

        CurrentGameState = GameState.Main;
        characterAnimator.SetTrigger(characterWalkAnimationHash);

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
            Debug.Log("Current game state: " + CurrentGameState);
    }

    public void PauseGame()
    {
        if (CurrentGameState == GameState.Pause)
            return;

        CurrentGameState = GameState.Pause;
        characterAnimator.SetTrigger(characterIdleAnimationHash);

        audioSequenceController.SequenceGamePauseSounds();
        animationControllerUI.OpenMainMenu();

        if (log)
            Debug.Log("Current game state: " + CurrentGameState);
    }
}
