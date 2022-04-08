using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadiationField : MonoBehaviour
{
    public float radiationLevel;

    private SphereCollider col;

    //should radiation use distance from center or just add/substract radiation level
    public bool stable;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (stable)
            if (other.tag == "Player")
                other.GetComponent<spaceship>().currentDarkEnergyLevel += radiationLevel;
    }

    private void OnTriggerStay(Collider other)
    {
        if(!stable)
        if (other.tag == "Player")
            other.GetComponent<spaceship>().currentDarkEnergyLevel += radiationLevel * (col.radius/Vector3.Distance(other.transform.position, col.center));
    }

    private void OnTriggerExit(Collider other)
    {
        if (stable)
            if (other.tag == "Player")
                other.GetComponent<spaceship>().currentDarkEnergyLevel -= radiationLevel;
    }
}
