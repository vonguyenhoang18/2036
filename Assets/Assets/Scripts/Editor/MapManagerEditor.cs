using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    private int _targetLevel;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapManager manager = (MapManager)target;

        GUILayout.Space(8);
        GUILayout.Label("Set Level");
        _targetLevel = EditorGUILayout.IntField("Level", _targetLevel);
        if (GUILayout.Button("Set Level"))
        {
            manager.SetLevel(_targetLevel);
            Debug.Log($"!@# CurrentLevel set to {_targetLevel}");
        }
    }
}
