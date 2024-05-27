using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    Transform target;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        Vector3 playerPos = target.position;
        playerPos.y = transform.position.y;
        transform.LookAt(playerPos);
    }
}
