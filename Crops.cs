using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Crops : MonoBehaviourPunCallbacks
{
    public GameObject cropPrefab;
    public string cropName = "";
    //public int growTime = 3;
    public int price = 50;

    public bool isGrow;
    public float curTime;

    public GameObject[] growObject;
    public int[] growTime = new int[] {1, 2};
    PhotonView pv;

    void Start()
    {
        curTime = 0;
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        pv = GetComponent<PhotonView>();
    }


    public void SetParent(int id)
    {
        print("ID: " + id);
        transform.parent = HarvestManager.instance.GetGroundObj(id).transform;
    }

    //다 자라면 자식0번(열매) 보이게
    void Update()
    {
        if(isGrow)
            return;
        curTime += Time.deltaTime;
        if (curTime > growTime[1])
        {
            isGrow = true;
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (curTime > growTime[0])
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void Harvest()
    {
        Vector3 pos = new Vector3(transform.position.x,
            transform.position.y + 1, transform.position.z);
        Quaternion rot = Quaternion.Euler(transform.rotation.x, 
            transform.rotation.y, transform.rotation.z);
        GameObject crop = PhotonNetwork.Instantiate(cropPrefab.name, pos, rot, 0);
        print("다 자란 꽃 클릭2");
        crop.transform.localScale /= 2f;
        crop.GetComponent<Rigidbody>().AddForce(Vector3.up*100 + Vector3.forward*100);
        
        DestroyCrop();
    }

    [PunRPC]
    void DestroyCrop()
    {
        transform.parent.GetComponent<Ground>().Harvest();
        Destroy(this.gameObject);
    }
}
