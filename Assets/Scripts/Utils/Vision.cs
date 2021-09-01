using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class Vision : MonoBehaviour
    {
        private Dictionary<int, GameObject> captured = new Dictionary<int, GameObject>();


        public string[] tagFilter;

        public Dictionary<int, GameObject> Captured { get => captured; }

        private bool hasTag(GameObject obj)
        {
            for (int i = 0; i < tagFilter.Length; i++)
            {
                if (obj.CompareTag(tagFilter[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public void OnTriggerEnter(Collider other)
        {

           

            if (hasTag(other.gameObject))
            {
                captured[other.gameObject.GetInstanceID()] = other.gameObject;
                //captured.Add(other.gameObject.GetInstanceID(), other.gameObject);
                Debug.Log($" >>> Total de inimigos = {captured.Count}");
                


            }
        }

        public void OnTriggerExit(Collider other)
        {
           
                    captured.Remove(other.gameObject.GetInstanceID());
                    Debug.Log($" <<< Total de inimigos = {captured.Count}");


        }
    }
}
