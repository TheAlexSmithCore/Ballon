using System.Collections.Generic;
using UnityEngine;

public class BalloonsPooling : Pool
{
    #region Singleton
    public static BalloonsPooling instance;
    #endregion

    private void Awake() {
        instance = this;
    }

    public static void ChangeBalloonState(GameObject balloon, bool isActive) {
        balloon.SetActive(isActive);
        balloon.transform.GetChild(0).gameObject.SetActive(isActive);
        balloon.transform.GetChild(1).gameObject.SetActive(isActive);
        balloon.transform.GetChild(2).gameObject.SetActive(!isActive);
        balloon.GetComponent<SphereCollider>().enabled = isActive;
        balloon.GetComponent<BalloonMotor>().enabled = isActive;
    }

    public static void ChangeBalloonState(GameObject balloon, bool isActive, bool isExplode) {
        balloon.transform.GetChild(0).gameObject.SetActive(isActive);
        balloon.transform.GetChild(1).gameObject.SetActive(isActive);
        balloon.GetComponent<SphereCollider>().enabled = isActive;
        balloon.GetComponent<BalloonMotor>().enabled = isActive;
        balloon.GetComponent<BalloonStateChanger>().ChangeBallonState(isExplode);
    }
}
