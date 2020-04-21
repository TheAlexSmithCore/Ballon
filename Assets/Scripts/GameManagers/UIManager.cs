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


    GlobalGameManager m_GlobalGameManager;

    private void Awake() {
        instance = this;

        m_GlobalGameManager = GlobalGameManager.instance;

        m_GlobalGameManager.OnScoreValueChanged += ChangeScoreValue;
        m_GlobalGameManager.OnTimerValueChanged += ChangeTimerValue;
        GameStateManager.OnGameStateChanged += EndGameInterfaceState;

        m_EndGamePanel.SetActive(false);
    }

    [Header("Score & Timer")]

    [SerializeField] private Text m_ScoreText;

    private void ChangeScoreValue() {
        m_ScoreText.text = String.Format("Score: {0}", m_GlobalGameManager.Score);
    }

    [SerializeField] private Text m_TimerText;

    private void ChangeTimerValue() {
        m_TimerText.text = String.Format("{0} S", m_GlobalGameManager.Timer);
    }

    [Header("End Game Panel")]

    [SerializeField] private GameObject m_EndGamePanel;
    [SerializeField] private Text m_EndGameScoreText;

    private void EndGameInterfaceState() {
        bool isActive = false;
        switch (GameStateManager.GameState)
        {
            case (GameStates)0:
                isActive = false;
                break;
            case (GameStates)1:
                isActive = true;
                break;
        }
        
        ChangeScoreValue();
        ChangeTimerValue();
        m_EndGamePanel.SetActive(isActive);
        SetActiveGameplayUI(!isActive);

        if(isActive) m_EndGameScoreText.text = String.Format("YOUR SCORE: {0}", m_GlobalGameManager.Score);
    }

    [SerializeField] private GameObject m_GameplayUI;

    private void SetActiveGameplayUI(bool state) {
        m_GameplayUI.SetActive(state);
    }
}
