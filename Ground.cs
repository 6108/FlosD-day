using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//식물이 심겨져있는지 판단
//물뿌리개를 든 상태로 상호작용하면 땅 색깔 변경
//10초가 지나면 다시 원래 땅 색깔로 돌아옴
public class Ground : MonoBehaviourPunCallbacks
{
    public bool isUsed;
    public Mesh normalMesh;
    public Mesh wetMesh;

    public void Plant()
    {
        isUsed = true;
        transform.GetComponent<MeshFilter>().mesh = wetMesh;
    }

    public void Harvest()
    {
        print(transform.childCount);
        if (transform.childCount <= 1)
        {
            isUsed = false;
            transform.GetComponent<MeshFilter>().mesh = normalMesh;
        }
    }
}
