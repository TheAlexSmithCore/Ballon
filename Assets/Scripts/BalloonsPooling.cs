using System.Collections.Generic;
using UnityEngine;

public class BalloonsPooling : MonoBehaviour
{
    #region Singleton
    public static BalloonsPooling instance;
    #endregion

    private void Awake()
    {
        instance = this;
    }

    public static List<GameObject> Balloons;

    [SerializeField] private GameObject m_BalloonPrefab;
    [SerializeField] private int m_AmountToPool { get; } = 10;

    private void Start() {
        Balloons = new List<GameObject>();
        for (int i = 0; i < m_AmountToPool; i++)
        {
            GameObject balloon = (GameObject)Instantiate(m_BalloonPrefab);
            balloon.transform.SetParent(this.transform);
            balloon.SetActive(false);
            Balloons.Add(balloon);
        }
    }

    public void DisableBalloon(bool disableMainObject,GameObject balloon) {
        balloon.transform.GetChild(0).gameObject.SetActive(false);
        balloon.transform.GetChild(1).gameObject.SetActive(false);
        balloon.transform.GetChild(2).gameObject.SetActive(!disableMainObject);
        balloon.GetComponent<BalloonMotor>().enabled = false;
        if(!disableMainObject) { return; }
        balloon.gameObject.SetActive(false);
    }

    public void DisableAllBalloons() {
        for (int i = 0; i < Balloons.Count; i++)
        {
            Balloons[i].SetActive(false);
        }
    }

    public GameObject GetPooledBalloon()
    {
        for (int i = 0; i < Balloons.Count; i++)
        {
            if (!Balloons[i].activeInHierarchy)
            {
                return Balloons[i];
            }
        }
        return null;
    }
}
