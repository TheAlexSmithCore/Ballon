using System.Collections.Generic;
using UnityEngine;

public enum GameStates { Started = 0, Ended = 1, Paused = 2 }
public class GameStateManager : MonoBehaviour
{
    public static GameStates GameState { get; private set; }

    public delegate void OnGameStateChangedEvent();
    public static OnGameStateChangedEvent OnGameStateChanged;

    public static void ChangeGameState(int stateID) {
        if(OnGameStateChanged == null) { return; }
        GameState = (GameStates)stateID;
        OnGameStateChanged();
    }
}
