using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    static public ScreenTransition instance;

    public Image upBack;
    public Image upFront;
    public Image downBack;
    public Image downFront;

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        //CloseOpenScreen();
    }

    public void CloseOpenScreen()
    {
        StartCoroutine(IeCloseOpenScreen());
    }

    IEnumerator IeCloseOpenScreen()
    {
        //이미지 인
        for(int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.01f);
            upBack.rectTransform.sizeDelta += new Vector2(0, 450f / 50);
            upFront.rectTransform.sizeDelta += new Vector2(0, 100f / 50);
            downBack.rectTransform.sizeDelta += new Vector2(0, 450f / 50);
            downFront.rectTransform.sizeDelta += new Vector2(0, 180f / 50);
        }
        for(int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.01f);
            upFront.rectTransform.sizeDelta += new Vector2(0, 100f / 50);
            downFront.rectTransform.sizeDelta += new Vector2(0, 180f / 50);
        }
        yield return new WaitForSeconds(0.2f);

        //이미지 아웃
        for(int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.01f);
            upFront.rectTransform.sizeDelta -= new Vector2(0, 100f / 50);
            downFront.rectTransform.sizeDelta -= new Vector2(0, 180f / 50);
        }
        for(int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.01f);
            upBack.rectTransform.sizeDelta -= new Vector2(0, 450f / 50);
            upFront.rectTransform.sizeDelta -= new Vector2(0, 100f / 50);
            downBack.rectTransform.sizeDelta -= new Vector2(0, 450f / 50);
            downFront.rectTransform.sizeDelta -= new Vector2(0, 180f / 50);
        }


    }

    public void CloseScreen()
    {
        StartCoroutine("IeCloseScene");
    }

    IEnumerator IeCloseScene()
    {
        //이미지 인
        for(int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.01f);
            upBack.rectTransform.localPosition -= new Vector3(0f, 300f / 50, 0f);
            upFront.rectTransform.localPosition -= new Vector3(0f, 150f / 50, 0f);
            downBack.rectTransform.localPosition += new Vector3(0f, 300f / 50, 0f);
            downFront.rectTransform.localPosition += new Vector3(0f, 150f / 50, 0f);
        }
        for(int i = 0; i < 50; i++)
        {
            yield return new WaitForSeconds(0.01f);
            upFront.rectTransform.localPosition -= new Vector3(0f, 150f / 50, 0f);
            downFront.rectTransform.localPosition += new Vector3(0f, 150f / 50, 0f);

            //upFront.rectTransform.sizeDelta += new Vector2(0, 100f / 50);
            //downFront.rectTransform.sizeDelta += new Vector2(0, 180f / 50);
        }
        yield return new WaitForSeconds(0.2f);
    }
}
