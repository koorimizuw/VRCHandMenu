
using UdonSharp;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PenHandle : UdonSharpBehaviour
    {
        [SerializeField]
        VRC_Pickup penObject = null;

        Text _tipText;
        Vector3 _defaultPosition;
        Quaternion _defaultRotation;

        private PositionConstraint clearButtonPositionConstraint;
        private RotationConstraint clearButtonRotationConstraint;

        void Start()
        {
            _defaultPosition = penObject.transform.position;
            _defaultRotation = penObject.transform.rotation;
            _tipText = transform.Find("Tip/Text").GetComponent<Text>();

            if (penObject.transform.parent.Find("Bureau/UI/Clear") != null )
            {
                clearButtonPositionConstraint = penObject.transform.parent.Find("Bureau/UI/Clear").GetComponentInChildren<PositionConstraint>();
                clearButtonRotationConstraint = penObject.transform.parent.Find("Bureau/UI/Clear").GetComponentInChildren<RotationConstraint>();
            }
        }
        void Update()
        {
            if (!_tipText) return;
            if (penObject.IsHeld)
            {
                var owner = Networking.GetOwner(penObject.gameObject);
                _tipText.text = $"使用中: {owner.displayName}";
                _tipText.color = new Color(255.0f / 255.0f, 128.0f / 255.0f, 103.0f / 255.0f, 255.0f / 255.0f);
            }
            else
            {
                _tipText.text = $"使用中: None";
                _tipText.color = new Color(159.0f / 255.0f, 255.0f / 255.0f, 103.0f / 255.0f, 255.0f / 255.0f);
            }
        }
        public void CallPen()
        {
            if (!penObject || penObject.IsHeld) return;

            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Networking.SetOwner(Networking.LocalPlayer, penObject.gameObject);
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(TeleportPen));

        }

        public void ResetPen()
        {
            if (!penObject || penObject.IsHeld) return;
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            Networking.SetOwner(Networking.LocalPlayer, penObject.gameObject);
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(ResetPenPosition));
            SendCustomNetworkEvent(NetworkEventTarget.All, nameof(EnableClearButtonConstraints));
        }

        public void TeleportPen()
        {
            var hand = Networking.GetOwner(penObject.gameObject).GetTrackingData(VRCPlayerApi.TrackingDataType.RightHand);
            penObject.transform.position = hand.position;
        }

        public void ResetPenPosition()
        {
            penObject.transform.SetPositionAndRotation(_defaultPosition, _defaultRotation);
        }

        public void EnableClearButtonConstraints()
        {
            if (clearButtonPositionConstraint == null ||  clearButtonRotationConstraint == null) return;
            if (clearButtonPositionConstraint)
                clearButtonPositionConstraint.enabled = true;
            if (clearButtonRotationConstraint)
                clearButtonRotationConstraint.enabled = true;

            SendCustomEventDelayedSeconds(nameof(_DisableClearButtonConstraints), 2f);
        }

        public void _DisableClearButtonConstraints()
        {
            if (clearButtonPositionConstraint == null || clearButtonRotationConstraint == null) return;
            if (clearButtonPositionConstraint)
                clearButtonPositionConstraint.enabled = false;
            if (clearButtonRotationConstraint)
                clearButtonRotationConstraint.enabled = false;
        }
    }
}