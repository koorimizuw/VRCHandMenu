using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PPController : UdonSharpBehaviour
    {
        [SerializeField]
        private PostProcessVolume[] PP;
        [SerializeField]
        private Text PPText;
        [SerializeField]
        private Slider PPSlider;

        private Text _PPText;
        private Slider _PPSlider;

        void Start()
        {
            _PPText = PPText ? PPText : transform.Find("Text/Text").GetComponent<Text>();
            _PPSlider = PPSlider ? PPSlider : transform.Find("Slider").GetComponent<Slider>();
        }

        void Update()
        {
            foreach (PostProcessVolume item in PP)
            {
                if (!item.gameObject.activeSelf) continue;
                _PPSlider.value = item.enabled ? item.weight : 0.0f;
            }
        }


        public void OnChange()
        {
            if (PP.Length == 0 || !_PPText || !_PPSlider) return;

            var value = _PPSlider.value;
            if (value == 0.0f)
            {
                _PPText.text = "OFF";
                foreach (PostProcessVolume item in PP)
                {
                    item.enabled = false;
                }
            }
            else
            {
                _PPText.text = $"{Math.Floor(value * 100)}%";
                foreach (PostProcessVolume item in PP)
                {
                    item.enabled = true;
                    item.weight = value;
                }
            }
            
        }
    }
}