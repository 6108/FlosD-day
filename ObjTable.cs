using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTable : MonoBehaviour
{
    public Item objItem;
    public GameObject obj;
   public bool activeTrue;

    public void SetObj(Item item)
    {
        if (activeTrue)
            return;
        BossManager.instance.AddBossObj();
        obj.SetActive(true);
        activeTrue = true;
    }
}
