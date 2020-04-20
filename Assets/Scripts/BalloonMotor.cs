using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMotor : MonoBehaviour
{

    private Rigidbody m_rigidbody;

    public float SpeedModifier { get; set; }

    BalloonsPooling m_BalloonsPooling;
    Vector3 m_BalloonToScreenPosition;

    private void Start() {
        m_rigidbody = GetComponent<Rigidbody>();
        m_BalloonsPooling = BalloonsPooling.instance;
    }

    private void Update() {
        m_BalloonToScreenPosition = Camera.main.WorldToViewportPoint(this.transform.position);
        if (m_BalloonToScreenPosition.y > 1.25f) {
            m_BalloonsPooling.DisableBalloon(true,this.gameObject);
        }
    }

    private void FixedUpdate() {
        m_rigidbody.velocity = (BalloonsManager.BalloonsSpeed + SpeedModifier) * Vector3.up;
    }
}
