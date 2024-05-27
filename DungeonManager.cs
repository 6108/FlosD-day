using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DungeonManager : MonoBehaviourPunCallbacks
{
    public bool isDungeon;
    GameObject player;

    public GameObject dungeonInventorySelect;
    public Image[] selectItemImage;

    public GameObject dungeonInventory;
    public Image[] dungeonItemImage; //선택한 아이템에 따라 바뀜
    //public GameObject[] invenSelectGuide; //선택됐는지 알 수 있게해주는 보조 스프라이트
    public Item dungeonItem;
    int dungeonItemCount = 1;
    public Gun[] dungeonGun = new Gun[2];

    int index = 0;
    int selectIndex = 0;
    public bool isOpen;

    public int dungeonLevel = 1;



    void Start()
    {
        StartCoroutine(IeFindPlayer());
    }

    IEnumerator IeFindPlayer()
    {
        yield return new WaitForSeconds(1f);
        player = PlayerManager.instance.GetPlayer();
        if (!player)
            StartCoroutine(IeFindPlayer());
    }

    void Update()
    {
        if (!isDungeon)
            return;
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0)
        {
            dungeonItemImage[selectIndex].GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            //invenSelectGuide[selectIndex].GetComponent<Image>().enabled = false;
            selectIndex++;
            selectIndex %= 3;
            if (selectIndex == 0)
            {
                player.GetComponentInChildren<InventoryManager>().SetCurItem(dungeonItem);
            }
            else
            {
                player.GetComponentInChildren<GunManager>().SetCurGun(dungeonGun[selectIndex - 1]);
                player.GetComponentInChildren<InventoryManager>().SetCurItem(dungeonGun[selectIndex - 1].gunItem);

            }
            dungeonItemImage[selectIndex].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            //p1InvenSelectImage[selectIndex].GetComponent<Image>().enabled = true;
            //player.GetComponentInChildren<InventoryManager>().SetCurItem(p1DungeonItem[selectIndex]);
            //InventoryManager.instance.SetCurItem(p1DungeonItem[selectIndex]);
        }
        else if (wheelInput < 0)
        {
            //invenSelectGuide[selectIndex].GetComponent<Image>().enabled = false;
            dungeonItemImage[selectIndex].GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            selectIndex--;
            if (selectIndex < 0)
                selectIndex += 3;
            selectIndex %= 3;
            if (selectIndex == 0)
            {
                if (dungeonItemCount <= 0)
                    player.GetComponentInChildren<InventoryManager>().SetCurItem(null);
                else
                    player.GetComponentInChildren<InventoryManager>().SetCurItem(dungeonItem);
            }
            else
            {
                player.GetComponentInChildren<GunManager>().SetCurGun(dungeonGun[selectIndex - 1]);
                player.GetComponentInChildren<InventoryManager>().SetCurItem(dungeonGun[selectIndex - 1].gunItem);


            }
            dungeonItemImage[selectIndex].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            //invenSelectGuide[selectIndex].GetComponent<Image>().enabled = true;
            //InventoryManager.instance.SetCurItem(p1DungeonItem[selectIndex]);
            //player.GetComponentInChildren<InventoryManager>().SetCurItem(p1DungeonItem[selectIndex]);
        }
    }

    

    public void DungeonInventorySelectOn()
    {
        dungeonInventorySelect.SetActive(true);
        isOpen=true;
    }

    public void DungeonInventorySelectOff()
    {
        dungeonInventorySelect.SetActive(false);
        isOpen=false;
    }

    public void AddDungeonInventoryItem(Item item, int count)
    {
        if(dungeonItem)
            player.GetComponentInChildren<InventoryManager>().AddItem(dungeonItem.name, dungeonItemCount);
        dungeonItem = item;
        if (count >= 5)
            dungeonItemCount = 5;
        else
            dungeonItemCount = count;
        player.GetComponentInChildren<InventoryManager>().RemoveItem(item.name, dungeonItemCount);
        selectItemImage[0].GetComponent<Image>().sprite = dungeonItem.itemImage;
        selectItemImage[0].transform.gameObject.SetActive(true);
        selectItemImage[0].GetComponentInChildren<Text>().text = dungeonItemCount + "";
        dungeonItemImage[0].GetComponentInChildren<Text>().text = dungeonItemCount + "";
        dungeonItemImage[0].sprite = dungeonItem.itemImage;
    }

    public void UseItem()
    {
        if(dungeonItemImage[0].GetComponent<Image>().color.a != 1 || dungeonItemCount <= 0)
            return;
        if (dungeonItem.category == ItemCategory.Food)
        {
            SoundManager.instance.PlaySound("Right");
            dungeonItem.itemObj.GetComponent<Food>().EatFood();
            dungeonItemCount--;
            dungeonItemImage[0].GetComponentInChildren<Text>().text = dungeonItemCount + "";
        }
       
    }

    public void AddDungeonGun(Gun gun)
    {
        dungeonGun[index] = gun;
        selectItemImage[index + 1].GetComponent<Image>().sprite = dungeonGun[index].gunItem.itemImage;
        selectItemImage[index + 1].transform.gameObject.SetActive(true);
        dungeonItemImage[index + 1].sprite = dungeonGun[index].gunItem.itemImage;
        index++;
        if (index >= 2)
            index = 0;
    }

    public void DungeonInventoryOn()
    {
        dungeonInventory.SetActive(true);
    }

    public void  DungeonInventoryOff()
    {
        dungeonInventory.SetActive(false);

    }
    
    public void ClickGoDungeon()
    {
        if (!BossManager.instance.boss.activeSelf)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(490, 1, 0);
            player.GetComponent<CharacterController>().enabled = true;
            StageManager.instance.EnterStage();
            SoundManager.instance.ChangeBGM("Dungeon");
        }

        isDungeon = true;
        DungeonInventorySelectOff();
        DungeonInventoryOn();
        player.GetComponentInChildren<InventoryManager>().InventoryOff();
        player.GetComponentInChildren<InventoryManager>().SetCurItem(dungeonItem);
        
    }

    [PunRPC]
    public void EnterText()
    {
        player.GetComponent<Player>().SetAlarmText(PhotonNetwork.NickName + "이 대기중입니다.");
    }

    public void GoHome()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(-10, 1, 10);
        player.GetComponent<CharacterController>().enabled = true;
        isDungeon = false;
        //player.GetComponentInChildren<InventoryManager>().AddItem(dungeonItem.name, dungeonItemCount);
        dungeonItem = null;
        dungeonItemCount = 0;
        DungeonInventoryOff();
    }
}
