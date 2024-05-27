using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HarvestManager : MonoBehaviourPunCallbacks
{
    public static HarvestManager instance;
    public GameObject[] grounds;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        grounds = GameObject.FindGameObjectsWithTag("Ground");
    }

    public int GetGroundId(GameObject g)
    {
        for(int i = 0; i < grounds.Length; i++)
        {
            if (g == grounds[i])
                return i;
        }
        return -1;
    }

    public GameObject GetGroundObj(int i)
    {
        return grounds[i];
    }

    Grid grid;
    //농작물 심기
    public void PlantingCrop(Vector3 pos, Item item, Transform ground)
    {
        if (ground.GetComponent<Ground>().isUsed)
            return;
        //ground.GetComponent<Ground>().Plant();
        //GameObject crop = Instantiate(item.itemObj, ground);
        GameObject crop = PhotonNetwork.Instantiate(item.itemObj.name, pos, Quaternion.identity, 0);
        crop.transform.position = pos;
        crop.GetComponent<Seed>().parent = ground;
        //GameObject crop = PhotonNetwork.Instantiate(item.itemObj.name, pos, Quaternion.identity, 0);
        GameObject player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<InventoryManager>().RemoveItem(item.name);
        SetCrop(crop, ground);

        
    }

    [PunRPC]
    void SetCrop(GameObject crop, Transform ground)
    {
        //crop.GetComponent<Seed>().parent = ground;
        ground.GetComponent<Ground>().Plant();
    }
}
