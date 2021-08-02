using Assets.Scripts.Common;
using Assets.Scripts.Entities;
using Assets.Scripts.Templates;
using Assets.Scripts.World;
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
    public void FixedUpdate()
    {
        
    }




    public override void MetaObjects(bool calledOnLive)
    {
        var meta = RegisterReference(Constants.ActionTriggerTag, calledOnLive, typeof(SphereCollider), typeof(ContextAction));
        if(meta != null)
        {
            meta.layer = LayerMask.NameToLayer(Constants.TriggersLayer);
            trigger = meta.GetComponent<SphereCollider>();

        }
        
    }

    
}
