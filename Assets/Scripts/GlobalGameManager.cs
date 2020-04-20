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

    public static event Action OnTimerValueChangedEvent;
    public void TimerValueChanged()
    {
        if (OnTimerValueChangedEvent == null) { return; }
        OnTimerValueChangedEvent();
    }

    public static event Action OnGameEndEvent;
    public void GameEnded()
    {
        if (OnGameEndEvent == null) { return; }
        OnGameEndEvent();
    }

    private void Start() {
        Timer = GameDuration;

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
        GameEnded();
    }

    public void RestartButtonPressed() {
        Paused = false;
        GameEnded();
        Timer = GameDuration;
        StartCoroutine(GameTimer());
    }

    public void ExitButtonPressed() {
        #if UNITY_EDITOR	
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
