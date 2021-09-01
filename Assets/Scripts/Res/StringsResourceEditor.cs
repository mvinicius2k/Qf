using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Res
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StringsResource))]
    class StringsResourceEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            StringsResource script = (StringsResource)target;
            EditorGUILayout.Space();
            if (GUILayout.Button(nameof(script.Load)))
            {
                Undo.RecordObject(target, target.name);
                script.Load();
            }
            if (GUILayout.Button(nameof(script.Save)))
            {
                script.Save();
            }
            if (GUILayout.Button(nameof(script.OpenFolder)))
            {
                script.OpenFolder();
            }








        }
    }
#endif
}
