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
        RaycastManager.OnScoreValueChangedEvent += ChangeScoreValue;
    }

    [SerializeField] private Text m_ScoreText;

    private void ChangeScoreValue() {
        m_ScoreText.text = String.Format("Score: {0}", GlobalGameManager.Score);
    }


    

}
