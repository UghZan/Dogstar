using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public Item baseItem;
    public int amount;
    public float price;

    public ShopItem(Item _bI, int _am, float _p)
    {
        baseItem = _bI;
        amount = _am;
        price = _p;
    }
    public ShopItem()
    {

    }
}
