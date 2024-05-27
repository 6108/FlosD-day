using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    int hp = 1000;
    GameObject[] players;

    public GameObject warningImage;
    public GameObject target;
    public GameObject thorn;

    public Transform[] allBulletPos;
    public GameObject bulletPrefab;

    public bool bossStage;


    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player"); 
    }

    void Update()
    {
        if (!bossStage)
            return;
        if (hp >= 0)
            Phase1();
    }

    bool canAttack = true;

    void Phase1()
    {
        if (canAttack)
            StartCoroutine(IePhase1());
        warningImage.SetActive(false);
        warningImage.transform.position = Vector3.Lerp(warningImage.transform.position,
            target.transform.position, 0.1f);
    }

    IEnumerator IePhase1()
    {
        canAttack = false;
        int randomPlayer = Random.Range(0, players.Length);
        target = players[randomPlayer];
        yield return new WaitForSeconds(1f);
        int random = Random.Range(1, 2);
        if (random == 0)
        {
            Vector3 pos = target.transform.position;
            yield return new WaitForSeconds(1);
            for (int i = 0; i < thorn.transform.childCount; i ++)
            {
                thorn.transform.GetChild(i).gameObject.SetActive(true);
                thorn.transform.position = pos;
                for (float j = 0; j < 1; j += 0.1f)
                {
                    thorn.transform.position = new Vector3(pos.x, pos.y + j, pos.z);
                    yield return new WaitForSeconds(0.01f);
                }
                for (float j = 1; j > 0; j -= 0.1f)
                {
                    thorn.transform.position = new Vector3(pos.x, pos.y + j, pos.z);
                    yield return new WaitForSeconds(0.01f);
                }
                thorn.transform.GetChild(i).gameObject.SetActive(false);
            }
            canAttack = true;
        }
        else if (random == 1)
        {
            for (int i = 0; i < allBulletPos.Length; i++)
            {
                allBulletPos[i].gameObject.SetActive(true);
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.transform.position = allBulletPos[i].transform.position;
                bullet.GetComponent<BossBullet>().target = target;
                yield return new WaitForSeconds(0.2f);
                allBulletPos[i].gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(15f);
            canAttack = true;
        }
    }

    public void hit(int i)
    {
        Scrollbar hpScroll = BossManager.instance.bossPanel.GetComponent<Scrollbar>();
        hp -= i;
        hpScroll.size = hp/1000f;
        if (hp <= 0)
            EndManager.instance.Ending();
    } 
}
