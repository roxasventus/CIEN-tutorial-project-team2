using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Weapon, Accessory } // can add more types
    
    [Header("# Main Info")]
    public ItemType type;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDescription;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float baseMagicCircleWait;
    public float[] damages;
    public int[] counts;
    public float[] magicCircleWait;

    [Header("# Weapon")]
    public GameObject projectile;
}
