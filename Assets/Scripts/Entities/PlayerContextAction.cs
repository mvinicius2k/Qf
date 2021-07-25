using Assets.Scripts.Common;
using Assets.Scripts.Templates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContextAction : MonoScript, IMonoScript
{
    public SphereCollider trigger;

    public PlayerContextAction()
    {
    }
  

    public void Awake()
    {

        
    }

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    

    public override void make()
    {
        Debug.Log("Make");
        var meta = RegisterReference(Constants.ActionTriggerTag, typeof(SphereCollider));
        trigger = meta.GetComponent<SphereCollider>();
    }

    
}