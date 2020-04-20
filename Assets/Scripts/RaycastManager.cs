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
                hit.collider.transform.GetChild(2).gameObject.SetActive(true);
                hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                hit.collider.transform.GetChild(1).gameObject.SetActive(false);
                ScoreValueChanged();
            }
        }
    }
}
