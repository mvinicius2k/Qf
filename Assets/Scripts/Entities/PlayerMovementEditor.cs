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
            if (GUILayout.Button("Set Min Feet Bounds"))
            {
                controller.SetFeetBounds(new Vector3(
                    controller.minFeetCheckSizeOffset,
                    controller.minFeetCheckSizeOffset,
                    controller.minFeetCheckSizeOffset));
            }
            if (GUILayout.Button("Set Extended Feet Bounds"))
            {
                controller.SetFeetBounds(new Vector3(
                    0f,
                    controller.extendedFeetCheckSizeOffset,
                    0f));
            }

        }


        

    }
}
