using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potal : MonoBehaviour
{
    public GameObject DungeonCanvas;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<DungeonManager>().DungeonInventorySelectOn();
            other.GetComponentInChildren<InventoryManager>().InventoryOn();
            //DungeonCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<DungeonManager>().DungeonInventorySelectOff();
            other.GetComponentInChildren<InventoryManager>().InventoryOff();

        }
    }

    
}
