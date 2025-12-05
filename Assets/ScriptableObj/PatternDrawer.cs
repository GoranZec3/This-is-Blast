using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Row))]
public class PatternDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty cells = property.FindPropertyRelative("cells");

        int count = cells.arraySize;
        float cellWidth = position.width / count;

        EditorGUI.BeginProperty(position, label, property);

        for (int i = 0; i < count; i++)
        {
            Rect cellRect = new Rect(
                position.x + i * cellWidth,
                position.y,
                cellWidth - 2,
                EditorGUIUtility.singleLineHeight
            );

            EditorGUI.PropertyField(cellRect, cells.GetArrayElementAtIndex(i), GUIContent.none);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight + 4;
    }
}
