using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//인벤토리에는 아이템 이름만 저장
//아이템 꺼낼 일 있을 때 이름으로 프래팹 생성
//프래팹에 이미 설정 다 되어있음
public class InventoryManager : MonoBehaviourPunCallbacks
{
    //public static InventoryManager instance;

    public Item curItem;

    public List<Item> inventoryItemList = new List<Item>();
    public List<int> inventoryItemCount = new List<int>();

    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public GameObject inventorySlotPrefab;

    public Transform inventoryContent;
    public GameObject inventory;

    public bool isOpen;

    void Awake()
    {
        //instance = this;
        
    }

    void Start()
    {
        ///TEST///
        if (!photonView.IsMine)
            return;
        AddItem("Seed_BellFlower", 10);
        AddItem("Seed_Canation", 10);
        AddItem("Seed_Dandelion", 10);
        AddItem("BossObj01");
         AddItem("BossObj02");
         AddItem("BossObj03");



        
    }

    public void InventoryOn()
    {
        if (!photonView.IsMine)
            return;
        isOpen = true;
        inventory.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraRotate>().canMove = false;
    }

    public void InventoryOff()
    {
        if (!photonView.IsMine)
            return;
        isOpen = false;
        inventory.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.GetComponent<CameraRotate>().canMove = true;
    }

    public void AddItem(string itemName, int count = 1)
    {
        if (!photonView.IsMine)
            return;
        //아이템 검색해서 정보 받아옴
        Item item = ItemManager.instance.GetItemInfo(itemName);
        ShowAddItem(item);
        //인벤토리에 있는 아이템이면 숫자만 증가
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].name.Equals(itemName))
            {
                inventoryItemCount[i] += count;
                inventorySlots[i].ItemCount(inventoryItemCount[i]);
                return;
            }
        }
        
        //해당 아이템을 인벤토리 리스트에 추가
        inventoryItemList.Add(item);
        inventoryItemCount.Add(count);
        //인벤토리 슬롯 생성, 슬롯관리용 리스트에 추가
        GameObject itemSlot = Instantiate(inventorySlotPrefab, inventoryContent);
        itemSlot.GetComponent<InventorySlot>().ItemCount(count);
        InventorySlot component = itemSlot.GetComponent<InventorySlot>();
        inventorySlots.Add(component);
        
        //인벤토리 슬롯에 아이템 이미지 등 정보 추가
        component.SetSlot(item);
    }

    public Image[] showItemImage = new Image[5];
    public List<Item> showItem = new List<Item>();
    int index;

    void ShowAddItem(Item item)
    {
        if (!photonView.IsMine)
            return;
        if (showItem.Count >= 5)
        {
            showItem.RemoveAt(0);
            for (int i = 0; i < showItem.Count; i++)
            {
                showItemImage[i].transform.GetChild(0).GetComponent<Image>().sprite = 
                    showItemImage[i + 1].transform.GetChild(0).GetComponent<Image>().sprite;
            }
        }
        showItemImage[showItem.Count].enabled = true;
        showItemImage[showItem.Count].transform.GetChild(0).GetComponent<Image>().enabled = true;
        showItemImage[showItem.Count].transform.GetChild(0).GetComponent<Image>().sprite = item.itemImage;
        showItem.Add(item);
        StartCoroutine("IeShowAddItem", item);
    }

    IEnumerator IeShowAddItem(Item item)
    {
        yield return new WaitForSeconds(5f);
        if (showItem.Count > 0)
        {
            showItem.RemoveAt(0);
            for (int i = 0; i < showItem.Count; i++)
            {
                showItemImage[i].transform.GetChild(0).GetComponent<Image>().sprite = 
                    showItemImage[i + 1].transform.GetChild(0).GetComponent<Image>().sprite;
            }
            showItemImage[showItem.Count].enabled = false;
            showItemImage[showItem.Count].transform.GetChild(0).GetComponent<Image>().enabled = false;
        }

    }

    public void RemoveItem(string itemName, int count = 1)
    {
        if (!photonView.IsMine)
            return;
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].name.Equals(itemName))
            {
                inventoryItemCount[i] -= count;
                print(inventoryItemCount[i]);
                if (inventoryItemCount[i] <= 0)
                {
                    inventoryItemList.RemoveAt(i);
                    inventorySlots.RemoveAt(i);
                    inventoryItemCount.RemoveAt(i);
                    Destroy(inventoryContent.transform.GetChild(i).gameObject);
                    if (curItem && curItem.name == itemName)
                        curItem = null;
                    ChangeHandItem();
                }
                else 
                    inventorySlots[i].ItemCount(inventoryItemCount[i]);
                return;
            }
        }
    }
    
    public GameObject itemSprite;
    public GameObject handItem;
    //손에 들고있는 아이템 변경
    public void ChangeHandItem()
    {
        if (!photonView.IsMine)
            return;
        if (!curItem)
        {
            handItem.SetActive(false);
            return;
        }
        handItem.SetActive(true);
        GameObject obj = Instantiate(curItem.itemObj);
        handItem.GetComponent<MeshFilter>().mesh = obj.transform.GetComponent<MeshFilter>().mesh;
        Destroy(obj);
        handItem.transform.localPosition = new Vector3(-0.5f, 1, 0.5f);
        if (curItem.category == ItemCategory.Gun)
        {
            handItem.transform.localPosition = new Vector3(-0.5f, 0, 0.5f);
        }
    }

    public void SetCurItem(Item item)
    {
        if (!photonView.IsMine)
            return;
        // if (curItem && curItem == item)
        //     curItem = null;
        // else
            curItem = item;
        ChangeHandItem();
        //ChangeHandItem();
        //itemSprite.GetComponent<SpriteRenderer>().sprite = item.itemImage;
        // if (curItem.category == ItemCategory.Gun)
        //     GunManager.instance.ChangeGun(item);
    }
}
