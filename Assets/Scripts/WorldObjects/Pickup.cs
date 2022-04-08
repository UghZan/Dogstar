using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int itemID;
    public bool flying;

    public GameObject pos;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (flying)
            transform.position = Vector3.MoveTowards(transform.position, pos.transform.position, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup")
        {
            if (itemID >= 0)
            {
                Inventory.instance.AddItem(itemID);
            }
            Destroy(gameObject);
        }
    }
}
