
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class SwitchHandle : UdonSharpBehaviour
    {
        [SerializeField]
        bool isGlobal = false;
        [SerializeField]
        GameObject targetObject;

        Image _icon;
        Color _activeColor = new Color(0.0f, 200.0f / 255.0f, 83.0f / 255.0f, 255.0f);
        Color _inactiveColor = new Color(197.0f / 255.0f, 17.0f / 255.0f, 98.0f / 255.0f, 255.0f);
        void Start()
        {
            _icon = transform.Find("Title/Image").GetComponent<Image>();
        }

        void Update()
        {
            if (targetObject.activeSelf) _icon.color = _activeColor;
            else _icon.color = _inactiveColor;
        }

        public void SetTargetActive()
        {
            targetObject.SetActive(true);
        }

        public void SetTargetInactive()
        {
            targetObject.SetActive(false);
        }

        public void SetActive()
        {
            if (isGlobal) SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetTargetActive));
            else SetTargetActive();
        }

        public void SetInactive()
        {
            if (isGlobal) SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(SetTargetInactive));
            else SetTargetInactive();
        }
    }
}