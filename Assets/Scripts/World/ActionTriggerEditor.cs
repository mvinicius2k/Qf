using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.World
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ActionTrigger))]
    public class ActionTriggerEditor : Editor
    {//
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ActionTrigger script = (ActionTrigger)target;
            EditorGUILayout.Space();
            if (GUILayout.Button(nameof(script.SetObjectTriggable)))
            {
                Undo.RecordObject(target, target.name);
                script.SetObjectTriggable();
            }
            if (GUILayout.Button(nameof(script.SetObjectAutoTriggable)))
            {
                Undo.RecordObject(target, target.name);
                script.SetObjectAutoTriggable();
            }









        }
    }
#endif
}
