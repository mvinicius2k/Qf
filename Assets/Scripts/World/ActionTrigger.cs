using Assets.Scripts.Common;
using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.World
{
    public class ActionTrigger : MonoBehaviour, IContextAction
    {
        public UltEvents.UltEvent action;
        public Collider trigger;
        public bool repeatable = true;
        private int count;
        public GUIActionContext GUIActionContext;

        private int colliderCount;

        [Header("Gizmos")][SerializeField]
        private float gizmoRadius = 0.2f;
        [SerializeField]
        private Color gizmoActiveColor = new Color(0f, 0.8f, 0.2f, 0.5f);
        [SerializeField]
        private Color gizmoInactiveColor = new Color(0.8f, 0f, 0.2f, 0.5f);
        public void Action()
        {
            count++;
            if (!repeatable && count > 1)
                return;
            action.Invoke();
            Debug.Log($"Action {gameObject.name}");
        }

        

        public void OnTriggerEnter()
        {
            
            colliderCount++;
        }

        public void OnTriggerExit()
        {
            
            colliderCount--;
        }


        public void SetObjectTriggable()
        {
            gameObject.tag = Constants.ActionTriggerTag;
            gameObject.layer = LayerMask.NameToLayer(Constants.TriggersLayer);

            if (trigger != null)
                trigger.isTrigger = true;
            else
                Debug.Log($"{nameof(trigger)} nulo");
            
        }
        public void SetObjectAutoTriggable()
        {
            gameObject.tag = Constants.AutoActionTriggerTag;
            gameObject.layer = LayerMask.NameToLayer(Constants.TriggersLayer);

            if (trigger != null)
                trigger.isTrigger = true;
            else
                Debug.Log($"{nameof(trigger)} nulo");
        }

        public void OnDrawGizmos()
        {
            if(trigger != null)
            {
                var pos = trigger.bounds.center;

                
                Gizmos.color = colliderCount == 0 ? gizmoInactiveColor : gizmoActiveColor;
                Gizmos.DrawSphere(pos, gizmoRadius);
            }
        }
    }
}
