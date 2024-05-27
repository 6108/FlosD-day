using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodCategory
{
    Peanut, //공격력 증가
    Amond, //방어력 증가
    CashewNut //체력+
}

public class Food : MonoBehaviour
{
    public FoodCategory category;
    public int level = 1;
    public int effect;
    public float effectTime = 30f;
    GameObject player;

    ///음식을 먹었을 때 실행
    public void EatFood()
    {
        player = PlayerManager.instance.GetPlayer();
        if (category == FoodCategory.Peanut)
        {
            effect = level * 20;
            effectTime = effectTime + level * 10;
            player.GetComponent<Player>().playerInfo.atk += effect;
            player.GetComponent<Player>().SetEffectTime(effectTime, "공격력 증가", effect, category);

        }
        else if (category == FoodCategory.Amond)
        {
            effect = level * 3;
            effectTime = effectTime + level * 10;
            player.GetComponent<Player>().playerInfo.def += effect;
            player.GetComponent<Player>().SetEffectTime(effectTime, "방어력 증가", effect, category);
            
        }
        else if (category == FoodCategory.CashewNut)
        {
            player.GetComponent<Player>().playerInfo.hp += level;
        }
    }

    IEnumerator IeDeleteEffect()
    {
        yield return new WaitForSeconds(effectTime);
        if (category == FoodCategory.Peanut)
        {
            player.GetComponent<Player>().playerInfo.atk -= effect;
        }
        else if (category == FoodCategory.Amond)
        {
            player.GetComponent<Player>().playerInfo.def += effect;
        }
    }
}
