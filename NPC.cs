using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public string[] lines;
    public GameObject talkBubble;
    Text line;

    void Start()
    {
        line = talkBubble.GetComponent<Text>();
        line.text = lines[0];
        StartCoroutine(IeDaliyText());
    }

    bool isDaily = true;

    //기본 대화
    IEnumerator IeDaliyText()
    {
        while(isDaily)
        {
            for (int i = 0; i < 3; i++)
            {
                if (!isDaily) break;
                line.text = lines[i];
                yield return new WaitForSeconds(3);

            }
        }
    }

    public void TextChange(int i)
    {
        isDaily = false;
        StartCoroutine("IeTextChange", i);
    }

    //대사 변경하고 3초 뒤 기본 대사로 돌아감
    IEnumerator IeTextChange(int i)
    {
        line.text = lines[i];
        yield return new WaitForSeconds(3);
        isDaily = true;
        StartCoroutine(IeDaliyText());
    }
}
