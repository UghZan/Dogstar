using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    public int size = 2000;
    public int amountOfAsteroids = 100;

    public int mainOre = 0;
    public int rareOre = 0;
    //public int specialOre = 0;

    public float mainChance = 0.25f;
    public float rareChance = 0.1f;
    //public float specialChance = 0.01f;
    public GameObject[] objs;

    public bool init;

    public GameObject player;
    Transform child;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = size;
        GetComponent<RadiationField>().radiationLevel = Random.Range(2,8);
        player = GameManager.instance.player;
        GameManager gm = GameManager.instance;
        child = transform.GetChild(0);
        mainOre += Mathf.CeilToInt(transform.position.z / 30000);
        mainOre = Mathf.Clamp(mainOre, 0, 7);
        for (int i = 0; i < amountOfAsteroids; i++)
        {
            Vector3 pos = Random.insideUnitSphere * size + transform.position;
            GameObject obj;
            obj = Instantiate(gm.rocks[Random.Range(0, gm.rocks.Length)], pos, Random.rotation, child);
            OreRock oreRock = obj.GetComponent<OreRock>();
            float val = Random.value;
            if (val < rareChance)
            {
                oreRock.ore = rareOre + 7;
                
            }
            else if (val < mainChance)
            {
                oreRock.ore = mainOre;
            }

            oreRock.oreRichness = Random.Range(2 + Mathf.Abs(Mathf.CeilToInt(transform.position.z / 10000)), 8 + Mathf.Abs(Mathf.CeilToInt(transform.position.z/10000)));
            oreRock.maxHealth = oreRock.oreRichness * 5;
        }
        child.gameObject.SetActive(false);
        InvokeRepeating("CheckForPlayer", 0, 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckForPlayer()
    {
        if (Vector3.Distance(GameManager.instance.player.transform.position, transform.position) < size*2)
        {
            if (!child.gameObject.activeSelf)
                child.gameObject.SetActive(true);
        }
        else
            if (child.gameObject.activeSelf)
            transform.GetChild(0).gameObject.SetActive(false);
    }
}
