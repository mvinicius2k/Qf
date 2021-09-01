using Assets.Scripts.Common;
using Assets.Scripts.Utils;
using Assets.Scripts.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDoor  : MonoBehaviour, IContextAction
{
    public const string VarOpen = "open";
    public int indexLight = 2;

    public Animator animDoors;
    public bool _active = true;
    public bool _passowordOk = true;

    public AudioSource audioSource;
    public AudioClip audioActive;

    private bool mute = false;
    public bool Active
    {
        get => _active;
        set
        {
            _active = value;
            UpdateMaterial();
        }
    }

    public bool PassawordOk { 
        get => _passowordOk;
        set 
        {
            _passowordOk = value;
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
        if (!Active || !PassawordOk)
            return;
        Openned = !Openned;

        UpdateMotion();

    }

    public void ForceOpen()
    {
        _openned = true;
        UpdateMotion();
    }

    
    private void UpdateMotion()
    {
        
        if (animDoors != null)
        {
            if(!mute)
                audioSource.PlayOneShot(audioActive);
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

            if (Active && PassawordOk && signalActived != null)
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
        if(animDoors != null)
        {
            mute = true;
            UpdateMotion();
            mute = false;
        }
        UpdateMaterial();
    }
    private void Awake()
    {
    }

    private void OnValidate()
    {
        UpdateMaterial();
    }
    


    
}
