using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    static public StageManager instance;

    public int[] stageEnemyCount;
    public GameObject[] stageDoor;
    public GameObject[] stageObj;
    int killEnemy = 0;
    int curStage = 1;
    GameObject player;

    void Start()
    {
        instance = this;
    }

    public void EnterStage()
    {
        player = PlayerManager.instance.GetPlayer();
        print("StageManagerPlayer: " + player);
        player.GetComponent<Player>().SetAlarmText("오브젝트를 찾아라");
    }

    public void GetObj()
    {
        player.GetComponent<Player>().SetAlarmText("길을 막고있던 덩쿨이 사라졌다");
        stageDoor[curStage - 1].SetActive(false);
    }

    public void NextStage()
    {
        curStage++;
    }
}
