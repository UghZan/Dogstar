using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipUI : MonoBehaviour
{
    public static ShipUI instance;

    public AudioSource messA;

    public Text speed;
    public Text accel;
    public Text torque;
    public Text rX;
    public Text rY;
    public Text rZ;

    public Text fuel;
    public Text integrity;
    public Text voidShield;
    public Text darkEnergy;

    public Text mode;
    public Text money;
    public Text time;

    public Text name_loc;
    public Text dist_loc;

    public TMP_Text messageScreen;

    public string messageBuffer;

    public spaceship ship;
    public ShipInteraction si;

    [SerializeField]
    private bool init = false;
    private RaycastHit hit;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<spaceship>();
        if (!init)
            StartCoroutine("InitIntercom");
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider.GetComponent<ScannerTarget>())
            {
                name_loc.text = hit.collider.GetComponent<ScannerTarget>().target_name;
                if (hit.collider.GetComponent<OreRock>() && hit.collider.GetComponent<OreRock>().scanned)
                {
                    dist_loc.text = hit.distance.ToString("F1") + "\n" + hit.collider.GetComponent<OreRock>().curHealth + "/" + hit.collider.GetComponent<OreRock>().maxHealth;
                }
                else
                {
                    dist_loc.text = hit.distance.ToString("F1");
                }
            }
        }
        else
        {

            name_loc.text = "";
            dist_loc.text = "";
        }
        if (ship.jumpOn == 0)
        {
            if (!ship.speedLimit1_reached)
            {
                speed.color = Color.green;
                speed.text = string.Format("SPD:{0:000} c.u.", ship.speed);
            }
            if (ship.speedLimit1_reached)
            {
                speed.color = Color.yellow;
                speed.text = string.Format("!SPD:{0:000} c.u.!", ship.speed);
            }
            if (ship.speedLimit2_reached)
            {
                speed.color = Color.red;
                speed.text = string.Format("!!!SPD:{0:000} c.u.!!!", ship.speed);
            }
            torque.text = "TORQUE";
            accel.text = string.Format("ACCL:{0:000} c.u.", ship.accel);
            rX.text = string.Format("{0:000}", ship.rotation.x * 1000);
            rY.text = string.Format("{0:000}", ship.rotation.y * 1000);
            rZ.text = string.Format("{0:000}", ship.rotation.z * 1000);
        }
        else if (ship.jumpOn == 1)
        {
            float j_fuel = (ship.jumpDistance / ship.jumpMaxDistance) * ship.maxFuel * ship.jumpFuelMultipler;
            speed.text = string.Format("MAX: {0:000.0}k c.u.", ship.jumpMaxDistance/1000);
            accel.text = string.Format("DST: {0:000.0}k c.u.", ship.jumpDistance/1000);
            torque.text = string.Format("FUEL: {0:000}%", (j_fuel/ship.fuel) * 100);
            rX.text = "";
            rY.text = "";
            rZ.text = "";
        }
        else
        {
            float j_fuel = Vector3.Distance(ship.transform.position, ship.jumpTargets[ship.currentTarget].position) * ship.jumpFuelMultipler / 10f;
            speed.text = string.Format("TRGT: {0}", ship.jumpTargets[ship.currentTarget].target_name);
            accel.text = string.Format("DST: {0:000.0}k c.u.", ship.jumpDistance / 1000);
            torque.text = string.Format("FUEL: {0:000}%", (j_fuel / ship.fuel) * 100);
            rX.text = "";
            rY.text = "";
            rZ.text = "";
        }

        fuel.text = string.Format("FUEL:{0}", GetBarProgress(ship.fuel, ship.maxFuel));
        if (ship.fuel / ship.maxFuel <= 0.25f)
            fuel.color = Color.red;
        else if (ship.fuel / ship.maxFuel <= 0.5f)
            fuel.color = Color.yellow;
        else
            fuel.color = Color.green;

        integrity.text = string.Format("INTG:{0}", GetBarProgress(ship.integrity, ship.maxIntegrity));
        if (ship.integrity / ship.maxIntegrity <= 0.25f)
            integrity.color = Color.red;
        else if (ship.integrity / ship.maxIntegrity <= 0.5f)
            integrity.color = Color.yellow;
        else
            integrity.color = Color.green;
        voidShield.text = string.Format("VDSH:{0}", GetBarProgress(ship.voidShield, ship.maxVoidShield));
        if (ship.voidShield / ship.maxVoidShield <= 0.25f)
            voidShield.color = Color.red;
        else if (ship.voidShield / ship.maxVoidShield <= 0.5f)
            voidShield.color = Color.yellow;
        else
            voidShield.color = Color.green;
        darkEnergy.text = string.Format("DKEN:{0:00.00} xu/s", GetRadiationWithError(ship.currentDarkEnergyLevel));

        if (init)
            messageScreen.text = messageBuffer;

        money.text = string.Format("MONEY:${0}", GameManager.instance.money);
        time.text = string.Format("TIME:{0}", GameManager.instance.getCurrentTimeOnly());

        switch (si.mode)
        {
            case 0:
                mode.text = "MODE:DRILL";
                break;
            case 1:
                mode.text = "MODE:SCANNER";
                break;
            case 2:
                mode.text = "MODE:ATTRACT";
                break;
        }
    }

    string GetBarProgress(float cur, float max)
    {
        float prog = cur / max;
        if (prog <= 0.01)
            return "[----------]";
        else if (prog < 0.1)
            return "[|---------]";
        else if (prog < 0.2)
            return "[||--------]";
        else if (prog < 0.3)
            return "[|||-------]";
        else if (prog < 0.4)
            return "[||||------]";
        else if (prog < 0.5)
            return "[|||||-----]";
        else if (prog < 0.6)
            return "[||||||----]";
        else if (prog < 0.7)
            return "[|||||||---]";
        else if (prog < 0.8)
            return "[||||||||--]";
        else if (prog < 0.9)
            return "[|||||||||-]";
        else
            return "[||||||||||]";
    }

    float GetRadiationWithError(float value)
    {
        float valRand = value + Random.Range(-value * 0.25f, value * 0.25f);
        valRand = Mathf.Clamp(valRand, 0, valRand);
        return valRand;
    }

    public void SendMessageToScreen(string name, string message, bool time = true, bool showName = true)
    {
        messA.Play();
        string mess = "";
        if (time)
            mess = "[" + GameManager.instance.getCurrentTimeFormatted() + "]";
        if (showName)
            mess += name + ": ";
        mess += message;
        messageBuffer += mess + "\n";
    }

    public void ClearBuffer()
    {
        messageBuffer = "";
    }

    IEnumerator InitIntercom()
    {
        yield return new WaitForSeconds(3);
        messageScreen.alignment = TextAlignmentOptions.Center;
        messageScreen.text = "SYS: WELCOME TO LUPUS MAJOR VOID. CURRENT INTERGALACTIC TIME IS: " + GameManager.instance.getCurrentTimeFormatted();
        yield return new WaitForSeconds(3);
        messageScreen.text = "";
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 10; i++)
        {
            messageScreen.text = "SYS: CONNECTING TO LOCAL COMMS... \\";
            ClearBuffer();
            yield return new WaitForSeconds(0.1f);
            messageScreen.text = "SYS: CONNECTING TO LOCAL COMMS... |";
            ClearBuffer();
            yield return new WaitForSeconds(0.1f);
            messageScreen.text = "SYS: CONNECTING TO LOCAL COMMS... /";
            ClearBuffer();
            yield return new WaitForSeconds(0.1f);
            messageScreen.text = "SYS: CONNECTING TO LOCAL COMMS... -";
            ClearBuffer();
            yield return new WaitForSeconds(0.1f);
        }
        messageScreen.text = "SYS: CONNECTION SUCCESFULL.";
        yield return new WaitForSeconds(3f);
        if (GameManager.instance.questProgress == 0)
        {
            messageScreen.text = "@%(D13O_@GST*A!R_@W1A1KE7S(";
            yield return new WaitForSeconds(0.6f);
            messageScreen.text = "";
            yield return new WaitForSeconds(2);
            messA.Play();
            messageScreen.text = "GBD: WELCOME";
            yield return new WaitForSeconds(3f);
            messA.Play();
            messageScreen.text = "GBD: I AM THE LEAD RESEARCHER AT LUPUS STATION";
            yield return new WaitForSeconds(3.1f);
            messA.Play();
            messageScreen.text = "GBD: IT IS PLEASURE TO SEE YOU";
            yield return new WaitForSeconds(2.8f);
            messA.Play();
            messageScreen.text = "GBD: HEALTHY AND WHOLE";
            yield return new WaitForSeconds(2.3f);
            messA.Play();
            messageScreen.text = "GBD: PLEASE COME IN";
            yield return new WaitForSeconds(4f);
            messA.Play();
            messageScreen.text = "GBD: WE WILL DISCUSS YOUR TASKS";
            yield return new WaitForSeconds(2.5f);
            messageScreen.text = "";
            GameManager.instance.questProgress = 5;
        }
        init = true;
        messageScreen.alignment = TextAlignmentOptions.Bottom;
        yield return null;
    }

    IEnumerator Briefing()
    {
        SendMessageToScreen("GBD", "WELCOME TO OUR STATION");
        yield return new WaitForSeconds(3f);
        SendMessageToScreen("GBD", "AS YOU ALREADY KNOW");
        yield return new WaitForSeconds(6f);
        SendMessageToScreen("GBD", "YOU ARE HERE TO HELP US WITH AN IMPORTANT RESEARCH");
        yield return new WaitForSeconds(4f);
        SendMessageToScreen("GBD", "IT IS A VERY IMPORTANT TASK");
        yield return new WaitForSeconds(5f);
        SendMessageToScreen("GBD", "YOU NEED TO FIND 12 ARTIFACTS AROUND THE DARK STAR");
        yield return new WaitForSeconds(5f);
        SendMessageToScreen("GBD", "THEY ARE PLACED AROUND IT AT ~300.000 C.U. DISTANCE");
        yield return new WaitForSeconds(5f);
        SendMessageToScreen("GBD", "BE WARY AS STAR EMANATES A VERY POTENT DARK ENERGY FIELD");
        yield return new WaitForSeconds(4f);
        SendMessageToScreen("GBD", "SO YOU'LL NEED A GOOD VOID SHIELD");
        yield return new WaitForSeconds(3f);
        SendMessageToScreen("GBD", "GOOD LUCK");
        yield return new WaitForSeconds(4f);
        string messageTemp = messageBuffer;
        SendMessageToScreen("GBD", "DOG STAR AWAITS");
        yield return new WaitForSeconds(0.7f);
        messageBuffer = messageTemp;
        yield return null;
    }

    public string OreNumToString(int num)
    {
        switch (num)
        {
            case 0:
                return "ROCK";
            case 1:
                return "CARBON";
            case 2:
                return "FERRITE";
            case 3:
                return "TITANIUM";
            case 4:
                return "THORIUM";
            case 5:
                return "UMBRIUM";
            case 6:
                return "RHODITE";
            case 7:
                return "IRIDIUM";
            case 8:
                return "PETROPHAGI";
            case 9:
                return "VOIDEN";
        }
        return "UNKNOWN";
    }
}
