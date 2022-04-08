using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationLandingZone : MonoBehaviour
{
    public int stationID;
    public bool hasPlayer;

    public GameObject ss;
    public Vector3 offsetVector;

    // Start is called before the first frame update
    void Start()
    {
        ss = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasPlayer)
        {
            ss.GetComponent<spaceship>().SetDock(transform.TransformPoint(transform.localPosition + offsetVector), Quaternion.LookRotation(transform.forward, transform.up), this);
            if(stationID == 0 && GameManager.instance.questProgress < 10)
            {
                GameManager.instance.questProgress = 15;
                ShipUI.instance.StartCoroutine("Briefing");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print("player connected");
            if (Mathf.Abs(other.GetComponent<spaceship>().speed) < 5 )
            {
                print("player docked");
                ss.GetComponent<spaceship>().controlsLocked = true;
                ss.GetComponent<spaceship>().docked = true;
                ss.GetComponent<spaceship>().voidShield = ss.GetComponent<spaceship>().maxVoidShield;
                hasPlayer = true;
                ShipUI.instance.SendMessageToScreen("SYS", "LOCKS ENGAGED.", false);
                GameManager.instance.currentStation = stationID;
                GetComponentInParent<Station>().UpdateShop();
                MainWindowManager.instance.UpdatePrice();
                SaveManager.SaveFile();
            }
            else
            {
                print("speed too great? i guess");
                ShipUI.instance.SendMessageToScreen("SYS", "ATTENTION: YOU ARE TOO FAST TO CONNECT TO THE STATION. PLEASE LOWER YOUR SPEED TO A MAX OF 5 C.U.", false);
            }
        }
    }
}
