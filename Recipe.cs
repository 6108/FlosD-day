using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeItem
{
    public Item item;
    public int count;
}

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public RecipeItem[] ingredientItem;
    public RecipeItem resultItem;
}