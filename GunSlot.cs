using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class GunSlot : MonoBehaviour, IPointerClickHandler
{
    public Gun gun;
    GameObject player;

    void Start()
    {
        player = PlayerManager.instance.GetPlayer();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SoundManager.instance.PlaySound("ButtonClick");

            if (player.GetComponentInChildren<GunManager>().isOpen)
            {
                player.GetComponentInChildren<GunManager>().SetCurGun(gun);
                player.GetComponentInChildren<GunManager>().ChangeGun(gun);
                
            }
            else if (player.GetComponentInChildren<DungeonManager>().isOpen)
            {
                player.GetComponentInChildren<DungeonManager>().AddDungeonGun(gun);
            }
            else
            {
                player.GetComponentInChildren<GunManager>().SetCurGun(gun);
                player.GetComponentInChildren<InventoryManager>().SetCurItem(gun.gunItem);
                player.GetComponentInChildren<InventoryManager>().InventoryOff();
            }
        }
    }
}
