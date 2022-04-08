using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item",menuName = "Item")]
public class Item : ScriptableObject
{
    public enum type
    {
        Ore,
        Device,
        Misc
    }
    public int ID;
    public Sprite item_sprite;
    public string item_name;
    public string item_desc;
    public float item_weight;

    public int item_base_price;
    public int item_rarity;
    public bool item_appearInTrade = true;
    public float item_basePriceJump = 0.11f;
}
