using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            StageManager.instance.NextStage();
            print("도착");
            other.gameObject.transform.position = new Vector3(0, 1, 0);
            other.GetComponentInChildren<DungeonManager>().GoHome();
            SoundManager.instance.ChangeBGM("Normal");
        }
    }
}
