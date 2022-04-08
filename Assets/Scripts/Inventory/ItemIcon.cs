using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemIcon : MonoBehaviour, IPointerClickHandler
{
    public bool isShop;
    public int keptItem;
    public int index;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!isShop)
        {
            Inventory.instance.currentIndex = index;
            Inventory.instance.SetItemInfo(keptItem);
        }
        else
        {
            TradeManager.instance.currentIndex = index;
            TradeManager.instance.SetItemInfo();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
