using Assets.Scripts.Entities;
using Assets.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    // Start is called before the first frame update

    public Soundtrack soundtrack;
    public CryWolf cryWolf;

    bool changed;
    private float oldLife;
    void Start()
    {
        oldLife = cryWolf.stats.currentLife;
    }

    // Update is called once per frame
    void Update()
    {
        if(cryWolf.stats.currentLife < oldLife && !changed)
        {
            soundtrack.audioSource.Stop();
            soundtrack.AudioNum++;
            soundtrack.StartSoundtrack();
            changed = true;
        }
    }



}
