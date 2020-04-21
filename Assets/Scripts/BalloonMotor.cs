using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMotor : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    public float SpeedModifier { get;set; }
    public int ScoreCost { get; set; }

    Vector3 m_BalloonToScreenPosition;

    private void Start() {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void LateUpdate() {
        m_BalloonToScreenPosition = Camera.main.WorldToViewportPoint(this.transform.position);
        if (m_BalloonToScreenPosition.y > 1.25f) {
            BalloonsPooling.ChangeBalloonState(this.gameObject, false, false);
        }
    }

    private void FixedUpdate() {
        m_rigidbody.velocity = (BalloonsManager.BalloonsSpeed + SpeedModifier) * Vector3.up;
    }
}
