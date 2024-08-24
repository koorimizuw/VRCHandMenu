
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;


namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class TimeHandle : UdonSharpBehaviour
    {
        [SerializeField] Text DateText;
        [SerializeField] Text TimeText;

        float _timeGap = 0f;
        TimeEventListener[] _listeners;

        public string[] dayOfWeekJP = { "日", "月", "火", "水", "木", "金", "土"};

        void Update()
        {
            if (_timeGap < 1.0f) { 
                _timeGap += Time.deltaTime;
            }

            _timeGap = 0f;
            updateTime();

            if (_listeners != null) foreach (var i in _listeners) i.TimeSecondEvent();
        }

        public void AddListener(TimeEventListener listener)
        {
            if (_listeners == null) _listeners = new TimeEventListener[0];
            TimeEventListener[] ret = new TimeEventListener[_listeners.Length + 1];
            _listeners.CopyTo(ret, 0);
            ret[_listeners.Length] = listener;
            _listeners = ret;
        }

        void updateTime()
        {
            if (!DateText || !TimeText) { return; }

            DateTime now = DateTime.Now;
            DateText.text = $"{now.ToString("yyyy/MM/dd")} {dayOfWeekJP[(int)now.DayOfWeek]}曜日";
            TimeText.text = now.ToString("HH:mm:ss");
        }

    }

}