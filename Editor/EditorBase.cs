
using UnityEngine;

namespace Yamadev.VRCHandMenu.Editor
{
    public abstract class EditorBase : UnityEditor.Editor
    {
        protected GUIStyle _uiTitle;
        protected GUIStyle _bold;
        protected virtual void Initilize()
        {
            _uiTitle = new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18,
            };
            _uiTitle.normal.textColor = Color.white;

            _bold = new GUIStyle(GUI.skin.label);
            _bold.fontStyle = FontStyle.Bold;
        }

        public override void OnInspectorGUI()
        {
            Initilize();
        }
    }


}