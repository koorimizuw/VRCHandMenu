
using UnityEngine;
using UnityEditor;
using Yamadev.VRCHandMenu.Script;

namespace Yamadev.VRCHandMenu.Editor
{
    [CustomPropertyDrawer(typeof(Pen))]
    public class PenDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect rect = new Rect(position.x, position.y + 3, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(Pen.PenName)));
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(Pen.TargetObject)));
            rect.y += EditorGUIUtility.singleLineHeight + 2;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(Pen.PenName)))
                    + EditorGUIUtility.standardVerticalSpacing
                    + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(Pen.TargetObject)));
        }
    }
}