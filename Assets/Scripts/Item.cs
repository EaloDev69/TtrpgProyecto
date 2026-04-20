using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "scriptableObjects/Items")]
public class Item : ScriptableObject
{
    [Header("Only UI")]
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5,4);
    
    [Header("Only UI")]
    public bool stackable;

    [Header("Both")]
    public Sprite image; 

    public enum ItemType
    {
        Weapon,
        Consumable,
        Collectible
    }

    public enum ActionType
    {
        Melee,
        Ranged,
        Heal,
        Buff,
        Debuff
    }
}
