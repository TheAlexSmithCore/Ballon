using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    RaycastHit hit;

    BalloonsPooling m_BalloonsPooling;

    private void Start()
    {
        m_BalloonsPooling = BalloonsPooling.instance;
    }

    public static event Action OnScoreValueChangedEvent;
    public void ScoreValueChanged()
    {
        if (OnScoreValueChangedEvent == null) { return; }
        OnScoreValueChangedEvent();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Balloons")))
        {
            if (Input.GetMouseButtonDown(0)) {
                GlobalGameManager.Score++;
                m_BalloonsPooling.DisableBalloon(false,hit.collider.gameObject);
                ScoreValueChanged();
                StartCoroutine(DisableBalloon(hit.collider.gameObject,1f));
            }
        }
    }

    IEnumerator DisableBalloon(GameObject ballon, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ballon.SetActive(false);
    }
}
