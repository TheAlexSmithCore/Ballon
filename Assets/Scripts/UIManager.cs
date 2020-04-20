using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;
    #endregion

    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        ChangeScoreValue();
        ChangeTimerValue();
        RaycastManager.OnScoreValueChangedEvent += ChangeScoreValue;
        GlobalGameManager.OnTimerValueChangedEvent += ChangeTimerValue;
        GlobalGameManager.OnGameEndedEvent += EndGameActiveState;

        m_EndGamePanel.SetActive(false);
    }

    [SerializeField] private Text m_ScoreText;

    private void ChangeScoreValue() {
        m_ScoreText.text = String.Format("Score: {0}", GlobalGameManager.Score);
    }

    [SerializeField] private Text m_TimerText;

    private void ChangeTimerValue() {
        m_TimerText.text = String.Format("{0} S", GlobalGameManager.Timer);
    }

    [SerializeField] private GameObject m_EndGamePanel;
    [SerializeField] private Text m_EndGameScoreText;

    private void EndGameActiveState(bool isActive) {
        ChangeScoreValue();
        ChangeTimerValue();
        m_EndGamePanel.SetActive(isActive);
        SetActiveGameplayUI(!isActive);

        if(isActive) m_EndGameScoreText.text = String.Format("YOUR SCORE: {0}", GlobalGameManager.Score);
    }

    [SerializeField] private GameObject m_GameplayUI;

    private void SetActiveGameplayUI(bool state) {
        m_GameplayUI.SetActive(state);
    }
}
