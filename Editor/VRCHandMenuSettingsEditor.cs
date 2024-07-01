
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using UdonSharp;
using UnityEditor;
using UnityEditorInternal;
using Yamadev.VRCHandMenu.Script;
using UnityEngine.Rendering.PostProcessing;

namespace Yamadev.VRCHandMenu.Editor
{
    [CustomEditor(typeof(VRCHandMenuSettings))]
    public class VRCHandMenuSettingsEditor : EditorBase
    {
        SerializedProperty _mainColor;
        SerializedProperty _canvas;
        SerializedProperty _menuSize;
        SerializedProperty _menuDistance;

        SerializedProperty _pens;
        SerializedProperty _switches;

        SerializedProperty _alarmSound;
        SerializedProperty _alarmVolume;
        SerializedProperty _alarmSource;

        SerializedProperty _ppBloom;
        SerializedProperty _ppMSVO;
        SerializedProperty _ppSAO;
        SerializedProperty _ppNight;

        SerializedProperty _lightDirectional;
        SerializedProperty _lightAvatar;

        SerializedProperty _lightDirectionalSlider;
        SerializedProperty _lightAvatarSlider;

        SerializedProperty _yamaPlayer;

        ReorderableList _penList;
        ReorderableList _switchList;

        VRCHandMenuSettings _target;

        private void OnEnable()
        {
            _target = target as VRCHandMenuSettings;

            _mainColor = serializedObject.FindProperty("mainColor");
            _canvas = serializedObject.FindProperty("canvas");
            _menuSize = serializedObject.FindProperty("menuSize");
            _menuDistance = serializedObject.FindProperty("menuDistance");

            _pens = serializedObject.FindProperty("pens");
            _switches = serializedObject.FindProperty("switches");

            _alarmSound = serializedObject.FindProperty("alarmSound");
            _alarmVolume = serializedObject.FindProperty("alarmVolume");
            _alarmSource = serializedObject.FindProperty("alarmSource");

            _ppBloom = serializedObject.FindProperty("PPBloom");
            _ppMSVO = serializedObject.FindProperty("PPMSVO");
            _ppSAO = serializedObject.FindProperty("PPSAO");
            _ppNight = serializedObject.FindProperty("PPNight");

            _lightDirectional = serializedObject.FindProperty("LightDirectional");
            _lightAvatar = serializedObject.FindProperty("LightAvatar");

            _lightDirectionalSlider = serializedObject.FindProperty("LightDirectionalSlider");
            _lightAvatarSlider = serializedObject.FindProperty("LightAvatarSlider");

            _yamaPlayer = serializedObject.FindProperty("yamaPlayer");

            if (_penList == null)
                _penList = new ReorderableList(serializedObject, _pens)
                {
                    onAddCallback = (ReorderableList list) => {
                        _pens.arraySize++;
                    },
                    drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, _pens.displayName);
                    },
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        EditorGUI.PropertyField(rect, _pens.GetArrayElementAtIndex(index));
                    },
                    elementHeightCallback = (index) =>
                    {
                        return EditorGUI.GetPropertyHeight(_pens.GetArrayElementAtIndex(index)) + EditorGUIUtility.standardVerticalSpacing;
                    },
                    onReorderCallback = (list) =>
                    {
                        ApplyModifiedProperties();
                    }
                };
            if (_switchList == null)
                _switchList = new ReorderableList(serializedObject, _switches) {
                    drawHeaderCallback = (rect) =>
                    {
                        EditorGUI.LabelField(rect, _switches.displayName);
                    },
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        EditorGUI.PropertyField(rect, _switches.GetArrayElementAtIndex(index));
                    },
                    elementHeightCallback = (index) =>
                    {
                        return EditorGUI.GetPropertyHeight(_switches.GetArrayElementAtIndex(index)) + EditorGUIUtility.standardVerticalSpacing;
                    },
                    onReorderCallback = (list) =>
                    {
                        ApplyModifiedProperties();
                    }
                };
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            EditorGUILayout.LabelField("VRC Hand Menu Settings", _uiTitle);

            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("UI", _bold);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_mainColor);
                EditorGUILayout.PropertyField(_menuSize);
                EditorGUILayout.PropertyField(_menuDistance);
                EditorGUI.indentLevel--;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Post Processing", _bold);

                EditorGUILayout.LabelField("Default Value (0 is OFF)");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_ppBloom);
                EditorGUILayout.PropertyField(_ppMSVO);
                EditorGUILayout.PropertyField(_ppSAO);
                EditorGUILayout.PropertyField(_ppNight);
                EditorGUI.indentLevel--;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Light", _bold);

                EditorGUILayout.LabelField("Default Value (0 is OFF)");
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_lightDirectional);
                EditorGUILayout.PropertyField(_lightAvatar);
                EditorGUI.indentLevel--;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Alarm", _bold);
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_alarmSource);
                EditorGUILayout.PropertyField(_alarmSound);
                EditorGUILayout.PropertyField(_alarmVolume);
                EditorGUI.indentLevel--;
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Pen List", _bold);

                if (_penList != null) _penList.DoLayoutList();
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Switch Targets", _bold);

                if (_switchList != null) _switchList.DoLayoutList();
            }

            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Video Player", _bold);

                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_yamaPlayer);
                EditorGUI.indentLevel--;
            }

            if (serializedObject.ApplyModifiedProperties())
                ApplyModifiedProperties();
        }

        internal void ApplyModifiedProperties()
        {
            // UI
            if (_mainColor.colorValue != null)
            {
                Transform canvas = _canvas.objectReferenceValue as Transform;
                for (int i = 0; i < canvas.childCount; i++)
                {
                    Image ret = canvas.GetChild(i).GetComponent<Image>();
                    if (ret == null) continue;
                    ret.color = _mainColor.colorValue;
                }
            }
            _target.MenuHandle.SetVariable("menuSize", _menuSize.floatValue);
            _target.MenuHandle.SetVariable("distance", _menuDistance.floatValue);

            // PP
            PostProcessVolume ppBloom = _target.PPHandle.Find("BLOOM").GetComponent<PostProcessVolume>();
            ppBloom.weight = _target.PPBloom;
            PostProcessVolume ppMSVO = _target.PPHandle.Find("AO/MSVO").GetComponent<PostProcessVolume>();
            ppMSVO.weight = _target.PPMSVO;
            PostProcessVolume ppSao = _target.PPHandle.Find("AO/SAO").GetComponent<PostProcessVolume>();
            ppSao.weight = _target.PPSAO;
            PostProcessVolume ppNight = _target.PPHandle.Find("NIGHTMODE").GetComponent<PostProcessVolume>();
            ppNight.weight = _target.PPNight;

            // Light
            Light lightDirectional = _target.LightHandle.Find("DirectionalLight").GetComponent<Light>();
            lightDirectional.intensity = _target.LightDirectional;
            (_lightDirectionalSlider.objectReferenceValue as Slider).value = _target.LightDirectional;

            Light lightAvatar = _target.LightHandle.Find("AvatarLight").GetComponent<Light>();
            lightAvatar.intensity = _target.LightAvatar;
            (_lightAvatarSlider.objectReferenceValue as Slider).value = _target.LightAvatar;

            // Alarm
            AudioSource alarmSource = _alarmSource.objectReferenceValue as AudioSource;
            alarmSource.clip = _alarmSound.objectReferenceValue as AudioClip;
            alarmSource.transform.GetComponent<VRC_SpatialAudioSource>().Gain = _alarmVolume.floatValue;

            // Pen
            int penCount = _target.PenContent.childCount;
            for (int i = 1; i < penCount; i++)
            {
                DestroyImmediate(_target.PenContent.GetChild(1).gameObject);
            }

            if (_pens.arraySize > 0)
            {
                Transform template = _target.PenContent.transform.GetChild(0);
                template.gameObject.SetActive(false);

                for (int i = 0; i < _pens.arraySize; i++)
                {
                    VRC_Pickup targetPen = _pens.GetArrayElementAtIndex(i).FindPropertyRelative("TargetObject").objectReferenceValue as VRC_Pickup;
                    if (targetPen.transform == null) continue;

                    GameObject newPen = Instantiate(template.gameObject, _target.PenContent, false);
                    GameObjectUtility.EnsureUniqueNameForSibling(newPen);

                    Text penName = newPen.transform.Find("Title/Text").GetComponent<Text>();
                    penName.text = _pens.GetArrayElementAtIndex(i).FindPropertyRelative("PenName").stringValue;

                    UdonSharpBehaviour penUdon = newPen.transform.GetComponent<UdonSharpBehaviour>();
                    penUdon.SetVariable("penObject", targetPen);

                    newPen.SetActive(true);
                }
            }

            // Swtich
            int switchCount = _target.SwitchContent.childCount;
            for (int i = 1; i < switchCount; i++)
            {
                DestroyImmediate(_target.SwitchContent.GetChild(1).gameObject);
            }
            if (_switches.arraySize > 0)
            {
                Transform template = _target.SwitchContent.transform.GetChild(0);
                template.gameObject.SetActive(false);

                for (int i = 0; i < _switches.arraySize; i++)
                {
                    GameObject targetObject = _switches.GetArrayElementAtIndex(i).FindPropertyRelative("TargetObject").objectReferenceValue as GameObject;
                    if (targetObject == null) continue;

                    GameObject newSwitch = Instantiate(template.gameObject, _target.SwitchContent, false);
                    GameObjectUtility.EnsureUniqueNameForSibling(newSwitch);

                    Text switchName = newSwitch.transform.Find("Title/Text").GetComponent<Text>();
                    switchName.text = _switches.GetArrayElementAtIndex(i).FindPropertyRelative("ObjectName").stringValue;

                    bool defaultActive = _switches.GetArrayElementAtIndex(i).FindPropertyRelative("DefaultActive").boolValue;

                    UdonSharpBehaviour switchUdon = newSwitch.transform.GetComponent<UdonSharpBehaviour>();
                    switchUdon.SetVariable("isGlobal", _switches.GetArrayElementAtIndex(i).FindPropertyRelative("IsGlobal").boolValue);
                    switchUdon.SetVariable("targetObject", targetObject);

                    if (targetObject != null)
                        targetObject.SetActive(defaultActive);

                    newSwitch.SetActive(true);
                }
            }
        }
    }
}