using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class levelUpItemCount
{
    public int bellCount;
    public int canationCount;
    public int dandelionCount;
}

[CreateAssetMenu]
public class Gun : ScriptableObject
{
    public string gunName;
    public Item gunItem;
    public int curLV = 1;
    public int maxLv = 4;
    public levelUpItemCount[] levelItemCount = new levelUpItemCount[3];
    //public int Exp = 0;
    public int[] damage;
    public float[] coolTime;
    public GameObject bulletPrefab;
}
