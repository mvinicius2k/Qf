using Assets.Scripts.Common;
using Assets.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class OpenGateDoor : MonoScript, IContextAction
{
    public const string VarOpen = "open";
    public uint indexLight = 2;

    public Collider trigger;
    public Animator animDoors;
    public bool active = true;
    public MeshRenderer meshSignal;
    public Material signalActived;
    public Material signalInactived;

    private bool _openned = false;
    public bool Openned
    {
        get => _openned;
        set
        {
            _openned = value;
            UpdateMaterial();
        }
    }

    public void Action()
    {
        if (!active)
            return;
        Openned = !Openned;

        if(animDoors != null)
        {
            animDoors.SetBool(VarOpen, Openned);

        } 
        else
        {
            Debug.LogWarning($"{nameof(animDoors)} nulo");
        }
        

        
    }

    public void UpdateMaterial()
    {
        if (meshSignal != null)
        {
            if (active)
            {

                meshSignal.materials[indexLight].CopyPropertiesFromMaterial(signalActived);
            }
            else
            {
                meshSignal.materials[indexLight].CopyPropertiesFromMaterial(signalInactived);
            }

        }
    }

    private void Start()
    {
        UpdateMaterial();
    }

    private void FixedUpdate()
    {
        
    }

    public override void MetaObjects(bool calledOnLive)
    {

        var gameObj = RegisterReference(Constants.ActionTriggerTag, calledOnLive, typeof(SphereCollider));
        if (gameObj == null)
            return;
        gameObj.layer = LayerMask.NameToLayer(Constants.TriggersLayer);
        
        trigger = gameObj.GetComponent<SphereCollider>();
        trigger.isTrigger = true;
        
    }


    
}
