using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public int stationID;
    public GameObject station;
    public int spawnRadiusPerStation = 2000;
    public int objectAmountPerStation = 50;
    public float speed = 1;

    public int mainOre = 0;
    public int level = 0;

    public float repairCoeff = 1.0f;
    public float refillCoeff = 1.0f;
    public float[] itemAppearChance;
    public float[] itemPriceModifier;
    [SerializeField]
    public List<ShopItem> shop = new List<ShopItem>();

    public List<Quest> quest = new List<Quest>();

    public float mainChance = 0.25f;
    Transform child;
    // Start is called before the first frame update
    void Start()
    {
        GameManager gm = GameManager.instance;
        child = transform.GetChild(0);
        repairCoeff = Random.Range(0.85f, 1.15f);
        refillCoeff = Random.Range(0.85f, 1.15f);
        SetShopModifiers();
        for (int i = 0; i < objectAmountPerStation; i++)
        {
            Vector3 pos = Random.insideUnitSphere * spawnRadiusPerStation + transform.position;
            GameObject obj = Instantiate(gm.rocks[Random.Range(0, gm.rocks.Length)], pos, Random.rotation, child);
            float val = Random.value;
            OreRock oreRock = obj.GetComponent<OreRock>();
            if (val < mainChance)
            {
                oreRock.ore = mainOre;
            }
            else
            {
                oreRock.ore = 0;
            }
            oreRock.oreLevel = mainOre / 3;
            oreRock.oreRichness = Random.Range(2, 8);
            oreRock.maxHealth = oreRock.oreRichness * 5;
        }
        child.gameObject.SetActive(false);
        InvokeRepeating("CheckForPlayer", 0, 8);
    }

    // Update is called once per frame
    void Update()
    {
        station.transform.Rotate(Vector3.forward, Time.deltaTime * speed);
    }

    void SetShopModifiers()
    {
        int max = ItemDatabase.instance.itemsMax;
        itemAppearChance = new float[max];
        itemPriceModifier = new float[max];
        for (int i = 0; i < max; i++)
        {
            Item it = ItemDatabase.instance.items[i];
            if (it.item_appearInTrade)
            {
                itemAppearChance[i] = (0.75f * Mathf.Sqrt(level + 1) * Random.Range(0.95f, 1.15f)) / ((it.item_rarity + 1) * 0.66f);
                itemAppearChance[i] = Mathf.Clamp(itemAppearChance[i], 0.01f, 0.99f);
                //print(itemAppearChance[i]);
            }
            else
                itemAppearChance[i] = 0;

            itemPriceModifier[i] = Random.Range(1-it.item_basePriceJump, 1 + it.item_basePriceJump) * Mathf.Sqrt(level + 1);

            float val = Random.value;
            if (val < itemAppearChance[i])
            {
                ShopItem si = new ShopItem();
                si.baseItem = it;
                si.price = it.item_base_price * itemPriceModifier[i];
                si.amount = Mathf.FloorToInt(Random.Range(100, 1500) * itemAppearChance[i])/ (it.item_rarity + 1);
                shop.Add(si);
            }
        }
    }

    void CheckForPlayer()
    {
        if (Vector3.Distance(GameManager.instance.player.transform.position, transform.position) < 5000)
        {
            if (!child.gameObject.activeSelf)
                child.gameObject.SetActive(true);
        }
        else
            if (child.gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(false);
    }

    public void UpdateShop()
    {
        foreach(ShopItem si in shop)
        {
            if (Random.value < 0.33f)
            {
                si.amount += Mathf.FloorToInt(Random.Range(-si.amount * 0.11f, +si.amount * 0.11f));
            }
            if (si.amount <= 0)
                shop.Remove(si);
        }
    }

    public void FillQuests()
    {
        int amount = Random.Range(1, level * 4);
        for (int i = 0; i < amount; i++)
        {
            type t = (type)Random.Range(0, 4);
            Quest q = new Quest(t);
            switch(t)
            {
                case type.Fetch:
                    q.neededItemID = Random.Range(0, 21);
                    q.neededItemAmount = Mathf.Clamp((int)(Random.Range(5, 100) / (ItemDatabase.instance.GetItemByID(q.neededItemID).item_rarity) + 1), 1, 100);
                    q.stationOrigin = stationID;
                    q.moneyReward = Mathf.CeilToInt(ItemDatabase.instance.GetItemByID(q.neededItemID).item_base_price * itemPriceModifier[q.neededItemID] * q.neededItemAmount*1.25f);
                    break;
                case type.Deliver:
                    q.neededItemID = Random.Range(0, 21);
                    q.neededItemAmount = Mathf.Clamp((int)(Random.Range(5, 100) / (ItemDatabase.instance.GetItemByID(q.neededItemID).item_rarity) + 1), 1, 100);
                    q.stationOrigin = stationID;
                    int tar = -1;
                    while (tar == q.stationOrigin || tar < 0)
                    {
                        tar = Random.Range(0, GameManager.instance.stations.Length);
                    }
                    q.stationTarget = tar;
                    q.moneyReward = Mathf.CeilToInt(ItemDatabase.instance.GetItemByID(q.neededItemID).item_base_price * itemPriceModifier[q.neededItemID] * q.neededItemAmount * 1.25f);
                    break;
                case type.Survey:
                    q.targetPos = Random.insideUnitSphere * 150000 + new Vector3(0, 0, 100000);
                    q.stationOrigin = stationID;
                    break;
                case type.Find:
                    break;
            }
        }
    }

    public void AddToShop(Item i, int amount = 1)
    {
        ShopItem si_new = new ShopItem(i, amount, i.item_base_price * itemPriceModifier[i.ID]);
        foreach(ShopItem si in shop)
        {
            if(si.baseItem == i)
            {
                si.amount += amount;
                return;
            }
        }
        shop.Add(si_new);
    }

    public float GetPriceMod(Item i)
    {
        int id = i.ID;
        return itemPriceModifier[id];
    }
}
