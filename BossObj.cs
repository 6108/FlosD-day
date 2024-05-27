using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObj : MonoBehaviour
{
    public Item item;

    void Update()
    {
        if (BossManager.instance.boss.GetComponent<Boss>().bossStage)
            return;
        transform.Rotate(Vector3.up * Time.deltaTime * 30);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            other.GetComponentInChildren<InventoryManager>().AddItem(item.name);
            StageManager.instance.GetObj();
            Destroy(this.gameObject);
        }
    }
}
