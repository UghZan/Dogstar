using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindowManager : MonoBehaviour
{
    public static MainWindowManager instance;
    public string[] posters;
    public string[] messages;
    public float averageSpeed;

    public Text fuel;
    public Text integrity;
    public Text voidShield;
    public Button repairButton;
    public Button refillButton;
    public Text mainChat;

    public int repairPrice;
    public int refillPrice;

    float cooldown = 0;
    float timer;
    spaceship sps;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        sps = GetComponentInParent<spaceship>();
    }

    public void Update()
    {
        fuel.text = string.Format("FUEL:{0:F1}/{1:F1}L", sps.fuel, sps.maxFuel);
        integrity.text = string.Format("ITGR:{0:F1}/{1:F1}", sps.integrity, sps.maxIntegrity);
        voidShield.text = string.Format("VDSHL:{0:F1}/{1:F1}", sps.voidShield, sps.maxVoidShield);

        refillButton.interactable = sps.docked && GameManager.instance.money > refillPrice ? true : false;
        repairButton.interactable = sps.docked && GameManager.instance.money > repairPrice ? true : false;

        mainChat.text = ShipUI.instance.messageBuffer;
    }

    public void UpdatePrice()
    {
        refillPrice = Mathf.FloorToInt((sps.maxFuel - sps.fuel) * 5 * GameManager.instance.stations[GameManager.instance.currentStation].refillCoeff);
        repairPrice = Mathf.FloorToInt((sps.maxIntegrity - sps.integrity ) * 5 * GameManager.instance.stations[GameManager.instance.currentStation].refillCoeff);
        refillButton.transform.GetChild(0).GetComponent<Text>().text = string.Format("REFUEL: ${0}", refillPrice);
        repairButton.transform.GetChild(0).GetComponent<Text>().text = string.Format("REPAIR: ${0}", repairPrice);
    }

    public void Refill()
    {
        if (sps.docked)
        {
            if (GameManager.instance.ChangeFunds(-refillPrice, "SYS", string.Format("SHIP_REFUEL OPERATION. TRANSACTION_ID: {0:00000000}", Random.Range(10000000, 99999999))))
            {
                sps.fuel = sps.maxFuel;
                UpdatePrice();
            }
        }
    }
    public void Repair()
    {
        if (sps.docked)
        {
            if(GameManager.instance.ChangeFunds(-repairPrice, "SYS", string.Format("SHIP_REPAIR OPERATION. TRANSACTION_ID: {0:00000000}", Random.Range(10000000, 99999999))))
            {
                sps.integrity = sps.maxIntegrity;
                UpdatePrice();
            }
        }
    }
}
