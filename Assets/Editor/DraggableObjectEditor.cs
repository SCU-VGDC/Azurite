/*using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DraggableObject))]
[CanEditMultipleObjects] // Allow multiple objects to be edited
public class DraggableObjectEditor : Editor
{
    public void OnSceneGUI()
    {
        DraggableObject obj = (DraggableObject)target;

        if (obj.DoesSnap)
        {
            Undo.RecordObject(obj, "Snap to Grid");
            obj.transform.position = obj.TilemapToWorld(obj.SnapToGrid(obj.WorldToTilemap(obj.transform.position)));
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the default Unity Inspector

        DraggableObject obj = (DraggableObject)target;

        if (GUILayout.Button("Update Collider Size"))
        {
            obj.OnValidate(); // Call OnValidate() to update the collider size
        }
    }
}*/