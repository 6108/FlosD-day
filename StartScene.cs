using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class StartScene : MonoBehaviourPunCallbacks
{
    public InputField roomNameInput;
    public InputField createPlayerNameInput;
    public GameObject createRoomButton;
    public Transform content;
    public GameObject roomSlotPrefab;
    Dictionary<string, RoomInfo> roomCache = new Dictionary<string, RoomInfo>();

    public GameObject joinRoomPanel;
    public Text joinRoomName;
    public InputField joinPlayerNameInput;
    public GameObject joinRoomButton;

    public GameObject playerPrefab;

    void Start()
    {
        joinRoomPanel.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {

    }

    //방 생성
    public void ClickCreateRoomButton()
    { 
        
        PhotonNetwork.NickName = createPlayerNameInput.text;
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomNameInput.text, roomOption);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        DeleteRoomListUI();
        UpdateRoomCache(roomList);
        CreateRoomListUI();
    }

    void UpdateRoomCache(List<RoomInfo> roomList)
    {

        for(int i = 0; i < roomList.Count; i++)
        {

            if(roomCache.ContainsKey(roomList[i].Name))
            {
                if(roomList[i].RemovedFromList)
                    roomCache.Remove(roomList[i].Name);
                else
                    roomCache[roomList[i].Name] = roomList[i];
            }
            else
                roomCache[roomList[i].Name] = roomList[i];
        }
    }

    void CreateRoomListUI()
    {
        foreach(RoomInfo info in roomCache.Values)
        {
            Button roomSlot = Instantiate(roomSlotPrefab, content).GetComponent<Button>();
            roomSlot.onClick.AddListener(() => ClickRoom(info.Name));
            roomSlot.GetComponentInChildren<Text>().text = info.Name + "(" + info.PlayerCount + "/" + info.MaxPlayers +")";
        }
    }

    void DeleteRoomListUI()
    {
        foreach(Transform t in content)
            Destroy(t.gameObject);
    }
    public string roomName;
    //방들 중 하나를 클릭했을 때, 해당 방에 입장할 것인지 물어보는 판넬 띄움
    public void ClickRoom(string roomName)
    {
        this.roomName = roomName;
        joinRoomPanel.SetActive(true);
        joinRoomName.text = "\"" + roomName + "\"방에 들어가기";
        //=>OnJoinedRoom()
    }

    // 방 참가
    public void ClickJoinButton()
    {
        PhotonNetwork.NickName = joinPlayerNameInput.text;
        PhotonNetwork.JoinRoom(roomName);
        //=>OnJoinedRoom()
    }

    //방참가 취소
    public void ClickCancelButton()
    {
        joinRoomPanel.SetActive(false);
    }

    //방 입징
    public override void OnJoinedRoom()
    {
        CloseScreen();
        //https://mingyu0403.tistory.com/312
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
            }
        }
    }

    public void CloseScreen()
    {
        StartCoroutine("IeCloseScene");
    }

    public Image blackScreen;
    IEnumerator IeCloseScene()
    {
        for (int i = 0; i <= 20; i++)
        {
            yield return new WaitForSeconds(0.05f);
            blackScreen.color = new Color(0, 0, 0, i * 0.05f);

        }
        PhotonNetwork.LoadLevel("MainScene");

    }
}
