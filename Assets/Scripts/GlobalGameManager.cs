using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static bool Paused { get; private set; } = false;

    public static int Score;
    public static int Timer { get; private set; }

    public static int GameDuration { get; private set; } = 60;

    BalloonsPooling m_BalloonsPooling;

    public static event Action OnTimerValueChangedEvent;
    public void TimerValueChanged()
    {
        if (OnTimerValueChangedEvent == null) { return; }
        OnTimerValueChangedEvent();
    }

    public delegate void GameEnded(bool isTrue);
    public static GameEnded OnGameEndedEvent;

    private void Start() {
        Timer = GameDuration;
        m_BalloonsPooling = BalloonsPooling.instance;

        StartCoroutine(GameTimer());
    }

    IEnumerator GameTimer() {
        while (Timer > 0) {
            if(Paused) { yield return null; }
            yield return new WaitForSeconds(1f);
            Timer--;
            TimerValueChanged();
        }
        Paused = true;
        OnGameEndedEvent(true);
        m_BalloonsPooling.DisableAllBalloons();
        Score = 0;
    }

    public void RestartButtonPressed() {
        Paused = false;
        Timer = GameDuration;
        OnGameEndedEvent(false);
        StartCoroutine(GameTimer());
    }

    public void ExitButtonPressed() {
        #if UNITY_EDITOR	
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
