using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonsPooling : MonoBehaviour
{
    public static List<GameObject> Balloons = new List<GameObject>();

    [SerializeField] private Transform m_BalloonsParent;

    public static void SetupBalloonsList(Transform balloonsParent) {
        if(Balloons.Count > 0) { Debug.LogError("Ballons List Exists"); return; }
        Balloons.Clear();
        for (int i = 0; i < balloonsParent.childCount; i++)
        {
            Balloons.Add(balloonsParent.GetChild(i).gameObject);
        }
    }

    public static GameObject GetDisabledObject(Transform balloonsParent) {
        if (Balloons.Count > 0) { Debug.LogError("Ballons List Empty"); return null; }
        for (int i = 0; i < balloonsParent.childCount; i++) {
            GameObject balloon = balloonsParent.GetChild(i).gameObject;
            if (balloon.activeSelf) {
                return balloon;
            }
        }
        return null;
    }
}
