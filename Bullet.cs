using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Bell,
    Canation,
    Dandelion,
    None
}

public class Bullet : MonoBehaviour
{
    public BulletType bulletType;
    public Vector3 targetPos;
    int damage;
    public int speed = 10;
    Vector3 dir;
    public GameObject bulletParticlePrefab;
    public GameObject enemyParticlePrefab;
    GameObject owner;

    void Start()
    {
        dir = targetPos - transform.position;
    }

    void Update()
    {
        transform.position += Time.deltaTime * dir * speed;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
    }


    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy")
        {
            SoundManager.instance.PlaySound("Explosion");
            int randomDamage = Random.Range(damage, damage*2);
            other.GetComponent<EnemyMove>().Hit(randomDamage, bulletType, enemyParticlePrefab, owner);
            GameObject particle = Instantiate(bulletParticlePrefab); 
            particle.transform.position = transform.position;
            transform.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 3f);
        }
        if (other.tag == "Boss")
        {SoundManager.instance.PlaySound("Explosion");
            int randomDamage = Random.Range(damage, damage*2);
            other.GetComponent<Boss>().hit(randomDamage);
            GameObject particle = Instantiate(bulletParticlePrefab); 
            particle.transform.position = transform.position;
            transform.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 3f);
        }
    }
    
}
