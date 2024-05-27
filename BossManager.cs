using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    static public BossManager instance;
    bool[] endingTirgger;
    public int objCount;
    public GameObject boss;
    public GameObject bossPanel;

    void Awake() {
        instance = this;
    }

    public void AddBossObj()
    {
        objCount++;
        if (objCount >= 3)
        {
            SoundManager.instance.ChangeBGM("Boss");
            StartCoroutine(IeStartBossStage());
        }
    }

    IEnumerator IeStartBossStage()
    {
        GameObject player = PlayerManager.instance.GetPlayer();
        bossPanel = player.GetComponent<Player>().BossHpPanel;
        player.transform.LookAt(player.GetComponentInChildren<NPCManager>().npc.transform);
        player.GetComponent<Player>().SetAlarmText("이제 하던 일을 마무리해야겠군요.");
        boss.GetComponent<Boss>().bossStage = true;
        yield return new WaitForSeconds(5f);
        player.GetComponentInChildren<NPCManager>().npc.SetActive(false);
        boss.SetActive(true);
        player.GetComponentInChildren<DungeonManager>().DungeonInventorySelectOn();
        player.GetComponentInChildren<InventoryManager>().InventoryOn();
        yield return new WaitForSeconds(5f);
        ClickReady();
    }

    public void ClickReady()
    {
        GameObject player = PlayerManager.instance.GetPlayer();
        player.transform.LookAt(boss.transform);
        bossPanel.SetActive(true);
    }
}
