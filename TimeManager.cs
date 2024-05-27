using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class TimeManager : MonoBehaviourPunCallbacks
{
    
    int dDay = 7;
    float curTime = 0;
    float dayTime = 300; //초단위
    PhotonView pv;

    public GameObject light;
    GameObject player;
    //방장만 타이머 코루틴 실행
    void Start()
    {
        pv = GetComponent<PhotonView>();
        //player = PlayerManager.instance.GetPlayer();
        //print("Player" + player);
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("IeTimer");
        }
    }

    
    IEnumerator IeTimer()
    {
        
        yield return new WaitForSeconds(1f);
        if (curTime > dayTime)
        {
            curTime = 0;
            dDay--;
        }
        curTime += 1f;
        pv.RPC("SetTimer", RpcTarget.All, dDay, curTime/dayTime);
        StartCoroutine("IeTimer");
    }

    [PunRPC]
    void SetTimer(int d, float t)
    {
        player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<Player>().SetDay(d);
        player.GetComponentInChildren<Player>().SetTime(t);
    }
}
