using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    [CustomEditor(typeof(ObjectsManager), true)]
    class ObjectsManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ObjectsManager script = (ObjectsManager)target;
            EditorGUILayout.Space();
            if (GUILayout.Button(nameof(script.DisableAllColliders)))
            {
                script.DisableAllColliders();
            }
            else if (GUILayout.Button(nameof(script.EnableAllColliders)))
            {
                script.EnableAllColliders();
            }


            



        }




    }
}
