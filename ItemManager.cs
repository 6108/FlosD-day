using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public Item[] allItem;

    void Awake()
    {
        instance = this;
    }

    void Start() 
    {

    }

    public Item GetItemInfo(string itemName)
    {
        for (int i = 0; i < allItem.Length; i++)
        {
            if (allItem[i].name == itemName)
            {
                return allItem[i];
            }
        }
        return allItem[0] ;
    }
}
