using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeManager : MonoBehaviour
{
    public static TradeManager instance;
    public GameObject shop_object;
    public GameObject item_icon;
    public Text item_info;
    public Button buyButton;

    public int currentIndex;
    public spaceship sps;
    public Station currentStation;
    List<ShopItem> shop;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        sps = GameManager.instance.player.GetComponent<spaceship>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentIndex != -1 && shop != null)
            buyButton.interactable = GameManager.instance.money >= shop[currentIndex].price && sps.docked ? true : false;
    }

    public void SetStation()
    {
        currentStation = GameManager.instance.stations[GameManager.instance.currentStation];
        shop = currentStation.shop;
    }

    public void SetItemInfo()
    {
        ShopItem si = shop[currentIndex];
        Item item = si.baseItem;
        item_info.text = item.item_name + "\n" + "//" + item.item_desc + "//\n" + "WEIGHT: " + item.item_weight + "\nPRICE: " + (int)si.price;
    }

    public void BuyItem(int id)
    {
        if (id == -1)
            id = currentIndex;
        if (shop[id] == null)
            return;
        ShopItem si = shop[id];
        if (GameManager.instance.ChangeFunds(-(int)si.price, "SYSTEM", string.Format("BOUGHT ITEM \"{0}\". TRANSACTION_ID: {1:00000000}", shop[id].price, Random.Range(10000000, 99999999))))
        {
            Inventory.instance.AddItem(si.baseItem);
            si.amount--;
            if (si.amount <= 0)
                shop.Remove(si);
            currentIndex = -1;
            UpdateShopWindow();
        }
    }

    public void UpdateShopWindow()
    {
        int i = 0;
        item_info.text = "";
        foreach (Transform t in shop_object.transform)
            Destroy(t.gameObject);
        foreach (ShopItem ii in shop)
        {
            GameObject icon = Instantiate(item_icon, shop_object.transform);
            icon.GetComponent<ItemIcon>().isShop = true;
            icon.GetComponent<ItemIcon>().keptItem = ii.baseItem.ID;
            icon.GetComponent<ItemIcon>().index = i;
            icon.transform.GetChild(2).GetComponent<Text>().text = ii.amount.ToString();
            icon.transform.GetChild(0).GetComponent<Image>().sprite = ii.baseItem.item_sprite;
            i++;
        }
    }
}
