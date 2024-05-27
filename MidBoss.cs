using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//순간 이동 후 원거리 공격
//돌진
//2페
//바닥에서 뭐가 나옴
public class MidBoss : MonoBehaviour
{
    List<int> attackPattern = new List<int>();
    int hp = 2000;
    int speed = 20;
    GameObject[] target;

    //1페
    public Transform[] telePos;
    public GameObject bulletPrefab;

    void Start()
    {
        attackPattern.Add(0);
        attackPattern.Add(1);
        target = GameObject.FindGameObjectsWithTag("Player");
    }

    bool isPhase2;
    void Update()
    {
        if (hp <= 1000 && !isPhase2)
        {
            isPhase2 = true;
            speed = 30;
            attackPattern.Add(2);
        }
    }
    bool canAttack = true;

    public void StartAttack()
    {
        
            //canAttack = false;
            StartCoroutine(IeStartAttack());
            
    }

    IEnumerator IeStartAttack()
    {
        int random = Random.Range(0, attackPattern.Count);
        if (random == 1)
        {
            int randomPos = Random.Range(0, telePos.Length);
            transform.position = telePos[randomPos].position;
            GameObject bullet = Instantiate(bulletPrefab);
            yield return new WaitForSeconds(2f);
        }
        else if (random == 2)
        {

        }
        //canAttack = true;
        StartAttack();
    }


}
