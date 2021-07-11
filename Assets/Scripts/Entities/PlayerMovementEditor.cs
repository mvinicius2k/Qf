using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Entities
{
    [CustomEditor(typeof(PlayerMovementController))]
    class PlayerMovementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            PlayerMovementController controller = (PlayerMovementController)target;
            EditorGUILayout.Space();
            if (GUILayout.Button("Set Auto Feet Bounds"))
            {
                controller.SetFeetBounds();
            }

        }


        

    }
}
