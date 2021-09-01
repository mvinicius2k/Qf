using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Res
{//
# if UNITY_EDITOR 
    [CustomEditor(typeof(StringResource))]
    class StringResourceEditor : Editor
    {
        StringResourceNode[] options = StringsResource.AllLeafs;
        
        int index = 0;
        int oldIndex = 0;

        public override void OnInspectorGUI()
        {
            if(options != null)
            {

                StringResource myTarget = (StringResource)target;

                DrawDefaultInspector();
                oldIndex = index;
                oldIndex = EditorGUILayout.Popup("Player", index, options.Select(s => s.key).ToArray());
                // Update the selected choice in the underlying object
                myTarget.source = options[oldIndex];
            }
            else
            {
                options = StringsResource.AllLeafs;
            }

        }
    }
#endif
}
