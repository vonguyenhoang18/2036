using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapManager manager = (MapManager)target;

        GUILayout.Space(8);
        if (GUILayout.Button("Level +1"))
        {
            int next = manager.CurrentLevel + 1;
            manager.SetLevel(next);
            Debug.Log($"CurrentLevel set to {next}");
        }
    }
}
