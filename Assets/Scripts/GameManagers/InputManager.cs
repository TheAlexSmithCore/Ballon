using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton
    public static InputManager instance;
    #endregion

    private void Awake() {
        instance = this;
    }

    #region MouseKeycodes
    [Header("Mouse Keycodes")]
    [SerializeField] private KeyCode m_BalloonClickMouseKey = KeyCode.Mouse0;
    #endregion

    public delegate void OnBalloonClickEvent(GameObject balloon);
    public OnBalloonClickEvent OnBalloonClicked;

    Ray m_rayToScreenPoint;
    RaycastHit m_raycastHit;

    private void Update() {
        m_rayToScreenPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(m_rayToScreenPoint, out m_raycastHit, LayerMask.GetMask("Balloons"))) {
            if (Input.GetKeyDown(m_BalloonClickMouseKey) && OnBalloonClicked != null) {
                OnBalloonClicked(m_raycastHit.collider.gameObject);
            }
        }
    }
}
