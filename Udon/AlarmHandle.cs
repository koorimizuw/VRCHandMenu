
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using System;

namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class AlarmHandle : UdonSharpBehaviour
    {
        [SerializeField]
        TimeHandle timeHandle;

        [Header("UI")]
        [SerializeField]
        Transform currentTime;
        [SerializeField]
        Transform alarmTime;
        [SerializeField]
        Transform ellipsis;
        [SerializeField]
        Transform hourController;
        [SerializeField]
        Transform minuteController;
        [SerializeField]
        Button setButton;

        [SerializeField]
        private Text mainPanelText;
        

        [Header("Settings")]
        [SerializeField]
        private AudioSource _alarmSound;

        private int _alarmHour = 8;
        private int _alarmMin = 0;
        private DateTime _alarmTime;

        private bool _enable = false;
        private bool _isActive = false;
        private float _activeTime = 0.0f;

        void Start()
        {
            timeHandle.AddListener(this);
            adjustTime();
        }

        void Update()
        {
            if (!_enable) return;

            if (_isActive)
            {
                var head = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head);
                _alarmSound.transform.SetPositionAndRotation(head.position, head.rotation);

                float soundFade = Mathf.Clamp01((Time.time - _activeTime) * 0.05f);
                _alarmSound.volume = soundFade * soundFade * 0.9f + 0.1f;

                // Auto off
                if (Time.time - _activeTime > 300f) turnOffAlarm();
                return;
            }

            DateTime now = DateTime.Now;
            if (now > _alarmTime) turnOnAlarm();
        }

        public void TimeSecondEvent()
        {
            if (!_enable) return; 

            updateView(currentTime, DateTime.Now);

            Text sleepDurationText = ellipsis.transform.Find("Text").GetComponent<Text>();
            if (DateTime.Now > _alarmTime)
            {
                sleepDurationText.text = "おはようございます！";
                return;
            }

            TimeSpan timeSpan = _alarmTime - DateTime.Now;

            var hourSpan = Math.Floor(timeSpan.TotalHours);
            var minuteSpan = Math.Ceiling(timeSpan.TotalMinutes % 60);

            var hourStr = hourSpan != 0 ? $"{hourSpan}時間" : "";
            var minuteStr = $"{minuteSpan}分";

            sleepDurationText.text = $"{hourStr}{minuteStr}後";
        }

        void turnOnAlarm()
        {
            _isActive = true;

            _alarmSound.volume = 0.1f;
            _alarmSound.Play();

            _activeTime = Time.time;
        }

        void turnOffAlarm()
        {
            _isActive = false;

            _alarmSound.Stop();
        }

        void addHours(int hour)
        {
            var ret = _alarmHour + hour;
            _alarmHour = ret < 0 ? 0 : ret > 23 ? 23 : ret;

            adjustTime();
        }

        public void AddOneHour()
        {
            addHours(1);
        }

        public void MinusOneHour()
        {
            addHours(-1);
        }

        void addMinutes(int min)
        {
            var ret = _alarmMin + min;
            _alarmMin = ret < 0 ? 0 : ret > 59 ? 59 : ret;

            adjustTime();
        }

        public void AddOneMinute()
        {
            addMinutes(1);
        }

        public void MinusOneMinute()
        {
            addMinutes(-1);
        }
        public void AddTenMinutes()
        {
            addMinutes(10);
        }

        public void MinusTenMinutes()
        {
            addMinutes(-10);
        }

        void adjustTime()
        {
            DateTime now = DateTime.Now;
            _alarmTime = now.Date.AddMinutes(_alarmHour * 60 + _alarmMin);
            _alarmTime = _alarmTime <= now ? _alarmTime.AddDays(1.0) : _alarmTime;

            updateView(alarmTime, _alarmTime);
        }

        void updateView(Transform t, DateTime time)
        {
            Text hourText = t.transform.Find("Hour").GetComponent<Text>();
            Text minText = t.transform.Find("Minute").GetComponent<Text>();
            Text dayText = t.transform.Find("Day").GetComponent<Text>();

            hourText.text = time.ToString("HH").PadLeft(2, '0');
            minText.text = time.ToString("mm").PadLeft(2, '0');

            dayText.text = time.ToString("MM/dd ddd");

            Text amText = t.transform.Find("AM").GetComponent<Text>();
            Text pmText = t.transform.Find("PM").GetComponent<Text>();

            int hour = int.Parse(time.ToString("HH"));
            if (hour < 12)
            {
                amText.gameObject.SetActive(true);
                pmText.gameObject.SetActive(false);
                if (hour > 10) amText.text = "正午";
                else amText.text = "午前";
            }
            else
            {
                amText.gameObject.SetActive(false);
                pmText.gameObject.SetActive(true);
                if (hour < 15) pmText.text = "正午";
                else pmText.text = "午後";
            }
        }

        public void SetAlarm()
        {
            if (_isActive) turnOffAlarm();
            adjustTime();

            mainPanelText.text = _alarmTime.ToString("HH:mm");
            _enable = true;

            currentTime.gameObject.SetActive(true);
            ellipsis.gameObject.SetActive(true);
            hourController.gameObject.SetActive(false);
            minuteController.gameObject.SetActive(false);
            setButton.gameObject.SetActive(false);
        }

        public void ClearAlarm()
        {
            mainPanelText.text = "未設定";
            _enable = false;

            if (_isActive) turnOffAlarm();

            currentTime.gameObject.SetActive(false);
            ellipsis.gameObject.SetActive(false);
            hourController.gameObject.SetActive(true);
            minuteController.gameObject.SetActive(true);
            setButton.gameObject.SetActive(true);
        }
    }
}