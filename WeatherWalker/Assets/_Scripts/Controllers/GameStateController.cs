using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public enum GameState
    {
        None = 0,
        Main = 1,
        Pause = 2
    }


    private GameState currentGameState = GameState.None;


    public void StartGame()
    {
        currentGameState = GameState.Main;
    }

    public void ResumeGame()
    {
        currentGameState = GameState.Main;
    }

    public void PauseGame()
    {
        currentGameState = GameState.Pause;
    }
}
