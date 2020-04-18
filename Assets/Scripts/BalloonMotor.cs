using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMotor : MonoBehaviour
{

    private Rigidbody m_rigidbody;

    public float SpeedModifier { get; set; }

    private void Start() {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        m_rigidbody.velocity = (BalloonsManager.BalloonsSpeed * SpeedModifier) * Vector3.up;
    }
}
