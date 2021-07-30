using System;
using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Common;

namespace Assets.Scripts.World
{
    [Obsolete]
    public class ActionTrigger : MonoBehaviour
    {

        public SphereCollider trigger;
        
        private GameObject meta;

        public void InitMetaObjects()
        {
            if(meta == null)
            {
                meta = gameObject.AddChildByTag(Constants.ActionTriggerTag);
                meta.layer = LayerMask.NameToLayer(Constants.TriggersLayer);
                
                if(! meta.TryGetComponent(out trigger))
                {
                    trigger = meta.AddComponent<SphereCollider>();
                }
                
            }
            
                
        }

        public void Start()
        {
            InitMetaObjects();
        }


        public void OnValidate()
        {
            InitMetaObjects();
        }

        
        
    }
}