using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    Item item;
    public int count;
    Image itemImage;
    GameObject player;
    public GameObject countImage;
    public Text countText;

    void Start()
    {
        player = PlayerManager.instance.GetPlayer();
    }

    public void SetSlot(Item item)
    {
        this.item = item;
        itemImage = transform.GetChild(0).GetComponent<Image>();
        this.itemImage.sprite = item.itemImage;
        count = 1;
    }

    public void ItemCount(int count)
    {
        this.count = count; 
        countText.text = this.count.ToString();
        if (this.count > 1)
            countImage.SetActive(true);
        else
            countImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SoundManager.instance.PlaySound("ButtonClick");
            if (player.GetComponentInChildren<GunManager>().isOpen) //총 업데이트 창이 열려있으면
            {
                if (item && item.category == ItemCategory.Crop)
                {
                    player.GetComponentInChildren<GunManager>().AddItem(item);
                    player.GetComponentInChildren<InventoryManager>().RemoveItem(item.name);
                }
            }
            else if (player.GetComponentInChildren<DungeonManager>().isOpen)
            {
                player.GetComponentInChildren<DungeonManager>().AddDungeonInventoryItem(item, count);
            }
            else
            {
                player.GetComponentInChildren<InventoryManager>().SetCurItem(item);
                print("방금 클릭한 슬롯에는" + item.name);
                //player.GetComponent<PlayerMove>().InventoryClose();
                player.GetComponentInChildren<InventoryManager>().InventoryOff();
            }
            
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item.category == ItemCategory.Seed)
                return;
            print(item.name + "버림");
            GameObject player = PlayerManager.instance.GetPlayer();
                player.GetComponentInChildren<InventoryManager>().RemoveItem(item.name);
            GameObject itemObj = Instantiate(item.itemObj);
            itemObj.name = item.name;
            itemObj.transform.position = new Vector3(player.transform.position.x, 
                player.transform.position.y + 1,
                player.transform.position.z + 1);
            //Transform t = PlayerManager.instance.throwPos;
            //Vector3 dir = t.forward;
            //dir = Camera.main.transform.forward + Camera.main.transform.up;
            //dir.y -=1;
            //itemObj.GetComponent<Rigidbody>().AddForce(dir * 200);
        }

    }
}
