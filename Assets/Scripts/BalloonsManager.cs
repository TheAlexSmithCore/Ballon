using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BalloonsManager : MonoBehaviour
{
    private const float BALLOON_MAX_SIZE = 1.25f;
    private const float BALLOON_NORMAL_SPEED = 2.5f;
    private const float BALLOON_SPEED_MULTIPLIER = 4f;
    private const float BALLOON_SPAWN_TIME = .65f;

    public static float BalloonsSpeed { get; private set; }

    private InputManager m_Inputs;
    private Coroutine m_BalloonSpanwer;

    private int m_SpawnerSide;

    private void Start() {
        m_Inputs = InputManager.instance;

        BalloonsSpeed = BALLOON_NORMAL_SPEED;

        GameStateManager.OnGameStateChanged += SetBalloonSpawner;
        m_Inputs.OnBalloonClicked += OnBalloonClicked;
    }

    private void SetBalloonSpawner() {
        if(GameStateManager.GameState == GameStates.Started) { 
            m_BalloonSpanwer = StartCoroutine(SpawnBalloons()); 
            return; 
        }
        StopCoroutine(m_BalloonSpanwer);
    }

    IEnumerator SpawnBalloons() {
        while (true)
        {
            yield return new WaitForSeconds(BALLOON_SPAWN_TIME);
            GameObject balloon = BalloonsPooling.instance.GetPooledObject();
            if (balloon == null) { continue; }

            Vector3 ballonPosition = Camera.main.ViewportToWorldPoint(GetBalloonSpawnPosition());
            ballonPosition.z = 0;
            balloon.transform.position = ballonPosition;

            BalloonsPooling.ChangeBalloonState(balloon, true);
            SetupBalloonProperties(balloon);
        }
    }

    private void OnBalloonClicked(GameObject balloon) {
        BalloonMotor m_BallonMotor = balloon.GetComponent<BalloonMotor>();
        int m_ScoreCost = (int)(1 + (m_BallonMotor.SpeedModifier * 2f)); 
        m_BallonMotor.ScoreCost = m_ScoreCost;
        StartCoroutine(BalloonScoreCostVizualization(m_ScoreCost, 1f, balloon));
        GlobalGameManager.instance.AddScore(m_ScoreCost);
        BalloonsPooling.ChangeBalloonState(balloon, false, true);
    }

    IEnumerator BalloonScoreCostVizualization(float cost, float delay, GameObject balloon) {
        Text m_ScoreCostText = ScoreCostPooling.instance.GetPooledObject().GetComponent<Text>();
        m_ScoreCostText.transform.localScale = new Vector3(1f, 1f, 1f);
        CanvasGroup m_ScoreCostCanvasGroup = m_ScoreCostText.GetComponent<CanvasGroup>();
        m_ScoreCostCanvasGroup.alpha = 1f;
        m_ScoreCostText.text = String.Format("+{0}", cost);

        m_ScoreCostText.transform.position = Camera.main.WorldToScreenPoint(balloon.transform.position);
        m_ScoreCostText.gameObject.SetActive(true);
        LeanTween.scale(m_ScoreCostText.gameObject, Vector3.zero, delay);
        LeanTween.alphaCanvas(m_ScoreCostCanvasGroup, 0f, delay / 2);
        yield return new WaitForSeconds(delay);
        m_ScoreCostText.gameObject.SetActive(false);
    }

    private Vector3 GetBalloonSpawnPosition() {
        m_SpawnerSide++;
        if (m_SpawnerSide >= 3) { m_SpawnerSide = 0; }
        switch (m_SpawnerSide)
        {
            case 0:
                return (Vector3.right * UnityEngine.Random.Range(.05f, .3f) - Vector3.up * 0.15f);
            case 1:
                return (Vector3.right * UnityEngine.Random.Range(.65f, .95f) - Vector3.up * 0.15f);
            case 2:
                return (Vector3.right * UnityEngine.Random.Range(.35f, .6f) - Vector3.up * 0.15f);
        }
        return Vector3.zero;
    }

    private void LateUpdate()
    {
        float convertedTimeValue = GlobalGameManager.instance.GetTimerToSpeedValue();
        float speedChangeResault = (convertedTimeValue * BALLOON_SPEED_MULTIPLIER);
        BalloonsSpeed = BALLOON_NORMAL_SPEED + speedChangeResault;
    }

    #region BALLOONS_GAMEOBJECT SETTINGS
    private void SetupBalloonProperties(GameObject balloon) {
        float m_BallonSize = UnityEngine.Random.Range(0.6f, BALLOON_MAX_SIZE);
        balloon.transform.localScale = new Vector3(m_BallonSize, m_BallonSize, m_BallonSize);

        float m_BalloonSpeedModifier = (BalloonsSpeed * (BALLOON_MAX_SIZE - m_BallonSize));
        balloon.GetComponent<BalloonMotor>().SpeedModifier = m_BalloonSpeedModifier;

        SetBalloonMaterialSettings(balloon);
    }

    private void SetBalloonMaterialSettings(GameObject balloon) {
        Color m_BalloonColor = UnityEngine.Random.ColorHSV(0.5f, 0.9f, 0.5f, 0.9f, 0.5f, 1);
        balloon.transform.GetChild(0).GetComponent<Renderer>().material.color = m_BalloonColor;
        ParticleSystem m_BallonTrailParticleSystem = balloon.transform.GetChild(1).GetComponent<ParticleSystem>();
        ParticleSystem m_BallonBoomParticleSystem = balloon.transform.GetChild(2).GetComponent<ParticleSystem>();
        m_BallonTrailParticleSystem.startColor = m_BalloonColor;
        m_BallonBoomParticleSystem.startColor = m_BalloonColor;
    }
    #endregion
}
