using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    static public PlayerManager instance;

    public GameObject[] players = new GameObject[2];
    public int[] playerId = new int[2];
    public Camera[] playerCameras = new Camera[2];
    public GameObject playerPrefab;
    
    void Awake()
    {
        if (!instance)
            instance = this;
    }

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(10, 1, -10), new Quaternion(0, 0, 0, 0), 0);
        FindPlayer();
    }

    [PunRPC]
    void FindPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

    }


    public GameObject GetPlayer()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (player.GetPhotonView().IsMine) 
            {
                return player;
            }
        }
        return null;
    }
}
