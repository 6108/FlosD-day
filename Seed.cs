using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Seed : MonoBehaviourPunCallbacks
{
    public GameObject flowerPrefab;
    public Transform parent;
    GameObject flower;
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        int random = Random.Range(1, 3);
        for (int i = 0; i < random; i++)
        {
            print("Rhc todtjd");
            float x, z;
            x = transform.position.x - 0.5f + Random.Range(0f, 1f);
            z = transform.position.z - 0.5f + Random.Range(0f, 1f);
            flower = PhotonNetwork.Instantiate(flowerPrefab.name, new Vector3(x, 0, z),
                Quaternion.Euler(0, Random.Range(0, 360), 0), 0);
            int id = HarvestManager.instance.GetGroundId(parent.gameObject);
            int flowerId = flower.GetComponent<PhotonView>().ViewID;
            pv.RPC("SetParent", RpcTarget.All, flowerId, id);
        }
        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void SetParent(int flowerId, int id)
    {
        PhotonView.Find(flowerId).GetComponent<Crops>().SetParent(id);
    }
}
