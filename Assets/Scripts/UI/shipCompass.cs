using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipCompass : MonoBehaviour
{
    public GameObject ship;
    public GameObject dogstar;
    public GameObject compass;
    public GameObject model;

    public Vector3 angles;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        angles = ship.transform.rotation.eulerAngles;
        Vector3 newRot = new Vector3(angles.z, angles.y - 90, -angles.x);
        model.transform.localRotation = Quaternion.Euler(newRot);
        compass.transform.LookAt(dogstar.transform.position, Vector3.up);
        compass.transform.Rotate(0, 90, 0);
    }
}
