using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunManager : MonoBehaviour
{
    public Gun[] allGuns;
    public Gun curGun;
    public Image curGunImage;
    public Transform gunPos;
    public bool isOpen;

    public GameObject gunUpdatePanel;
    public Button cancelButton;
    public Button upGradeButton;

    public Item[] levelUpItem;
    public Text levelText;
    public Text attackText;
    public Text coolTimeText;

    int curBellCount = 0;
    int curCanationCount = 0;
    int curDandelionCount = 0;

    public Text bellCountText;
    public Text canationCountText;
    public Text dandelionCountText;
    public Text curBellCountText;
    public Text curCanationCountText;
    public Text curDandelionCountText;
    public GameObject bellImage;
    public GameObject canationImage;
    public GameObject dandelionImage;

    GameObject player;

    void Start()
    {
        player = PlayerManager.instance.GetPlayer();
        upGradeButton.onClick.AddListener(GunUpdate);
        cancelButton.onClick.AddListener(GunPanelOff);
        PanelReset();
    }

    void PanelReset()
    {
        curGunImage.color = new Color(1, 1, 1, 0);
        levelText.text = "Level";
        attackText.text = "공격력";
        coolTimeText.text = "쿨타임";
        bellCountText.text = "";
        canationCountText.text = "";
        dandelionCountText.text = "";
        curBellCountText.text = "";
        curCanationCountText.text = "";
        curDandelionCountText.text = "";
        bellImage.SetActive(false);
        canationImage.SetActive(false);
        dandelionImage.SetActive(false);
        upGradeButton.gameObject.SetActive(false);
    }

    public void SetCurGun(Gun gun)
    {
        curGun = gun;
    }
    
    public void ChangeGun(Gun gun)
    {
        
        curGunImage.color = new Color(1, 1, 1, 1);
        curGunImage.sprite = curGun.gunItem.itemImage;
        if (curGun.curLV >= 4)
        {
            levelText.text = "Level: MAX";
            attackText.text = "공격력: " + curGun.damage + "";
            coolTimeText.text = "쿨타임: " + curGun.coolTime + "";
        }
        else
        {
            SetLevelUpItem();
        }
        
    }

    public void AddItem(Item item)
    {
        print(item.itemName + ",");
        if(item.itemName == "방울꽃")
        {
            curBellCount++;
            bellImage.SetActive(true);
        }
        else if (item.itemName == "카네이션")
        {
            curCanationCount++;
            canationImage.SetActive(true);
        }
        else if (item.itemName == "민들레")
        {
            curDandelionCount++;
            dandelionImage.SetActive(true);
        }
        curBellCountText.text = curBellCount+"";
        curCanationCountText.text = curCanationCount + "";
        curDandelionCountText.text = curDandelionCount + "";
        if (curBellCount == curGun.levelItemCount[curGun.curLV].bellCount &&
            curCanationCount == curGun.levelItemCount[curGun.curLV].canationCount &&
            curDandelionCount == curGun.levelItemCount[curGun.curLV].dandelionCount)
        {
            upGradeButton.gameObject.SetActive(true);
        }
    }

    void GunUpdate()
    {
        curGun.curLV++;
        curBellCount = 0;
        curCanationCount = 0;
        curDandelionCount = 0;
        curBellCountText.text = "";
        curCanationCountText.text = "";
        curDandelionCountText.text = "";
        bellImage.SetActive(false);
        canationImage.SetActive(false);
        dandelionImage.SetActive(false);
        SetLevelUpItem();
    }


    void SetLevelUpItem()
    {
        levelText.text = "Level: " + curGun.curLV + " > " + (curGun.curLV + 1);
        attackText.text = "공격력: " + curGun.damage[curGun.curLV - 1] +  " > " + curGun.damage[curGun.curLV];
        coolTimeText.text = "쿨타임: " + curGun.coolTime[curGun.curLV - 1] +  " > " + curGun.coolTime[curGun.curLV];
        bellCountText.text = "" + curGun.levelItemCount[curGun.curLV - 1].bellCount;
        canationCountText.text = "" + curGun.levelItemCount[curGun.curLV].canationCount;
        dandelionCountText.text = "" + curGun.levelItemCount[curGun.curLV].dandelionCount;
        bellImage.SetActive(false);
        canationImage.SetActive(false);
        dandelionImage.SetActive(false);
        upGradeButton.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<InventoryManager>().AddItem(levelUpItem[0].itemName, curBellCount);
        player.GetComponentInChildren<InventoryManager>().AddItem(levelUpItem[1].itemName, curCanationCount);
        player.GetComponentInChildren<InventoryManager>().AddItem(levelUpItem[2].itemName, curDandelionCount);

        GunPanelOff();
    }

    public void GunPanelOn()
    {
        player.GetComponentInChildren<NPCManager>().TalkPanelOut();
        gunUpdatePanel.SetActive(true);
        isOpen = true;
    }

    public void GunPanelOff()
    {
        player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<NPCManager>().TalkPanelIn();
        player.GetComponentInChildren<InventoryManager>().InventoryOff();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraRotate>().canMove = false;
        gunUpdatePanel.SetActive(false);
        isOpen = false;
    }
    
    //총알 발사
    public void Shoot(Vector3 targetPos, GameObject target = null)
    {
        if(!canAttack)
            return;
        SoundManager.instance.PlaySound("Shoot");
        canAttack = false;
        StartCoroutine(IeDelay());
        GameObject bullet = Instantiate(curGun.bulletPrefab);
        bullet.transform.position = player.transform.position;
        //bullet.transform.position = gunPos.position;
        bullet.GetComponent<Bullet>().targetPos = targetPos;
        bullet.GetComponent<Bullet>().SetDamage(curGun.damage[curGun.curLV - 1]);
        bullet.GetComponent<Bullet>().SetOwner(player);
        Destroy(bullet, 3f);
    }

    bool canAttack = true;
    IEnumerator IeDelay()
    {
        yield return new WaitForSeconds(curGun.coolTime[curGun.curLV - 1]);
        canAttack = true;
    }
}
