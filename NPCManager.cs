using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    [TextArea]
    public string[] talk;

    public Image p1TalkImage;
    public Image p1TalkButtons;

    public Text talkText;

    public Button mergeBtn;
    public Button powerUpBtn;
    public Button talkBtn;

    int repeat = 50;

    public GameObject npc;
    GameObject player;

    void Start()
    {
        npc = GameObject.FindWithTag("NPC");
        powerUpBtn.onClick.AddListener(ClickMergeButton);
        mergeBtn.onClick.AddListener(ClickPowerUpButton);
        talkBtn.onClick.AddListener(ClickP1TalkQuit);
        SetTalkText("도움이 필요하신가요?");
        player = PlayerManager.instance.GetPlayer();
    }

    void Update()
    {
        if (!player)
            player = PlayerManager.instance.GetPlayer();

        npc.transform.LookAt(player.transform);
    }

    public Vector3 GetNPCPosition()
    {
        return npc.transform.position;
    }

    public void SetTalkText(string s)
    {
        talkText.text = s;
    }
    public void TalkPanelIn()
    {
        StartCoroutine(IeTalkPanelIn());
    }
    //대화창 In
    IEnumerator IeTalkPanelIn()
    {

        for(int i = 0; i < repeat; i++)
        {
            yield return new WaitForSeconds(0.005f);
            p1TalkImage.rectTransform.localPosition += new Vector3(0, 200f/repeat, 0);
            p1TalkButtons.rectTransform.localPosition += new Vector3(-200f/repeat, 0, 0);
        }
    }
    

    public void TalkPanelOut()
    {
        StartCoroutine(IeTalkPanelOut());
    }

    //대화창 In
    IEnumerator IeTalkPanelOut()
    {
        
        for(int i = 0; i < repeat; i++)
        {
            yield return new WaitForSeconds(0.005f);
            p1TalkImage.rectTransform.localPosition -= new Vector3(0, 200f/repeat, 0);
            p1TalkButtons.rectTransform.localPosition -= new Vector3(-200f/repeat, 0, 0);
        }
    }

    public void ClickMergeButton()
    {
        GameObject player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<MergeManager>().MergeCanvasOn();
        player.GetComponentInChildren<InventoryManager>().InventoryOn();
    }

    public void ClickPowerUpButton()
    {
        GameObject player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<GunManager>().GunPanelOn();
        player.GetComponentInChildren<InventoryManager>().InventoryOn();
    }

    public void ClickP1TalkQuit()
    {
        PlayerManager.instance.players[0].GetComponent<PlayerMove>().CursorOff();
        TalkPanelOut();
    }

}
