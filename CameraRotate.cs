using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraRotate : MonoBehaviourPunCallbacks
{
    float rx;
    float ry;
    public float rotSpeed = 200; 
    
    public bool canMove = true;

    void Update()
    {
        if (!photonView.IsMine)
            return;
        if (!canMove)
            return;
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        rx += my * rotSpeed * Time.deltaTime; 
        ry += mx * rotSpeed * Time.deltaTime;
        rx = Mathf.Clamp(rx, -80, 80);
        transform.eulerAngles = new Vector3(-rx, ry, 0); 
        //transform.rotation = Quaternion.Euler(-rx, ry, 0);
    }
}