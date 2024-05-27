using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//클릭한 곳이 땅이면 HarvestManager한테 위치값 넘김
public class PlayerMove : MonoBehaviourPunCallbacks
{
    int id;
    public float speed = 10;

    public GameObject hand;
    public GameObject myCamera;
    bool isInventoryOpen;
    GameObject NPC;
    
    public CharacterController cc;

    Vector3 handMin= new Vector3(0.4f,-0.46f,1.364f);
    Vector3 handMax= new Vector3(0.405f,-0.514f,1.338f);

    GameObject player;
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cc = GetComponent<CharacterController>();
        NPC = GameObject.FindWithTag("NPC");
        StartCoroutine(IeHandMoving());
        player = PlayerManager.instance.GetPlayer();
        if (photonView.IsMine)
        {
            selectSoil.SetActive(true);
            selectSoil.transform.parent = null;
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = transform.right * h + transform.forward * v;
        dir.Normalize();
        cc.SimpleMove(dir * speed * Time.deltaTime);
        InventoryCheck();
        ClickCheck();
        Dash();
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(IeDash());
        }
    }
    
    IEnumerator IeDash()
    {
        float startTime = Time.time; 
        while(Time.time < startTime + 0.3f)
        {
            cc.SimpleMove(transform.forward * speed * 5 * Time.deltaTime);
            yield return null;
        }

    }
    
    public float handSpeed = 0.05f;
    IEnumerator IeHandMoving()
    {
        Vector3 pos = new Vector3(0, 0.7f, 0);
        Vector3 pos2 = new Vector3(0, 0.5f, 0);
        for (int i = 0; i < 10; i++)
        {
            myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, pos2, 0.4f);
            //myCamera.transform.localPosition += new Vector3(0.001f, 0.005f, 0);
            yield return new WaitForSeconds(handSpeed);
        }
        for (int i = 0; i < 10; i++)
        {
            myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, pos, 0.4f);

            //myCamera.transform.localPosition -= new Vector3(0.001f, 0.005f, 0);
            yield return new WaitForSeconds(handSpeed);
        }
        StartCoroutine(IeHandMoving());
    }

    public void SetId(int id)
    {
        this.id = id;
        print(id);
    }
    
    void InventoryCheck()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (player.GetComponentInChildren<DungeonManager>().isDungeon)
                return;
            if (isInventoryOpen) 
            {
                //인벤토리 닫기
        player.GetComponentInChildren<InventoryManager>().InventoryOff();
                isInventoryOpen = false;

            }
            else
            {
                //인벤토리 열기
        player.GetComponentInChildren<InventoryManager>().InventoryOn();
                isInventoryOpen = true;

            }
        }
    }

    public void CursorOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraRotate>().canMove = false;
    }

    public void CursorOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Camera.main.GetComponent<CameraRotate>().canMove = true;
    }

    Item curItem;
    public GameObject selectSoil; //칸 알게해주는 보조 오브젝트
    //클릭했을 때 닿은 물체의 태그가 Soil이면 작물 심기
    void ClickCheck()
    {
        curItem = player.GetComponentInChildren<InventoryManager>().curItem;
        if (player.GetComponentInChildren<InventoryManager>().isOpen)
            return;
        CheckCurLook();
        
        if (Input.GetMouseButtonDown(0))
        {  
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (player.GetComponentInChildren<DungeonManager>().isDungeon)
            {
                player.GetComponentInChildren<DungeonManager>().UseItem();
                if (curItem && curItem.category == ItemCategory.Gun)
            {
                if (Physics.Raycast(ray, out hit, 100))
                {
                    player.GetComponentInChildren<GunManager>().Shoot(hit.point, hit.transform.gameObject);
                }
                else
                {
                    player.GetComponentInChildren<GunManager>().Shoot(ray.GetPoint(100));
                }
                return;
            }
            }
            
            
            if (Physics.Raycast(ray, out hit, 100))
            {
                //print(hit.transform.name +", " + "Tag: "+ hit.transform.tag);
                if (hit.transform.tag == "NPC")
                {
                    SoundManager.instance.PlaySound("Right");
                    //PlayerManager.instance.ZoomIn(id);
                    player.GetComponentInChildren<NPCManager>().TalkPanelIn();
                    CursorOn();
                    return;
                }

                if (curItem && hit.transform.tag == "Ground")
                {
                    if (hit.transform.GetComponent<Ground>().isUsed)
                        return;
                    if (curItem.category == ItemCategory.Seed)
                    {
                        SoundManager.instance.PlaySound("Right");
                        Vector3 groundPos = hit.transform.position;
                        HarvestManager.instance.PlantingCrop(groundPos, curItem, hit.transform);
                    }
                }
                else if(curItem && curItem.category == ItemCategory.BossObj && hit.transform.GetComponent<ObjTable>())
                {
                    if (hit.transform.GetComponent<ObjTable>().activeTrue)
                        return;
            SoundManager.instance.PlaySound("Right");

                    hit.transform.GetComponent<ObjTable>().SetObj(curItem);
                    player.GetComponentInChildren<InventoryManager>().RemoveItem(curItem.name);
                }

                if (hit.transform.GetComponent<Crops>())
                {
                    if (hit.transform.GetComponent<Crops>().isGrow)
                    {
                        print("다 자란 꽃 클릭");
            SoundManager.instance.PlaySound("Right");
                        
                        hit.transform.GetComponent<Crops>().Harvest();

                    }
                }
            }
            
        }
        else if (Input.GetMouseButton(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (curItem && hit.transform.tag == "Ground")
                {
                    if (hit.transform.GetComponent<Ground>().isUsed)
                        return;
                    if (curItem.category == ItemCategory.Seed)
                    {
                        Vector3 groundPos = hit.transform.position;
                        HarvestManager.instance.PlantingCrop(groundPos, curItem, hit.transform);
                    }
                }
                else if (hit.transform.GetComponent<Crops>())
                {
                    if (hit.transform.GetComponent<Crops>().isGrow)
                    {
                        print("다 자란 꽃 클릭");
                        hit.transform.GetComponent<Crops>().Harvest();

                    }
                }
            }
        }
    }

    void CheckCurLook()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Plane");
        if (Physics.Raycast(ray, out hit, 100, ~mask))
        {
            //print(hit.transform.name+ "higiyf");
            if (hit.transform.tag != "Ground")
                selectSoil.SetActive(false);
            else
            {
                selectSoil.SetActive(true);
                Vector3 hitObjectPos = hit.transform.gameObject.transform.position;
                selectSoil.transform.position = new Vector3(hitObjectPos.x, 
                    hitObjectPos.y + 1f, 
                    hitObjectPos.z);
            }
        }
    }
}