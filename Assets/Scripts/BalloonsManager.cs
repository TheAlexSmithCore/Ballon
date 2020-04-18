using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsManager : MonoBehaviour
{

    public static float BalloonsSpeed;

    [SerializeField] private GameObject m_BalloonPrefab;
    [SerializeField] private float m_MaxSpawnTime = 2f;

    private BalloonsPooling m_BalloonsParent;

    private void Start() {
        BalloonsSpeed = 1.75f;
        StartCoroutine(BalloonsSpawner());

        m_BalloonsParent = FindObjectOfType<BalloonsPooling>();
    }

    private void SpawnBalloon() {
        Vector3 m_ConvertedPosition = Vector3.zero;
    }

    IEnumerator BalloonsSpawner() {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(.5f,m_MaxSpawnTime));
            Vector3 m_BalloonViewportPosition = Vector3.right * Random.Range(0.05f, .95f) - Vector3.up;
            Vector3 ballonPosition = Camera.main.ViewportToWorldPoint(m_BalloonViewportPosition);
            ballonPosition.z = 0;
            GameObject balloon = Instantiate(m_BalloonPrefab, ballonPosition, Quaternion.identity);
            SetupBalloonProperties(balloon);
        }
    }

    private void SetupBalloonProperties(GameObject balloon) {
        float m_BallonSize = Random.Range(0.4f, 1.25f);
        balloon.GetComponent<BalloonMotor>().SpeedModifier = BalloonsSpeed + (BalloonsSpeed * (1 - m_BallonSize));
        balloon.transform.localScale = new Vector3(m_BallonSize, m_BallonSize, m_BallonSize);
        Color m_BalloonColor = Random.ColorHSV(0.5f, 0.9f, 0.5f, 0.9f, 0.5f, 1);
        balloon.GetComponent<Renderer>().material.color = m_BalloonColor;
        ParticleSystem m_BallonParticleSystem = balloon.transform.GetChild(0).GetComponent<ParticleSystem>();
        m_BallonParticleSystem.startColor = m_BalloonColor;
        m_BallonParticleSystem.startSize = m_BallonSize;
        balloon.GetComponent<SphereCollider>().radius = 0.55f;

        balloon.transform.SetParent(m_BalloonsParent.transform);
    }
}
