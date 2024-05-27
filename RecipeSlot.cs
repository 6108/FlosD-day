using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeSlot : MonoBehaviour, IPointerClickHandler
{
    Recipe recipe;
    public bool canMake;

    public void SetItem(Recipe recipe)
    {
        this.recipe = recipe;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (canMake)
            {
            SoundManager.instance.PlaySound("ButtonClick");

                GameObject player = PlayerManager.instance.GetPlayer();
                for (int i = 0; i < recipe.ingredientItem.Length; i++)
                {
                    RecipeItem recipeItem = recipe.ingredientItem[i];
                    player.GetComponentInChildren<InventoryManager>().RemoveItem(recipeItem.item.name, recipeItem.count);
                }
                player.GetComponentInChildren<InventoryManager>().AddItem(recipe.resultItem.item.name, recipe.resultItem.count);
                player.GetComponentInChildren<MergeManager>().RecipeCheck();
            }
            
        }
    }
}
