using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{

    public static int Score;
    public static int Timer;

    [SerializeField] private int m_MaxGameDuration;

    private void Start() {
        Timer = m_MaxGameDuration;
    }
}
