using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour
{
    public int pattern;
    public Color color;
    public bool found;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/alien_artifact_" + pattern);
        transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Found()
    {
        GameManager.instance.ChangeFunds(Random.Range(1000, 2000) * (GameManager.instance.current_verse+1), "GB", "SPLENDIDLY DONE. HERE\'S YOUR REWARD.");
        found = true;
        StartCoroutine("Fade");
    }

    IEnumerator Fade()
    {
        float spent = 0;
        Color c;
        while(spent < 5)
        { 
            c = Color.Lerp(color, Color.black, spent/5);
            transform.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", c);
            spent += Time.deltaTime;
            yield return null;
        }
        c = Color.black;
        yield return null;
    }
}
