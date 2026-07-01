using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CutSceneAction))]
public class CutSceneActionDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float line = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;

        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, line),
            property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            EditorGUI.PropertyField(
                new Rect(position.x, position.y + line + pad, position.width, line),
                property.FindPropertyRelative("triggerTime"));

            SerializedProperty typeProp = property.FindPropertyRelative("type");
            EditorGUI.PropertyField(
                new Rect(position.x, position.y + (line + pad) * 2, position.width, line),
                typeProp);

            float y = position.y + (line + pad) * 3;
            var actionType = (CutSceneAction.ActionType)typeProp.enumValueIndex;

            if (actionType == CutSceneAction.ActionType.Move)
            {
                EditorGUI.PropertyField(
                    new Rect(position.x, y, position.width, line),
                    property.FindPropertyRelative("destination"));

                EditorGUI.PropertyField(
                    new Rect(position.x, y + line + pad, position.width, line),
                    property.FindPropertyRelative("scale"));

                EditorGUI.PropertyField(
                    new Rect(position.x, y + (line + pad) * 2, position.width, line),
                    property.FindPropertyRelative("duration"));
            }
            else
            {
                SerializedProperty actionProp = property.FindPropertyRelative("action");
                EditorGUI.PropertyField(
                    new Rect(position.x, y, position.width, EditorGUI.GetPropertyHeight(actionProp)),
                    actionProp);
            }

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float line = EditorGUIUtility.singleLineHeight;
        float pad = EditorGUIUtility.standardVerticalSpacing;

        if (!property.isExpanded)
            return line;

        SerializedProperty typeProp = property.FindPropertyRelative("type");
        var actionType = (CutSceneAction.ActionType)typeProp.enumValueIndex;

        float total = (line + pad) * 3; // foldout + triggerTime + type

        if (actionType == CutSceneAction.ActionType.Move)
            total += (line + pad) * 3; // destination + scale + duration
        else
            total += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("action")) + pad;

        return total;
    }
}
