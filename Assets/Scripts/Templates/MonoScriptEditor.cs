using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Templates
{
    [CustomEditor(typeof(MonoScript), true)]
    class MonoScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            MonoScript script = (MonoScript)target;
            EditorGUILayout.Space();
            if (GUILayout.Button("Create Meta Objects"))
            {
                script.InitMetaObjects(true);
            }
            if (GUILayout.Button("Remove Meta Objects"))
            {
                script.RemoveGameObjects();
            }
            if (GUILayout.Button("Reset Meta Objects"))
            {
                script.ResetMetaObjects();
            }



        }




    }
}
