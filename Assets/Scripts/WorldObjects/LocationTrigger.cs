using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public string senderName;
    public string locationName;
    public string welcomeMessage;
    public string exitMessage;

    public bool overrideDefault = false;

    public string overrideStringEnter = "";
    public string overrideStringExit = "";

    public bool addsJumpTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (!overrideDefault)
                ShipUI.instance.SendMessageToScreen(senderName, string.Format("Welcome to {0}. {1}.", locationName, welcomeMessage));
            else
                ShipUI.instance.SendMessageToScreen(senderName, string.Format("{0}", overrideStringEnter));

            if (addsJumpTarget)
                GameManager.instance.player.GetComponent<spaceship>().jumpTargets.Add(new JumpTarget(senderName, other.transform.position));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!overrideDefault)
                ShipUI.instance.SendMessageToScreen(senderName, string.Format("Now leaving {0}. {1}.", locationName, exitMessage));
            else
                ShipUI.instance.SendMessageToScreen(senderName, string.Format("{0}", overrideStringExit));
        }
    }
}
