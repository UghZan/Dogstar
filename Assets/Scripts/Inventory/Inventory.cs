using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> inv;
    public GameObject screen;
    // 0 - inventory, 1 - research, 2 - shop, 3 - quests, 4 - ship
    private int mode;
    private Animator anim;

    public Button shopButton;
    public Button tasksButton;

    public bool openInv = true;
    public float maxCargoWeight = 100;
    public float curCargoWeight = 0;

    public int currentIndex = -1;

    [Header("Inventory Stuff")]
    public GameObject inv_window;
    public GameObject items_object;
    public GameObject item_icon;
    public Text item_info;
    public Text cargo_weight;
    public Button discardButton;
    public Button sellButton;

    [Header("Research Stuff")]
    public GameObject res_window;
    public Text verses;

    [Header("Upgrade Stuff")]
    public GameObject upg_window;

    [Header("Main Window")]
    public GameObject main_window;

    [Header("Trade Window")]
    public GameObject trade_window;

    private spaceship sp_plr;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        inv = new List<InventoryItem>();
        /*for (int i = 0; i < 12; i++)
        {
            AddItem(i);
        }*/
        sp_plr = GameManager.instance.player.GetComponent<spaceship>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetBool("open", openInv);
            openInv = !openInv;
            sp_plr.rotLocked = !openInv;
        }
        cargo_weight.text = "CARGO: " + curCargoWeight + "/" + maxCargoWeight + " kg";

        discardButton.interactable = currentIndex > -1 ? true : false;
        sellButton.interactable = sp_plr.docked ? true : false;

        shopButton.interactable = sp_plr.docked ? true : false;

    }

    public bool AddItem(Item item)
    {
        if (curCargoWeight + item.item_weight > maxCargoWeight)
            return false;
        inv.Add(new InventoryItem(item));
        curCargoWeight += item.item_weight;

        return true;
    }

    public bool AddItem(int id)
    {
        Item item = ItemDatabase.instance.GetItemByID(id);
        if (curCargoWeight + item.item_weight > maxCargoWeight)
            return false;
        inv.Add(new InventoryItem(item));
        curCargoWeight += item.item_weight;
        return true;
    }

    public void DropItem(int id)
    {
        if (id == -1)
            id = currentIndex;
        if (inv[id] == null)
            return;
        GameObject pickup = Instantiate(GameManager.instance.pickup, ShipInteraction.instance.pickuper.transform.position - transform.up * 5, Quaternion.identity, GameManager.instance.universe.transform);
        pickup.GetComponent<Pickup>().itemID = inv[id].item.ID;
        curCargoWeight -= inv[id].item.item_weight;
        inv.RemoveAt(id);
        currentIndex = -1;
        UpdateInventory();
    }

    public void SellItem(int id)
    {
        if (id == -1)
            id = currentIndex;
        if (inv[id] == null)
            return;
        curCargoWeight -= inv[id].item.item_weight;
        int price = Mathf.RoundToInt(inv[id].item.item_base_price * TradeManager.instance.currentStation.GetPriceMod(inv[id].item) * 0.95f);
        GameManager.instance.ChangeFunds(price, "SYSTEM", string.Format("SOLD ITEM \"{0}\". TRANSACTION_ID: {1:00000000}", inv[id].item.item_name, Random.Range(10000000, 99999999)));
        GameManager.instance.stations[GameManager.instance.currentStation].AddToShop(inv[id].item);
        inv.RemoveAt(id);
        currentIndex = -1;
        UpdateInventory();
        TradeManager.instance.UpdateShopWindow();
    }


    public void UpdateInventory()
    {
        int i = 0;
        item_info.text = "";
        foreach (Transform t in items_object.transform)
            Destroy(t.gameObject);
        foreach(InventoryItem ii in inv)
        {
            GameObject icon = Instantiate(item_icon, items_object.transform);
            icon.GetComponent<ItemIcon>().keptItem = ii.item.ID;
            icon.GetComponent<ItemIcon>().index = i;
            icon.transform.GetChild(1).GetComponent<Image>().sprite = ii.item.item_sprite;
            i++;
        }
    }

    public void UpdateVerses()
    {
        verses.text = "";
        for (int i = 0; i < 12; i++)
        {
            if (GameManager.instance.artifacts_found[i])
                verses.text += GameManager.instance.verses[i] + "\n";
            else
                verses.text += "????????????\n";
        }
    }

    public void SetItemInfo(int id)
    {
        Item item = ItemDatabase.instance.GetItemByID(id);
        if(!sp_plr.docked)
            item_info.text = item.item_name + "\n" + "//" + item.item_desc + "//\n" + "WEIGHT: " + item.item_weight + "\nPRICE: " + item.item_base_price;
        else
            item_info.text = item.item_name + "\n" + "//" + item.item_desc + "//\n" + "WEIGHT: " + item.item_weight + "\nPRICE: " + item.item_base_price * TradeManager.instance.currentStation.GetPriceMod(inv[id].item);
    }

    public void SwitchPage(int page)
    {
        switch(page)
        {
            case 0:
                main_window.SetActive(true);
                trade_window.SetActive(false);
                inv_window.SetActive(false);
                res_window.SetActive(false);
                upg_window.SetActive(false);
                break;
            case 1:
                TradeManager.instance.SetStation();
                TradeManager.instance.UpdateShopWindow();
                main_window.SetActive(false);
                trade_window.SetActive(true);
                inv_window.SetActive(false);
                res_window.SetActive(false);
                upg_window.SetActive(false);
                break;
            case 2:
                UpdateInventory();
                main_window.SetActive(false);
                trade_window.SetActive(false);
                inv_window.SetActive(true);
                res_window.SetActive(false);
                upg_window.SetActive(false);
                break;
            case 3:
                UpdateVerses();
                main_window.SetActive(false);
                trade_window.SetActive(false);
                inv_window.SetActive(false);
                res_window.SetActive(true);
                upg_window.SetActive(false);
                break;
            case 4:
                main_window.SetActive(false);
                trade_window.SetActive(false);
                inv_window.SetActive(false);
                res_window.SetActive(false);
                upg_window.SetActive(true);
                break;
        }
    }

    public void HideInvScreen()
    {
        screen.SetActive(false);
    }

    public void ShowInvScreen()
    {
        screen.SetActive(true);
        UpdateInventory();
        UpdateVerses();
    }

    public int[] GetIDArray()
    {
        int[] arr = new int[inv.Capacity];
        for (int i = 0; i < inv.Capacity; i++)
        {
            arr[i] = inv[i].item.ID;
        }
        return arr;
    }
}
