using UnityEngine;
using UnityEditor;
using Opibus;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    //public override void OnInspectorGUI()
    //{
    //    Building building = (Building)target;
    //
    //    if (GUILayout.Button("Collected Resources"))
    //    {
    //        Debug.Log(building.ProducedResources);
    //    }
    //}
}