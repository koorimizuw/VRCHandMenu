﻿
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Yamadev.VRCHandMenu {
    public abstract class TimeEventListener : UdonSharpBehaviour
    {
        public virtual void TimeSecondEvent() { }
    }
}