using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] tracks;
    public int currentTrack;
    public AudioSource ass;
    public float timeBetween;

    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timeBetween = Random.Range(10, 30);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ass.isPlaying)
        {
            currentTrack = Random.Range(0, tracks.Length);
            timer += Time.deltaTime;
            if (timer >= timeBetween)
            {
                ass.clip = tracks[currentTrack];
                timeBetween = Random.Range(10, 30);
            }
        }
    }
}
