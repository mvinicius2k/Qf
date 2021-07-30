using Assets.Scripts.Common;
using Assets.Scripts.Templates;
using Assets.Scripts.World;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContextAction : MonoScript, IMonoScript
{

    private class ContextAction : MonoBehaviour
    {
        private Collider currentTrigger;
        public Collider CurrentTrigger { get => currentTrigger; }

        private void Update()
        {
            if(currentTrigger != null)
            {
                if (Input.GetButtonDown(Constants.InputContextAction))
                {
                    var trigger = currentTrigger.gameObject.GetComponentInParent(typeof(IContextAction));
                    if(trigger != null)
                    { 
                        var triggerInstance = (IContextAction)trigger;
                        triggerInstance.Action();
                    }
                }

            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ActionTriggerTag))
            {
                Debug.Log("Pressione E");
                currentTrigger = other;
                


            }


        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Saiu");
            if (other.GetInstanceID() == currentTrigger.GetInstanceID())
            {
                currentTrigger = null;
            }
        }
    }

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
        meta.layer = LayerMask.NameToLayer(Constants.TriggersLayer);
        trigger = meta.GetComponent<SphereCollider>();
        
    }

    
}
