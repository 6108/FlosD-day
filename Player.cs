using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerInfo
{
    public string playerName;
    public int hp = 5;
    public int atk = 100;
    public int def = 10;
}

public class Player : MonoBehaviourPunCallbacks
{
    public PlayerInfo playerInfo = new PlayerInfo();
    public GameObject[] hpImage;
    public Text timeText;
    public Scrollbar timeSlider;
    public Camera myCamera;
    public GameObject player;

    //오른쪽 상단 알림
    public Image alarm;
    public Text alarmText;

    //던전에서 효과 시간 표시
    public Text effectText;
    public Scrollbar effectTimeSlider;
    //FoodCategory curEffect;
    float effectTime;
    float curEffectTime;
    int effect;

    //공격 쿨타임 표시
    public Scrollbar coolTimeSlider;
    public GameObject BossHpPanel;

    public GameObject endPanel;
    
    void Start()
    {
        playerInfo.atk = 10;
        playerInfo.def = 5;
        if (photonView.IsMine)
        {
            myCamera.gameObject.SetActive(true);

        }
        else
        {
            myCamera.gameObject.SetActive(false);
            timeSlider.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
        }
        StartCoroutine(FindPlayer());

    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(1f);
        player = PlayerManager.instance.GetPlayer();
        if (!player)
            StartCoroutine(FindPlayer());
    }

    public void SetDay(int day)
    {
        timeText.text = "D-" + day;
    }

    public void SetTime(float time)
    {
        timeSlider.value = time;
    }

    public void SetAlarmText(string text)
    {
        alarmText.text = text;
        if(!alarmActive)
            StartCoroutine(IeSetAlarmText());
    }

    bool alarmActive;
    IEnumerator IeSetAlarmText()
    {
        alarmActive = true;
        int count = 20;
        alarmText.gameObject.SetActive(true);
        for (int i = 0; i < count; i++)
        {
            alarm.rectTransform.localPosition -= new Vector3(0f, 100f/count, 0f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < count; i++)
        {
            alarm.rectTransform.localPosition += new Vector3(0f, 100f/count, 0f);
            yield return new WaitForSeconds(0.02f);
        }
        alarmText.gameObject.SetActive(false);
        alarmActive = false;
    }

    ///플레이어 화면에 '효과 시간 슬라이더' 설정하고 시간 잼
    ///time: 효과 적용되는 시간
    public void SetEffectTime(float time, string effectName, int effect, FoodCategory category)
    {
        //시간이 끝나기 전에 새 효과가 들어오면 효과와 시간 초기화
        if (curEffectTime != 0)
        {
            StopCoroutine("IeSetEffectTime");
            StopEffect(category);
        }
        this.effect = effect;
        effectText.gameObject.SetActive(true);
        effectText.text = effectName;
        effectTimeSlider.size = 1;
        effectTime = time;
        curEffectTime = 0;
        StartCoroutine("IeSetEffectTime", category);
    }

    IEnumerator IeSetEffectTime(FoodCategory category)
    {
        //시간 끝나면 코루틴 종료, Text 끄기
        if (curEffectTime >= effectTime)
        {
            StopEffect(category);
            effectText.gameObject.SetActive(false);
            curEffectTime = 0;
            yield return null;
        }   
        effectTimeSlider.size = 1 - curEffectTime/effectTime;
        yield return new WaitForSeconds(1f);
        curEffectTime++;
        StartCoroutine("IeSetEffectTime", category);
    }

    void StopEffect(FoodCategory category)
    {
        if (category == FoodCategory.Peanut)
        {
            player.GetComponent<Player>().playerInfo.atk -= effect;
        }
        else if (category == FoodCategory.Amond)
        {
            player.GetComponent<Player>().playerInfo.def -= effect;
        }
    }

    bool isSuper;
    public GameObject hitPanel;
    public void Hit(int damage)
    {
        if (isSuper)
            return;
        hitPanel.SetActive(true);
        
        StartCoroutine(IeSuper());
        isSuper = true;
        
        playerInfo.hp -= damage;
        if (playerInfo.hp <= 0)
            playerInfo.hp = 0;
        for (int i = hpImage.Length - 1; i >= playerInfo.hp; i--)
        {
            hpImage[i].GetComponent<Image>().enabled = false;
        }
        if (playerInfo.hp <= 0)
        {
            Death();
        }
    }
    
    IEnumerator IeSuper()
    {
        CameraShake(1);
        hitPanel.SetActive(false);
        hitPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(1.5f);
        isSuper = false;
    }

    void Death()
    {
        GameObject player = PlayerManager.instance.GetPlayer();
        player.transform.position = new Vector3(0, 1, 0);
        player.GetComponentInChildren<DungeonManager>().GoHome();
        SetAlarmText("부상을 입었습니다.");
        playerInfo.hp = 5;
        for (int i = 0; i <= hpImage.Length; i++)
        {
            hpImage[i].GetComponent<Image>().enabled = true;
        }
    }

    public void ResetHp()
    {

     
    }

    public void CameraShake(int power)
    {
        StartCoroutine("IeCameraShake", power);
    }

    IEnumerator IeCameraShake(int power)
    {
        Vector3 originPos = myCamera.transform.position;
        for (int i = 0; i < 5; i++)
        {
            hitPanel.GetComponent<Image>().color = new Color(1, 1, 1, 1 - i * 0.2f);
            myCamera.transform.position = Random.insideUnitSphere * 0.5f * power + originPos;
            yield return new WaitForSeconds(0.01f);
        }
        myCamera.transform.position = originPos;
    }
}
