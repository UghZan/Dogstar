using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipInteraction : MonoBehaviour
{
    public static ShipInteraction instance;

    //0 - drill, 1 - scan, 2 - pickup
    public int mode = 0;

    public int drillLevel = 0;
    public float drillPower = 1;
    public float drillSpeed = 1;
    public float drillGain = 1;

    public float scanSpeed = 1;

    public Image crosshair;
    public GameObject drill;
    public GameObject scanner;
    public GameObject drill_hit;
    public GameObject hit_effect;
    //0 - default, 1 - minable, 2 - scannable, 3 - pickupable
    public int ch_mode = 0;
    public Sprite[] ch_modes;

    private Vector3 drill_end_pos;
    private RaycastHit hit;
    private float timer;
    public GameObject pickuper;

    private bool scanInProgress = false;
    private Transform par;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        par = transform.parent;
        drill_end_pos = drill_hit.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (mode == 0)
        {
            if (Physics.Raycast(drill.transform.position, drill.transform.forward, out hit, 6))
            {
                if (hit.collider.GetComponent<OreRock>())
                {
                    ch_mode = 1;
                }
                else
                {
                    ch_mode = 0;
                }
            }
            else
            {
                ch_mode = 0;
            }
        }
        else if(mode == 2)
        {
            if (Physics.Raycast(par.position, par.forward, out hit, 32))
            {
                if (hit.collider.GetComponent<Pickup>())
                {
                    ch_mode = 3;
                }
                else
                {
                    ch_mode = 0;
                }
            }
            else
            {
                ch_mode = 0;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            mode++;
            if (mode > 2)
                mode = 0;
        }
        if(Input.GetMouseButton(0))
        {
            if(mode == 0)
            {
                drill.SetActive(true);
                if (Physics.Raycast(drill.transform.position, drill.transform.forward, out hit, 6))
                {
                    ch_mode = 1;
                    if (timer > 1 / drillSpeed)
                    {
                        //print("drilling something...");
                        if (hit.collider.GetComponent<OreRock>())
                        {
                            hit_effect.SetActive(true);
                            hit_effect.transform.position = hit.point;
                            if (drillLevel >= hit.collider.GetComponent<OreRock>().oreLevel)
                                hit.collider.GetComponent<OreRock>().DrillHit(drillPower);
                            else
                                ShipUI.instance.SendMessageToScreen("SYS", "ATTENTION! DRILL IS TOO WEAK FOR THIS MATERIAL!", false);
                        }
                        timer = 0;
                    }
                }
                else
                {
                    hit_effect.SetActive(false);
                    ch_mode = 0;
                    drill_hit.transform.localPosition = drill_end_pos;
                }
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            if (mode == 1)
            {
                if (Physics.Raycast(drill.transform.position, drill.transform.forward, out hit, 32) && !scanInProgress)
                {
                    if (hit.collider.GetComponent<OreRock>())
                    {
                        StartCoroutine(ScanProcedure(hit.transform.gameObject, 1));
                    }
                    else if(hit.collider.GetComponent<Artifact>())
                    {
                        StartCoroutine(ScanProcedure(hit.transform.gameObject, 2));
                    }
                    else
                    {
                        StartCoroutine(ScanProcedure(hit.transform.gameObject, 0));
                    }
                    scanInProgress = true;
                }
            }
            else if (mode == 2)
            {
                if (Physics.Raycast(par.position, par.forward, out hit, 32))
                {
                    if (hit.transform.gameObject.GetComponent<Pickup>())
                    {
                        hit.transform.gameObject.GetComponent<Pickup>().pos = pickuper;
                        hit.transform.gameObject.GetComponent<Pickup>().flying = true;
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            hit_effect.SetActive(false);
            if (mode == 0)
            {
                drill.SetActive(false);
            }
            else if (mode == 1)
            {

            }
        }

        crosshair.sprite = ch_modes[ch_mode];
    }

    IEnumerator ScanProcedure(GameObject go, int mode)
    {
        ShipUI.instance.SendMessageToScreen("SYS", "SCAN STARTED");
        yield return new WaitForSeconds(3);
        switch(mode)
        {
            case 0:
                ShipUI.instance.SendMessageToScreen("SYS", "SCAN FAILED: NO SCAN ANCHORS");
                break;
            case 1:
                ShipUI.instance.SendMessageToScreen("SYS", "SCAN ANCHOR FOUND: MINERAL_DEPOSIT...");
                yield return new WaitForSeconds(10 / scanSpeed);
                ShipUI.instance.SendMessageToScreen("SYS", "SCAN FINISHED.");
                int ore = go.GetComponent<OreRock>().ore;
                if(ore == 0)
                    ShipUI.instance.SendMessageToScreen("SYS", string.Format("NO MAJOR MINERAL TRACES FOUND."));
                else
                    ShipUI.instance.SendMessageToScreen("SYS", string.Format("TRACES OF {0} FOUND", ShipUI.instance.OreNumToString(ore)));
                go.GetComponent<OreRock>().scanned = true;
                break;
            case 2:
                ShipUI.instance.SendMessageToScreen("SYS", "SCAN ANCHOR FOUND: ALIEN_LANGUAGE...");
                yield return new WaitForSeconds(15 / scanSpeed);
                ShipUI.instance.SendMessageToScreen("SYS", "SCAN FINISHED. A NEW VERSE FOUND.");
                GameManager.instance.LearnVerse();
                go.GetComponent<Artifact>().Found();
                break;
        }
        scanInProgress = false;
        yield return null;
    }

}
