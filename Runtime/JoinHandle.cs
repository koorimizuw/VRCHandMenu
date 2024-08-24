
using UdonSharp;
using System;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class JoinHandle : TimeEventListener
    {
        [SerializeField] TimeHandle timeHandle;
        [SerializeField] GameObject logContent;
        [SerializeField] Text instancePlayerCountText;
        [UdonSynced] string[] joinLogs = new string[0];

        void Start()
        {
            if (timeHandle) timeHandle.AddListener(this);
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            UpdateInRoomPlayerCount();

            if (!Networking.IsOwner(gameObject)) return;

            int status = 0;
            if (joinLogs != null && joinLogs.Length != 0)
            {
                string[] lastLog = parseLog(joinLogs[joinLogs.Length - 1]);
                string lastPlayerName = lastLog[3];
                int lastJoinTime = int.Parse(lastLog[2]);

                if (DateTimeOffset.Now.ToUnixTimeSeconds() - lastJoinTime <= 60 && lastLog[0] == "1" && lastPlayerName == player.displayName)
                    status = 2;
            }

            addLog(status, player);
            UpdateView();
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            SendCustomEventDelayedSeconds(nameof(UpdateInRoomPlayerCount), 2.0f);

            if (!Networking.IsOwner(gameObject)) return;

            addLog(1, player);
            UpdateView();
        }

        public void UpdateInRoomPlayerCount()
        {
            if (!instancePlayerCountText) return;
            instancePlayerCountText.text = VRCPlayerApi.GetPlayerCount().ToString().PadLeft(2, '0');

        }

        private string[] parseLog(string log)
        {
            return log.Split(new string[] { ",,," }, StringSplitOptions.None);
        }

        private void addLog(int ststus, VRCPlayerApi player)
        {
            var isVR = player.IsUserInVR() ? 1 : 0;
            var time = DateTimeOffset.Now.ToUnixTimeSeconds();

            var content = $"{ststus},,,{isVR},,,{time},,,{player.displayName}";

            string[] ret = new string[joinLogs.Length + 1];
            joinLogs.CopyTo(ret, 0);
            ret[joinLogs.Length] = content;
            joinLogs = ret;

            RequestSerialization();
        }

        public void UpdateView()
        {
            if (!logContent || !logContent.activeSelf || joinLogs.Length == 0) return;

            var objCount = logContent.transform.childCount;
            int start = joinLogs.Length - objCount;
            start = start < 0 ? 0 : start;

            for (var i = start; i < joinLogs.Length; i++)
            {
                var logItem = logContent.transform.GetChild(i);

                string[] logInfo = parseLog(joinLogs[i]);

                var status = logInfo[0];
                var isVR = logInfo[1];
                var joinTime = logInfo[2];
                var playerName = logInfo[3];

                Image joinMark = logItem.transform.Find("JoinMark").GetComponent<Image>();
                Image leftMark = logItem.transform.Find("LeftMark").GetComponent<Image>();
                Image rejoinMark = logItem.transform.Find("RejoinMark").GetComponent<Image>();

                if (joinMark) joinMark.gameObject.SetActive(false);
                if (leftMark) leftMark.gameObject.SetActive(false);
                if (rejoinMark) rejoinMark.gameObject.SetActive(false);

                switch (status)
                {
                    case "0": // join
                        if (joinMark) joinMark.gameObject.SetActive(true);
                        break;
                    case "1": // left
                        if (leftMark) leftMark.gameObject.SetActive(true);
                        break;
                    case "2": // rejoin
                        if (rejoinMark) rejoinMark.gameObject.SetActive(true);
                        var lastItem = logContent.transform.GetChild(i-1);
                        if (lastItem) lastItem.gameObject.SetActive(false);
                        break;
                }

                Text playerNameText = logItem.transform.Find("Name").GetComponent<Text>();
                if (playerNameText)
                    playerNameText.text = $"{playerName}";

                UInt32 timeStamp = uint.Parse(joinTime);
                DateTime localJoinTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timeStamp).ToLocalTime();

                TimeSpan timeSpan = DateTime.Now - localJoinTime;

                var hourSpan = Math.Floor(timeSpan.TotalHours);
                var minuteSpan = Math.Floor(timeSpan.TotalMinutes % 60);

                var hourStr = hourSpan != 0 ? $"{hourSpan}時間" : "";
                var minuteStr = $"{minuteSpan}分";

                Text timeText = logItem.transform.Find("Time").GetComponent<Text>();
                if (timeText) timeText.text = localJoinTime.ToString("HH:mm:ss");

                Text extraText = logItem.transform.Find("Extra").GetComponent<Text>();
                if (extraText) extraText.text = $"({hourStr}{minuteStr}前)";

                logItem.gameObject.SetActive(true);
            }
        }

        public override void TimeSecondEvent() => UpdateView();

        public override void OnDeserialization() => UpdateView();
    }
}