using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public Item[] items;
    public int itemsMax;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        items = Resources.LoadAll<Item>("Items/");
        itemsMax = items.Length;
    }

    public Item GetItemByID(int id)
    {
        foreach (Item item in items)
        {
            if (item.ID == id)
                return item;
        }
        return null;
    }
}
