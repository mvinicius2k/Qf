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
    class ContextAction : MonoBehaviour
    {
        private Collider currentTrigger;
        public Collider CurrentTrigger { get => currentTrigger; }

        private void Update()
        {
            if (currentTrigger != null)
            {
                if (Input.GetButtonDown(Constants.InputContextAction))
                {
                    var trigger = currentTrigger.gameObject.GetComponentInParent(typeof(IContextAction));
                    if (trigger != null)
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
