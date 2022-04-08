using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreRock : MonoBehaviour
{
    public float maxHealth = 0;
    [SerializeField]
    public float curHealth;

    public bool infinite = false;
    public bool scanned = false;

    public GameObject destroyEffect;

    public int ore;
    public int oreRichness;
    public int oreLevel;

    private GameObject pickup;

    private void Start()
    {
        oreLevel = ore / 3;
        curHealth = maxHealth;
        pickup = GameManager.instance.pickup;
    }

    public void DrillHit(float amount)
    {
        curHealth -= amount;
        if (curHealth <= 0)
        {
            Mined();
        }
    }

    public void Mined()
    {
        GameObject dE = Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(dE, 8);
        for (int i = 0; i < Mathf.Floor(oreRichness * ShipInteraction.instance.drillGain); i++)
        {
            GameObject go = Instantiate(pickup, transform.position + Random.insideUnitSphere*4, Random.rotation);
        }
        Destroy(gameObject);
    }
}
