using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBeacon : MonoBehaviour
{
    public string beacon_name;
    public bool scanned;
    // Start is called before the first frame update
    void Start()
    {
        beacon_name = GenerateName();

    }

    // Update is called once per frame
    void Update()
    {
    }

    string GenerateName()
    {
        string[] first = GameManager.instance.firstSyl;
        string[] second = GameManager.instance.secondSyl;
        string[] third = GameManager.instance.thirdSyl;

        string name = first[Random.Range(0, first.Length)] + "\'"+ second[Random.Range(0, second.Length)] + " " + third[Random.Range(0, third.Length)];
        return name;
    }
}
