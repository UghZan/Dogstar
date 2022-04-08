using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public Item item;

    public InventoryItem(Item _item)
    {
        item = _item;
    }

}
