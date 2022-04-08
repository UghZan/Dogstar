using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    public GameObject upg_window;
    public Text descr;
    public Button buy_button;
    public Button equip_button;

    public int[] upgrades;

    public int currently_selected_upgrade;

    private int current_plate_up = 0;
    private int current_engine_up = 0;
    private int current_fuel_up = 0;
    private int current_void_shield_up = 0;
    private int current_maneuver_up = 0;
    private int current_cargo_up = 0;
    private int current_jump_up = 0;
    private int current_drill_up = 0;
    private int current_scan_up = 0;

    public float u_integrity = 0;
    public float u_armor = 0;

    public float u_accel = 0;
    public float u_speedLim1 = 0;
    public float u_speedLim2 = 0;

    public float u_fuel = 0;
    public float u_fuelMul = 0;

    public float u_shield = 0;
    public float u_minVE = 0;
    public float u_absorptionVE = 0;

    public float u_maxCargo = 0;

    public float u_sensitivity = 0;

    public float u_fuelMulJump = 0;
    public float u_maxDistJump = 0;

    public int u_drillLevel = 0;
    public float u_drillPower = 0;
    public float u_drillSpeed = 0;
    public float u_drillGain = 0;

    public float u_scanSpeed = 0;

    public int price;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currently_selected_upgrade)
        {
            case 0:
                descr.text = "Integrity: 200 units\nAbsorption: 10%\nBasic armor plating.";
                break;
            case 1:
                descr.text = "Integrity: 220 units\nAbsorption: 15%\nReinforced armor plating.";
                break;
            case 2:
                descr.text = "Integrity: 250 units\nAbsorption: 20%\nMilitary grade armor plating.";
                break;
            case 3:
                descr.text = "Integrity: 300 units\nAbsorption: 25%\nResearcher special armor plating.";
                break;
            case 4:
                descr.text = "Integrity: 500 units\nAbsorption: 33%\nExperimental armor plating.";
                break;

            case 5:
                descr.text = "Max acceleration: 5 c.u.\nSoft Speed Limit: 100 c.u.\nHard Speed Limit: 150 c.u.\nBasic engine.";
                break;
            case 6:
                descr.text = "Max acceleration: 7 c.u.\nSoft Speed Limit: 125 c.u.\nHard Speed Limit: 175 c.u.\nReinforced engine.";
                break;
            case 7:
                descr.text = "Max acceleration: 10 c.u.\nSoft Speed Limit: 150 c.u.\nHard Speed Limit: 200 c.u.\nMilitary grade engine.";
                break;
            case 8:
                descr.text = "Max acceleration: 20 c.u.\nSoft Speed Limit: 200 c.u.\nHard Speed Limit: 250 c.u.\nResearcher special engine.";
                break;
            case 9:
                descr.text = "Max acceleration: 300 c.u.\nSoft Speed Limit: 250 c.u.\nHard Speed Limit: 300 c.u.\nExperimental engine.";
                break;

            case 10:
                descr.text = "Max fuel: 200 l.\nFuel efficiency: 100%\nBasic fuel system.";
                break;
            case 11:
                descr.text = "Max fuel: 250 l.\nFuel efficiency: 100%\nReinforced fuel system.";
                break;
            case 12:
                descr.text = "Max fuel: 300 l.\nFuel efficiency: 110%\nMilitary grade fuel system.";
                break;
            case 13:
                descr.text = "Max fuel: 400 l.\nFuel efficiency: 120%\nResearcher special fuel system.";
                break;
            case 14:
                descr.text = "Max fuel: 600 l.\nFuel efficiency: 133%\nExperimental fuel system.";
                break;

            case 15:
                descr.text = "Max shield energy: 200 su\nSafe radiation levels: 0 xu/s\nShield absorption: 2%\nBasic void shield.";
                break;
            case 16:
                descr.text = "Max shield energy: 250 su\nSafe radiation levels: 2 xu/s\nShield absorption: 5%\nReinforced void shield.";
                break;
            case 17:
                descr.text = "Max shield energy: 300 su\nSafe radiation levels: 4 xu/s\nShield absorption: 10%\nMilitary grade void shield.";
                break;
            case 18:
                descr.text = "Max shield energy: 400 su\nSafe radiation levels: 8 xu/s\nShield absorption: 16%\nResearcher special void shield.";
                break;
            case 19:
                descr.text = "Max shield energy: 600 su\nSafe radiation levels: 16 xu/s\nShield absorption: 25%\nExperimental void shield.";
                break;

            case 20:
                descr.text = "Maneuver engine power: 100%\nBasic maneuver engines.";
                break;
            case 21:
                descr.text = "Maneuver engine power: 150%\nAdvanced maneuver engines.";
                break;
            case 22:
                descr.text = "Maneuver engine power: 200%\nExperimental maneuver engines.";
                break;

            case 23:
                descr.text = "Max cargo capacity: 50 gkg\nBasic cargo drive.";
                break;
            case 24:
                descr.text = "Max cargo capacity: 75 gkg\nReinforced cargo drive.";
                break;
            case 25:
                descr.text = "Max cargo capacity: 100 gkg\nMilitary grade cargo drive.";
                break;
            case 26:
                descr.text = "Max cargo capacity: 150 gkg\nResearcher special cargo drive.";
                break;
            case 27:
                descr.text = "Max cargo capacity: 250 gkg\nExperimental vortex cargo drive.";
                break;

            case 28:
                descr.text = "Fuel efficiency: 20%\nMax jump distance: 50.000 c.u.\nStandard-issue Alcubierre-Shapiro warp drive.";
                break;
            case 29:
                descr.text = "Fuel efficiency: 50%\nMax jump distance: 100.000 c.u.\nExperimental Alcubierre-Shapiro warp drive with integrated support of extra-long jumps.";
                break;

            case 30:
                descr.text = "Drill tier: Basic\nDrill impact power: 1000 N\nDrill speed: 1 impact/s\nDrill efficiency: 100%\nBasic mining drill.";
                break;
            case 31:
                descr.text = "Drill technology: High-impact\nDrill impact power: 2000 N\nDrill speed: 2 impacts/s\nDrill efficiency: 110%\nReinforced mining drill.";
                break;
            case 32:
                descr.text = "Drill technology: High-impact\nDrill impact power: 2000 N\nDrill speed: 2 impacts/s\nDrill efficiency: 125%\nMilitary grade mining drill.";
                break;
            case 33:
                descr.text = "Drill technology: 4-lens ultra impact\nDrill impact power: 3000 N\nDrill speed: 4 impacts/s\nDrill efficiency: 133%\nResearcher special mining drill.";
                break;
            case 34:
                descr.text = "Drill technology: Antimatter microexplosion\nDrill impact power: 5000 N\nDrill speed: 4 impacts/s\nDrill efficiency: 150%\nExperimental mining drill.";
                break;

            case 35:
                descr.text = "Scanner speed: 100%\nBasic scanning equipment";
                break;
            case 36:
                descr.text = "Scanner speed: 200%\nAdvanced scanning equipment. Equipped with \"Deep Dark Scan\"(c) technology to ensure optimal performance.";
                break;
            case 37:
                descr.text = "Scanner speed: 300%\nExperimental scanning equipment. Using nano-drones equipped with basic warp drives allows fast and thorough examination of any given object.";
                break;
        }
        equip_button.interactable = upgrades[currently_selected_upgrade] == 1 ? true : false;
        buy_button.interactable = (upgrades[currently_selected_upgrade] == 0 && GameManager.instance.money >= price && GameManager.instance.player.GetComponent<spaceship>().docked) ? true : false;
    }

    public void UpdateStats()
    {
        for (int i = 0; i < 38; i++)
        {
            if (upgrades[i] == 2)
            {
                upg_window.transform.GetChild(i).GetComponent<Image>().color = Color.white;
                upgrades[i] = 1;
            }
            if (i == current_plate_up || i == current_engine_up + 5 || i == current_fuel_up + 10 || i == current_void_shield_up + 15 ||
               i == current_maneuver_up + 20 || i == current_cargo_up + 23 || i == current_jump_up + 28 || i == current_drill_up + 30 ||
               i == current_scan_up + 35)
            {
                upg_window.transform.GetChild(i).GetComponent<Image>().color = Color.green;
                upgrades[i] = 2;
            }
            if(upgrades[i] == 0)
                upg_window.transform.GetChild(i).GetComponent<Image>().color = Color.gray;
        }
        switch (current_plate_up)
        {
            case 0:
                u_integrity = 200;
                u_armor = 0.1f;
                break;
            case 1:
                u_integrity = 220;
                u_armor = 0.15f;
                break;
            case 2:
                u_integrity = 250;
                u_armor = 0.2f;
                break;
            case 3:
                u_integrity = 300;
                u_armor = 0.25f;
                break;
            case 4:
                u_integrity = 500;
                u_armor = 0.33f;
                break;
        }
        switch (current_engine_up)
        {
            case 0:
                u_accel = 5;
                u_speedLim1 = 100;
                u_speedLim2 = 150;
                break;
            case 1:
                u_accel = 7;
                u_speedLim1 = 125;
                u_speedLim2 = 175;
                break;
            case 2:
                u_accel = 10;
                u_speedLim1 = 150;
                u_speedLim2 = 200;
                break;
            case 3:
                u_accel = 20;
                u_speedLim1 = 200;
                u_speedLim2 = 250;
                break;
            case 4:
                u_accel = 30;
                u_speedLim1 = 300;
                u_speedLim2 = 350;
                break;
        }
        switch (current_fuel_up)
        {
            case 0:
                u_fuel = 200;
                u_fuelMul = 0.5f;
                break;
            case 1:
                u_fuel = 250;
                u_fuelMul = 0.5f;
                break;
            case 2:
                u_fuel = 300;
                u_fuelMul = 0.45f;
                break;
            case 3:
                u_fuel = 400;
                u_fuelMul = 0.4f;
                break;
            case 4:
                u_fuel = 600;
                u_fuelMul = 0.33f;
                break;
        }
        switch (current_void_shield_up)
        {
            case 0:
                u_shield = 200;
                u_minVE = 0;
                u_absorptionVE = 0.02f;
                break;
            case 1:
                u_shield = 250;
                u_minVE = 2;
                u_absorptionVE = 0.05f;
                break;
            case 2:
                u_shield = 300;
                u_minVE = 4;
                u_absorptionVE = 0.1f;
                break;
            case 3:
                u_shield = 400;
                u_minVE = 8;
                u_absorptionVE = 0.16f;
                break;
            case 4:
                u_shield = 600;
                u_minVE = 16;
                u_absorptionVE = 0.25f;
                break;
        }
        switch (current_maneuver_up)
        {
            case 0:
                u_sensitivity = 1;
                break;
            case 1:
                u_sensitivity = 1.5f;
                break;
            case 2:
                u_sensitivity = 2f;
                break;

        }
        switch (current_cargo_up)
        {
            case 0:
                u_maxCargo = 60;
                break;
            case 1:
                u_maxCargo = 75;
                break;
            case 2:
                u_maxCargo = 100;
                break;
            case 3:
                u_maxCargo = 150;
                break;
            case 4:
                u_maxCargo = 250;
                break;
        }
        switch (current_jump_up)
        {
            case 0:
                u_fuelMulJump = 0.8f;
                u_maxDistJump = 50000;
                break;
            case 1:
                u_fuelMulJump = 0.5f;
                u_maxDistJump = 100000;
                break;
        }
        switch (current_drill_up)
        {
            case 0:
                u_drillLevel = 0;
                u_drillPower = 1;
                u_drillSpeed = 1;
                u_drillGain = 1;
                break;
            case 1:
                u_drillLevel = 1;
                u_drillPower = 2;
                u_drillSpeed = 1;
                u_drillGain = 1.1f;
                break;
            case 2:
                u_drillLevel = 1;
                u_drillPower = 2;
                u_drillSpeed = 2;
                u_drillGain = 1.25f;
                break;
            case 3:
                u_drillLevel = 2;
                u_drillPower = 3;
                u_drillSpeed = 3;
                u_drillGain = 1.33f;
                break;
            case 4:
                u_drillLevel = 3;
                u_drillPower = 5;
                u_drillSpeed = 4;
                u_drillGain = 1.5f;
                break;

        }
        switch (current_scan_up)
        {
            case 0:
                u_scanSpeed = 1;
                break;
            case 1:
                u_scanSpeed = 2;
                break;
            case 2:
                u_scanSpeed = 3;
                break;
        }

        GameManager.instance.player.GetComponent<spaceship>().UpdateStats();
        Inventory.instance.maxCargoWeight = u_maxCargo;
        ShipInteraction.instance.drillGain = u_drillGain;
        ShipInteraction.instance.drillPower = u_drillPower;
        ShipInteraction.instance.drillSpeed = u_drillSpeed;
        ShipInteraction.instance.drillLevel = u_drillLevel;
        ShipInteraction.instance.scanSpeed = u_scanSpeed;
    }

    public void SelectUpgrade(int id)
    {
        currently_selected_upgrade = id;
        if((currently_selected_upgrade >= 0 && currently_selected_upgrade < 5) ||
           (currently_selected_upgrade >= 5 && currently_selected_upgrade < 10) ||
           (currently_selected_upgrade >= 10 && currently_selected_upgrade < 15) ||
           (currently_selected_upgrade >= 15 && currently_selected_upgrade < 20) ||
           (currently_selected_upgrade >= 30 && currently_selected_upgrade < 35))
        {
            price = 1000 * (currently_selected_upgrade % 5);
        }
        else if(currently_selected_upgrade >= 23 && currently_selected_upgrade < 28)
        {
            price = 1000 * (currently_selected_upgrade % 23);
        }
        else if(currently_selected_upgrade >= 20 && currently_selected_upgrade < 23)
        {
            price = 500 * (currently_selected_upgrade % 5);
        }
        else if (currently_selected_upgrade == 28 || currently_selected_upgrade == 29)
        {
            price = 1500 * (currently_selected_upgrade % 27);
        }
        else if (currently_selected_upgrade == 35 || currently_selected_upgrade == 36 || currently_selected_upgrade == 37)
        {
            price = 300 * (currently_selected_upgrade % 5);
        }
    }

    public void BuyUpgrade()
    {
        if (GameManager.instance.ChangeFunds(-price, "SYSTEM", string.Format("BOUGHT UPGRADE ID{0}. TRANSACTION_ID: {1:00000000}", currently_selected_upgrade, Random.Range(10000000, 99999999))))
        {
            upgrades[currently_selected_upgrade] = 1;
            UpdateStats();
        }
    }

    public void EquipUpgrade()
    {
        if (currently_selected_upgrade >= 0 && currently_selected_upgrade < 5)            
        {
            current_plate_up = currently_selected_upgrade % 5;
        }
        else if (currently_selected_upgrade >= 5 && currently_selected_upgrade < 10)
        {
            current_engine_up = currently_selected_upgrade % 5;
        }
        else if (currently_selected_upgrade >= 10 && currently_selected_upgrade < 15)
        {
            current_fuel_up = currently_selected_upgrade % 5;
        }
        else if (currently_selected_upgrade >= 15 && currently_selected_upgrade < 20)
        {
            current_void_shield_up = currently_selected_upgrade % 5;
        }
        else if (currently_selected_upgrade >= 20 && currently_selected_upgrade < 23)
        {
            current_maneuver_up = currently_selected_upgrade % 5;
        }
        else if (currently_selected_upgrade >= 23 && currently_selected_upgrade < 28)
        {
            current_cargo_up = currently_selected_upgrade % 5;
        }
        else if (currently_selected_upgrade == 28 || currently_selected_upgrade == 29)
        {
            current_jump_up = currently_selected_upgrade % 28;
        }
        else if (currently_selected_upgrade >= 30 && currently_selected_upgrade < 35)
        {
            current_drill_up = currently_selected_upgrade % 5;
        }
        else if (currently_selected_upgrade == 35 || currently_selected_upgrade == 36 || currently_selected_upgrade == 37)
        {
            current_scan_up = currently_selected_upgrade % 5;
        }
        
        UpdateStats();
    }
}
