using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemCategory
{
    Seed,
    Crop,
    Food,
    Equipment,
    Gun,
    BossObj,
    None
}
//Food
//상태이상 면역, 자힐(힐1칸), 타힐(피2칸 채워진 상태로 부활)

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public GameObject itemObj;
    public string itemName = "";
    public Sprite itemImage;
    public ItemCategory category = ItemCategory.None;
    public int price;
    public int exp = 3;
}
