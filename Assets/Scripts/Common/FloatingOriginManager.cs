using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingOriginManager : MonoBehaviour
{
    public float pointMargin = 8000f;

    public GameObject player;
    public GameObject universe;
    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!player.GetComponent<spaceship>().isJumping)
        {
            if (player.transform.position.magnitude > pointMargin)
            {
                print("TRANSLATING");
                universe.transform.position -= player.transform.position;
                player.transform.position = Vector3.zero;
            }
        }
    }
}
