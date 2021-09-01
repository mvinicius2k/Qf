using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour


{

    public AudioSource sfx;
    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX()
    {
        sfx.PlayOneShot(clip, 0.8f);
    }
}
