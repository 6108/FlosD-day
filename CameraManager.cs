using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager instance;

    public Camera[] playerCameras = new Camera[2];
    GameObject NPC;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < PlayerManager.instance.players.Length; i++)
        {
            playerCameras[i] = PlayerManager.instance.players[i].transform.GetChild(0).GetComponent<Camera>();
        }
        NPC = GameObject.FindWithTag("NPC");
    }

    
}
