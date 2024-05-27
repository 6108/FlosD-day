using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject bulletEffect;
    public GameObject target;
    Vector3 dir;

    void Start()
    {
        dir = target.transform.position - transform.position;
    }

    private void Update() {
        
        transform.position += Time.deltaTime * dir * 4;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            other.GetComponentInChildren<Player>().Hit(1);
        }
        GameObject particle = Instantiate(bulletEffect);
        particle.transform.position = transform.position;
        GameObject player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<Player>().CameraShake(1);
        SoundManager.instance.PlaySound("Explosion");
        Destroy(this.gameObject);
    }
}
