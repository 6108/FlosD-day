using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class EndManager : MonoBehaviour
{
    static public EndManager instance;

    void Awake()
    {
        instance = this;
    }

    public void Ending()
    {
        GameObject player = PlayerManager.instance.GetPlayer();
        GameObject endpanel = player.GetComponent<Player>().endPanel;
        endpanel.SetActive(true);
        endpanel.transform.GetComponentInChildren<Button>().onClick.AddListener(ClickGoStartScene);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ClickGoStartScene()
    {
        PhotonNetwork.LoadLevel("StartScene");
    }
}
