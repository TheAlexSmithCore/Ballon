using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    #region Singleton
    public static GlobalGameManager instance;
    #endregion

    private void Awake() {
        instance = this;
    }

    public int Score { get; private set; }
    public int Timer { get; private set; }

    public int GameDuration { get; private set; } = 60;

    #region UIEvents
    public delegate void OnTimerValueChangedEvent();
    public OnTimerValueChangedEvent OnTimerValueChanged;

    public delegate void OnScoreValueChangedEvent();
    public OnScoreValueChangedEvent OnScoreValueChanged;
    #endregion

    private void Start() {
        StartRound();
    }

    private void StartRound() {
        GameStateManager.ChangeGameState(0);
        Timer = GameDuration;
        StartCoroutine(GameTimer());
        OnTimerValueChanged();
        OnScoreValueChanged();
    }

    IEnumerator GameTimer() {
        while (Timer > 0) {
            yield return new WaitForSeconds(1f);
            Timer--;
            OnTimerValueChanged();
        }
        EndRound();
    }

    private void EndRound() {
        GameStateManager.ChangeGameState(1);
        BalloonsPooling.instance.DisableAllPoolingObjects();
        Score = 0;
    }

    public void AddScore(int value) {
        Score += value;
        OnScoreValueChanged();
    }

    public float GetTimerToSpeedValue() => (1 - ((float)Timer / (float)GameDuration));

    #region Endgame Buttons Events
    public void RestartButtonPressed() {
        Timer = GameDuration;
        GameStateManager.ChangeGameState(0);
        StartCoroutine(GameTimer());
    }

    public void ExitButtonPressed() {
        #if UNITY_EDITOR	
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    #endregion
}
