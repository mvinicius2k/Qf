using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using Assets.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateDoor : MonoScript, IContextAction
{
    public const string VarOpen = "open";
    public int indexLight = 2;

    public Collider trigger;
    public Animator animDoors;
    public bool _active = true;
    public bool Active
    {
        get => _active;
        set
        {
            _active = value;
            UpdateMaterial();
        }
    }
    public MeshRenderer meshSignal;
    public Material signalActived;
    public Material signalInactived;

    public bool _openned = false;
    public bool Openned
    {
        get => _openned;
        set
        {
            _openned = value;
            UpdateMotion();
        }
    }

    

    public void Action()
    {
        if (!Active)
            return;
        Openned = !Openned;

        UpdateMotion();

    }

    
    private void UpdateMotion()
    {
        
        if (animDoors != null)
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
            if(indexLight >= meshSignal?.sharedMaterials.Length)
            {
                Debug.LogWarning($"{nameof(indexLight)} ({indexLight}) precisa ser menor que {meshSignal?.sharedMaterials.Length}");
                return;
            }

            if (Active && signalActived != null)
            {
                meshSignal.SetSharedMaterial(indexLight, signalActived);
                //meshSignal.sharedMaterials[indexLight]?.CopyPropertiesFromMaterial(signalActived);

            }
            else if(signalInactived != null)
            {
                meshSignal.SetSharedMaterial(indexLight, signalInactived);
                //meshSignal.sharedMaterials[indexLight]?.CopyPropertiesFromMaterial(signalInactived);
            }
            else
            {
                Debug.LogWarning($"{nameof(signalInactived)} nulo");
            }


        }
    }

    private void Start()
    {
        UpdateMaterial();
    }
    private void Awake()
    {
        if(animDoors != null)
        {
            UpdateMotion();
        }
    }

    private new void OnValidate()
    {
        base.OnValidate();
        UpdateMaterial();
        //UpdateMotion();
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
