using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other) {
        //print("Player 발견");
        if (other.tag == "Player")
            transform.parent.GetComponent<EnemyMove>().target = other.gameObject;
    }
}
