using System.Collections;
using UnityEngine;

public class BalloonStateChanger : MonoBehaviour
{
    public void ChangeBallonState(bool isExplode) {
        if (!isExplode) { return; }
            this.transform.GetChild(2).gameObject.SetActive(true);
            StartCoroutine(BalloonDisableDelay(1f));
    }

    IEnumerator BalloonDisableDelay(float delay) {
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}
