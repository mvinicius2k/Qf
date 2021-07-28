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
            if (GUILayout.Button("Update Meta Objects"))
            {
                script.InitMetaObjects(true, false);
            }
            if (GUILayout.Button("Remove Meta Objects"))
            {
                if(EditorUtility.DisplayDialog(nameof(MonoScript), "Tem certeza?", "Sim", "Cancelar"))
                {
                    script.RemoveGameObjects(false);
                }
            }
            if (GUILayout.Button("Reset Meta Objects"))
            {
                if (EditorUtility.DisplayDialog(nameof(MonoScript), "Tem certeza?", "Sim", "Cancelar"))
                {
                    script.ResetMetaObjects(false);
                }
            }



        }




    }
}
