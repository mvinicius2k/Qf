using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrackChanger))]
[CanEditMultipleObjects]
class TrackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Track track = (Track)target;


    }
}
   