using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsManager : MonoBehaviour
{
    private const float BALLOON_SPEED_MULTIPLIER = 1.15f;

    public static float BalloonsSpeed;
    [SerializeField] private float m_NormalBalloonSpeed;

    [SerializeField] private GameObject m_BalloonPrefab;
    [SerializeField] private float m_MaxSpawnTime = 2f;
    [SerializeField] private float m_MaxBalloonSize = 1f;

    private BalloonsPooling m_BalloonsParent;

    Coroutine m_BalloonSpanwer;

    private bool m_SpawnerSideChanged;

    private void Start() {
        m_BalloonSpanwer = StartCoroutine(BalloonsSpawner());

        m_BalloonsParent = FindObjectOfType<BalloonsPooling>();

        GlobalGameManager.OnGameEndEvent += StopBalloonSpawner;
    }

    private void LateUpdate() {
        float convertedTimeValue = (1 - ((float)GlobalGameManager.Timer / (float)GlobalGameManager.GameDuration));
        float speedChangeResault = (convertedTimeValue * BALLOON_SPEED_MULTIPLIER);
        BalloonsSpeed = m_NormalBalloonSpeed + speedChangeResault;
    }

    private void SpawnBalloon() {
        Vector3 m_ConvertedPosition = Vector3.zero;
    }

    private void StopBalloonSpawner() {
        StopCoroutine(m_BalloonSpanwer);
        m_BalloonsParent.gameObject.SetActive(false);
    }

    IEnumerator BalloonsSpawner() {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(.5f,m_MaxSpawnTime));
            Vector3 ballonPosition = Camera.main.ViewportToWorldPoint(GetBalloonSpawnPosition());
            ballonPosition.z = 0;
            GameObject balloon = Instantiate(m_BalloonPrefab, ballonPosition, Quaternion.identity);
            SetupBalloonProperties(balloon);
        }
    }

    private Vector3 GetBalloonSpawnPosition() {
        if (m_SpawnerSideChanged)
        {
            m_SpawnerSideChanged = !m_SpawnerSideChanged;
            return (Vector3.right * Random.Range(.05f, .45f) - Vector3.up * 0.25f);
        } else {
            m_SpawnerSideChanged = !m_SpawnerSideChanged;
            return (Vector3.right * Random.Range(.45f, .95f) - Vector3.up * 0.25f);
        }
    }

    private void SetupBalloonProperties(GameObject balloon) {
        float m_BallonSize = Random.Range(0.6f, m_MaxBalloonSize);
        float m_BalloonSpeedModifier = (BalloonsSpeed * (m_MaxBalloonSize - m_BallonSize)) * BALLOON_SPEED_MULTIPLIER;
        balloon.GetComponent<BalloonMotor>().SpeedModifier = m_BalloonSpeedModifier;
        balloon.transform.localScale = new Vector3(m_BallonSize, m_BallonSize, m_BallonSize);
        balloon.GetComponent<SphereCollider>().radius = 0.55f;

        Color m_BalloonColor = Random.ColorHSV(0.5f, 0.9f, 0.5f, 0.9f, 0.5f, 1);
        balloon.transform.GetChild(0).GetComponent<Renderer>().material.color = m_BalloonColor;
        ParticleSystem m_BallonTrailParticleSystem = balloon.transform.GetChild(1).GetComponent<ParticleSystem>();
        ParticleSystem m_BallonBoomParticleSystem = balloon.transform.GetChild(2).GetComponent<ParticleSystem>();
        m_BallonTrailParticleSystem.startColor = m_BalloonColor;
        m_BallonBoomParticleSystem.startColor = m_BalloonColor;
        m_BallonTrailParticleSystem.startSize = m_BallonSize;

        balloon.transform.SetParent(m_BalloonsParent.transform);
    }
}
