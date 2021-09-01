using Assets.Scripts.Common;
using Assets.Scripts.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    class PlayerContextAction : MonoBehaviour
    {
        private Collider currentTrigger;
        public Collider triggerActivator;
        public Collider CurrentTrigger { get => currentTrigger; }

        public GUIActionContext GUIActionContext;

        private void Update()
        {
            
                if (Input.GetButtonDown(Constants.InputContextAction))
                {
                    Action();
                }


        }

        private void Action()
        {
            if(currentTrigger == null)
            {
                Debug.Log($"{nameof(currentTrigger)} null");
                return;
            }
            var trigger = currentTrigger.gameObject.GetComponentInParent(typeof(IContextAction));
            if (trigger != null)
            {
                var triggerInstance = (IContextAction)trigger;
                triggerInstance.Action();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Constants.ActionTriggerTag))
            {
                Debug.Log("Pressione E");
                currentTrigger = other;
                if (other.gameObject.CompareTag(Constants.ActionTriggerTag))
                    GUIActionContext.ShowUI();
            }
            else if(other.CompareTag(Constants.AutoActionTriggerTag))
            {
                currentTrigger = other;
                Action();
            }


        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Saiu");
            if (other.gameObject.CompareTag(Constants.ActionTriggerTag))
                GUIActionContext.HideUI();
            if (other != null && currentTrigger != null)
            {
                if (other.GetInstanceID() == currentTrigger.GetInstanceID())
                {
                    currentTrigger = null;
                }

            }
        }
    }
}
