using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeManager : MonoBehaviour
{
    public Recipe[] recipes;
    public GameObject mergeCanvas;
    public GameObject content;
    public GameObject recipSlotPrefab;
    public bool isOpen;

    public void MergeCanvasOn()
    {
        RecipeCheck();
        mergeCanvas.SetActive(true);
        isOpen = true;
        GameObject player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<NPCManager>().TalkPanelOut();

    }

    public void MergeCanvasOff()
    {
        isOpen = false;
        mergeCanvas.SetActive(false);
    }
    
    void Start()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            GameObject recipeSlot = Instantiate(recipSlotPrefab, content.transform);
            //recipeSlot.transform.parent = content.transform;
            Transform userItem = recipeSlot.transform.GetChild(0);
            for (int j = 0; j < recipes[i].ingredientItem.Length; j++)
            {
                Transform itemImage = userItem.transform.GetChild(j);
                itemImage.gameObject.SetActive(true);
                itemImage.GetComponent<Image>().sprite = recipes[i].ingredientItem[j].item.itemImage;
                itemImage.GetComponentInChildren<Text>().text = recipes[i].ingredientItem[j].count +"";
            }
            Transform resultItem = recipeSlot.transform.GetChild(1);
            resultItem.GetComponent<Image>().sprite = recipes[i].resultItem.item.itemImage; 
            resultItem.GetComponentInChildren<Text>().text = recipes[i].resultItem.count + "";
            recipeSlot.GetComponent<RecipeSlot>().SetItem(recipes[i]);
            //print(recipes[i].ingredientItem +", " + recipes[i].resultItem);
        }
        RecipeCheck();
    }

    //재료가 있는 레시피만 활성화
    public void RecipeCheck()
    {
        for (int i = 0; i < recipes.Length; i++)
        {
            for (int j = 0; j < recipes[i].ingredientItem.Length; j++)
            {
                if (IngredientCheck(recipes[i].ingredientItem[j].item , recipes[i].ingredientItem[j].count))
                {
                    print(recipes[i].ingredientItem[j]);
                    content.transform.GetChild(i).GetComponent<RecipeSlot>().canMake = true;
                    content.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(false);
                }
                else 
                {
                    content.transform.GetChild(i).GetComponent<RecipeSlot>().canMake = false;
                    content.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(true);
                    break;
                }
            }

        }
    }

    //재료가 인베노리에 있는지 확인
    public bool IngredientCheck(Item item, int count)
    {
        GameObject player = PlayerManager.instance.GetPlayer();
        for (int i = 0; i < player.GetComponentInChildren<InventoryManager>().inventoryItemList.Count; i++)
        {
            if (item == player.GetComponentInChildren<InventoryManager>().inventoryItemList[i])
            {
                if (player.GetComponentInChildren<InventoryManager>().inventoryItemCount[i] >= count)
                    return true;
                return false;
            }
        }
        return false;
    }

    public void ClickCancelBtn()
    {
        MergeCanvasOff();
        GameObject player = PlayerManager.instance.GetPlayer();
        player.GetComponentInChildren<NPCManager>().TalkPanelIn();
        player.GetComponentInChildren<InventoryManager>().InventoryOff();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Camera.main.GetComponent<CameraRotate>().canMove = false;

    }
}
