using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    public Enemy enemy;
    float curHp;
    NavMeshAgent agent;
    public GameObject target;
    public Vector3 tragetPos;
    public Slider hpSlider;

    public int randomRange = 100;
    Rigidbody rb;

    Animator anim;

    bool isDeath;
    void Start()
    {
        //enemy = GetComponent<Enemy>();
        curHp = enemy.hp;
        agent = GetComponent<NavMeshAgent>();  
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(1).transform.GetComponent<Animator>();
    }

    void Update()
    {
        if (isDeath)
            return;
        if (!target)
        {
            RandomMove();
        }
        else
        {
            ChasePlayer();
        }
        
    }

    IEnumerator IeAttack()
    {
        if (canAttack)
        {
            
            yield return new WaitForSecondsRealtime(1f);
            canAttack = true;
        }

    }

    //해당 공격과 관련된 +a 공격 맞기 전까지 효과 파티클 재생 
    //효과 파티클이 이미 있으면 득수 효과 주고 효과 파티클 삭제
    //
    GameObject curParticle;
    BulletType curEffect;

    public void Hit(float damage, BulletType bulletType, GameObject particle, GameObject owner)
    {
        target = owner;
        anim.SetTrigger("Hit");
        if (!particle)
        {
            HpChange(damage);
        }
        else if (!curParticle)
        {
            curEffect = bulletType;
            curParticle = Instantiate(particle);
            curParticle.transform.position = transform.position;
            curParticle.transform.SetParent(transform);     
            HpChange(damage);
        }
        //특수 효과
        //방울꽃 + 카네이션 =  폭발
        //방울꼴 + 민들레 = 슬로우
        //카네이션 + 민들레 = 크리티컬
        else if (curEffect != bulletType)
        {
            if (curEffect == BulletType.Bell)
            {
                if (bulletType == BulletType.Canation)
                    Explosion(damage);
                else if (bulletType == BulletType.Dandelion)
                    StartCoroutine(IeSlow());
            }
            else if (curEffect == BulletType.Canation)
            {
                if (bulletType == BulletType.Bell)
                    Explosion(damage);
                else if (bulletType == BulletType.Dandelion)
                    Critical(damage);
            }
            else if (curEffect == BulletType.Dandelion)
            {
                if (bulletType == BulletType.Bell)
                    StartCoroutine(IeSlow());
                else if (bulletType == BulletType.Canation)
                    Critical(damage);
            }           
            curEffect = BulletType.None;
            Destroy(curParticle);
        }
        else
        {
            HpChange(damage);
        }

    }

    public GameObject damageObjPrefab;
    void HpChange(float damage)
    {
        
        int random = Random.Range(1, 4);
        for(int i = 0; i < 3; i++)
        {
            GameObject damageObj = Instantiate(damageObjPrefab);
            damageObj.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = damage + "";
            damageObj.transform.position = transform.position + new Vector3(0, 1, 0);
            damageObj.transform.forward = transform.forward;
            damageObj.GetComponent<Rigidbody>().AddForce(Vector3.up*200 + Random.insideUnitSphere * 200);
            Destroy(damageObj, 3);
            curHp -= damage;
        }
        hpSlider.value = curHp / enemy.hp;
        if (curHp <= 0)
        {
            anim.SetTrigger("Death");
            isDeath = true;
            if (enemy.dropItem)
            {
                GameObject dropItem = Instantiate(enemy.dropItem);
                dropItem.transform.position = transform.position;
            }
            Destroy(gameObject, 3f);
        }
    }

    //주변 몹에 (0.2% 데미지)
    //폭발
    void Explosion(float damage)
    {
        print("폭발");
        HpChange(damage);
        Collider[] aroundCollider = Physics.OverlapSphere(transform.position, 5);
        foreach(Collider collider in aroundCollider)
        {
            if (collider.GetComponent<EnemyMove>())
            {
                collider.GetComponent<EnemyMove>().Hit(damage * 0.2f, BulletType.None, null, target);
            }
        }
    }

    //속도 감소
    IEnumerator IeSlow()
    {
        print("속도감소");
        agent.speed = 1;
        yield return new WaitForSeconds(2);
        agent.speed = 2;
    }

    //높은 데미지 (1.5배)
    void Critical(float damage)
    {
        print("크리티컬");
        HpChange(damage * 1.5f);
    }

    bool canAttack = true;

    IEnumerator IeDelay()
    {
        yield return new WaitForSecondsRealtime(enemy.attackDelay);
        canAttack = true;
    }

    bool isRandomMove;

    void RandomMove()
    {
        if (!isRandomMove)
        {
            isRandomMove= true;
            

            float randomX = Random.Range(0f, randomRange * 2) - randomRange;
            float randomZ = Random.Range(0f, randomRange * 2) - randomRange;
            Vector3 randomPos = transform.position + new Vector3(randomX, 0, randomZ);
            //tragetPos = randomPos;
            agent.SetDestination(randomPos);
            StartCoroutine(IeSetRandomTarget());
        }
    }

    IEnumerator IeSetRandomTarget()
    {
        agent.isStopped = false;
        anim.SetBool("Run", true);
        yield return new WaitForSeconds(1f);
        agent.isStopped = true;
        anim.SetBool("Run", false);
        yield return new WaitForSeconds(2f);
        // agent.isStopped = false;
        // anim.SetBool("Run", true);
        isRandomMove = false;


    }

    void ChasePlayer()
    {
        agent.SetDestination(target.transform.position);
        if (Vector3.Distance(transform.position, target.transform.position) <= 3f)
        {
            print("적이 가까이 옴");
            print(transform.position + ", " + target.transform.position);

            agent.isStopped = true;
            anim.SetBool("Run", false);
            agent.velocity = Vector3.zero;
            if (canAttack)
            {
                canAttack = false;
        anim.SetTrigger("Attack");
                target.transform.GetComponent<Player>().Hit(enemy.damage);
                StartCoroutine(IeDelay());
            }
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("Run", true);
        }


    }
}
