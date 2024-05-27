using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class GunExpSlot : MonoBehaviour, IPointerClickHandler
{
    Item item;
    Image itemImage;

    public void SetSlot(Item item)
    {
        this.item = item;
        itemImage = transform.GetChild(0).GetComponent<Image>();
        this.itemImage.sprite = item.itemImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
        }
    }
}
