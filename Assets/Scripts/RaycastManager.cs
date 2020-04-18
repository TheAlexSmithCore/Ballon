using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    RaycastHit hit;

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
                Debug.Log(hit.collider.name);
                Destroy(hit.collider.gameObject);
                ScoreValueChanged();
            }
        }
    }
}
