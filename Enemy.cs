using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weakness
{
    Fire,
    Water,
    Grass,
    None
}

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    public string enemyName;
    public Weakness weakness;
    public int hp = 100;
    public int speed = 5;
    public int damage = 1;
    public int attackDelay = 2;
    public GameObject dropItem;
}
