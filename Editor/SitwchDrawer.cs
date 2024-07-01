
using UnityEngine;
using UnityEditor;
using Yamadev.VRCHandMenu.Script;

namespace Yamadev.VRCHandMenu.Editor
{
    [CustomPropertyDrawer(typeof(Switch))]
    public class SwitchDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Rect rect = new Rect(position.x, position.y + 3, position.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(Switch.ObjectName)));
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(Switch.TargetObject)));
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(Switch.DefaultActive)));
            rect.y += EditorGUIUtility.singleLineHeight + 2;

            EditorGUI.PropertyField(rect, property.FindPropertyRelative(nameof(Switch.IsGlobal)));
            rect.y += EditorGUIUtility.singleLineHeight + 2;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(Switch.ObjectName)))
                    + EditorGUIUtility.standardVerticalSpacing
                    + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(Switch.DefaultActive)))
                    + EditorGUIUtility.standardVerticalSpacing
                    + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(Switch.IsGlobal)))
                    + EditorGUIUtility.standardVerticalSpacing
                    + EditorGUI.GetPropertyHeight(property.FindPropertyRelative(nameof(Switch.TargetObject)));
        }
    }
}