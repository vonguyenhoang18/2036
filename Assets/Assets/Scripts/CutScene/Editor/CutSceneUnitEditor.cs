using UnityEditor;

[CustomEditor(typeof(CutSceneUnit))]
public class CutSceneUnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("animator"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("actions"), true);
        serializedObject.ApplyModifiedProperties();
    }
}
