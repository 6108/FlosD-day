using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            SoundManager.instance.PlaySound("AddItem");

            string otherName = other.name.Split('(')[0];
            print(otherName);
            GameObject player = PlayerManager.instance.GetPlayer();
            player.GetComponentInChildren<InventoryManager>().AddItem(otherName);
            Destroy(other.gameObject);
        }
    }
}
