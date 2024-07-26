using UnityEngine;
using UnityEngine.UI;
using UdonSharp;
using Yamadev.YamaStream;

namespace Yamadev.VRCHandMenu.Script
{
    public class VRCHandMenuSettings : MonoBehaviour
    {
        // UI
        [SerializeField]
        Color mainColor;
        [SerializeField]
        Transform canvas;
        [SerializeField, Range(0, 1)]
        float menuSize = 0.85f;
        [SerializeField, Range(0, 0.2f)]
        float menuDistance = 0.05f;

        // PP
        [Range(0.0f, 1.0f)]
        public float PPBloom = 0.0f;
        [Range(0.0f, 1.0f)]
        public float PPMSVO = 0.0f;
        [Range(0.0f, 1.0f)]
        public float PPSAO = 0.0f;
        [Range(0.0f, 1.0f)]
        public float PPNight;

        [SerializeField]
        Pen[] pens;
        [SerializeField]
        Switch[] switches;

        [SerializeField]
        AudioSource alarmSource;
        [SerializeField]
        AudioClip alarmSound;
        [SerializeField, Range(0, 100)]
        float alarmVolume = 10.0f;

        [SerializeField]
        UdonSharpBehaviour menuHandle;

        [SerializeField]
        Transform ppHandle;
        [SerializeField]
        Transform lightHandle;
        [SerializeField]
        Transform penContent;
        [SerializeField]
        Transform switchContent;

        [Range(0.0f, 1.0f)]
        public float LightDirectional = 1.0f;
        [Range(0.0f, 1.0f)]
        public float LightAvatar = 0.1f;

        public Slider LightDirectionalSlider;
        public Slider LightAvatarSlider;

        public Controller yamaPlayer;
        public RawImage videoScreen;

        public Transform PPHandle => ppHandle;
        public Transform LightHandle => lightHandle;
        public Transform PenContent => penContent;
        public Transform SwitchContent => switchContent;
        public UdonSharpBehaviour MenuHandle => menuHandle;
    }
}