using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsManager : MonoBehaviour
{

    private const float BALLOON_MAX_SIZE = 1.25f;
    private const float BALLOON_NORMAL_SPEED = 3f;
    private const float BALLOON_SPEED_MULTIPLIER = 6f;
    private const float BALLOON_SPAWN_TIME = .65f;

    public static float BalloonsSpeed { get; private set; }

    private Coroutine m_BalloonSpanwer;

    private int m_SpawnerSide;

    private void Start() {
        SetBalloonSpawner(false);
        BalloonsSpeed = BALLOON_NORMAL_SPEED;

        GlobalGameManager.OnGameEndedEvent += SetBalloonSpawner;
    }

    // Запуск корутины для спавна шариков в зависимости от перменной isEnabled.
    private void SetBalloonSpawner(bool isEnded) {
        if(!isEnded) { m_BalloonSpanwer = StartCoroutine(SpawnBalloons()); return; }
        StopCoroutine(m_BalloonSpanwer);
    }

    // Спавнер шариков с дальнейшей их настройкой.
    IEnumerator SpawnBalloons() {
        while (true)
        {
            yield return new WaitForSeconds(BALLOON_SPAWN_TIME);
            Vector3 ballonPosition = Camera.main.ViewportToWorldPoint(GetBalloonSpawnPosition());
            ballonPosition.z = 0;
            GameObject balloon = BalloonsPooling.instance.GetPooledBalloon();
            if (balloon != null)
            {
                balloon.SetActive(true);
                balloon.transform.position = ballonPosition;
                balloon.transform.GetChild(0).gameObject.SetActive(true);
                balloon.transform.GetChild(1).gameObject.SetActive(true);
                balloon.transform.GetChild(2).gameObject.SetActive(false);
                balloon.GetComponent<BalloonMotor>().enabled = true;
                SetupBalloonProperties(balloon);
            }
        }
    }

    // Получаем случайную позицию из чередующихся сторон.
    private Vector3 GetBalloonSpawnPosition() {
        m_SpawnerSide++;
        if (m_SpawnerSide >= 3) { m_SpawnerSide = 0; }
        switch (m_SpawnerSide)
        {
            case 0:
                return (Vector3.right * Random.Range(.05f, .3f) - Vector3.up * 0.15f);
            case 1:
                return (Vector3.right * Random.Range(.65f, .95f) - Vector3.up * 0.15f);
            case 2:
                return (Vector3.right * Random.Range(.35f, .6f) - Vector3.up * 0.15f);
        }
        return Vector3.zero;
    }

    // Увеличиваем скорость шариков в зависимости от игрового времени.
    private void LateUpdate()
    {
        float convertedTimeValue = (1 - ((float)GlobalGameManager.Timer / (float)GlobalGameManager.GameDuration));
        float speedChangeResault = (convertedTimeValue * BALLOON_SPEED_MULTIPLIER);
        BalloonsSpeed = BALLOON_NORMAL_SPEED + speedChangeResault;
    }

    // Настраиваем параметры нашаего шарика.
    #region BALLOONS_GAMEOBJECT SETTINGS
    private void SetupBalloonProperties(GameObject balloon) {
        float m_BallonSize = Random.Range(0.6f, BALLOON_MAX_SIZE);
        balloon.transform.localScale = new Vector3(m_BallonSize, m_BallonSize, m_BallonSize);

        float m_BalloonSpeedModifier = (BalloonsSpeed * (BALLOON_MAX_SIZE - m_BallonSize));
        balloon.GetComponent<BalloonMotor>().SpeedModifier = m_BalloonSpeedModifier;

        SetBalloonMaterialSettings(balloon);
    }

    private void SetBalloonMaterialSettings(GameObject balloon) {
        Color m_BalloonColor = Random.ColorHSV(0.5f, 0.9f, 0.5f, 0.9f, 0.5f, 1);
        balloon.transform.GetChild(0).GetComponent<Renderer>().material.color = m_BalloonColor;
        ParticleSystem m_BallonTrailParticleSystem = balloon.transform.GetChild(1).GetComponent<ParticleSystem>();
        ParticleSystem m_BallonBoomParticleSystem = balloon.transform.GetChild(2).GetComponent<ParticleSystem>();
        m_BallonTrailParticleSystem.startColor = m_BalloonColor;
        m_BallonBoomParticleSystem.startColor = m_BalloonColor;
    }
    #endregion
}
