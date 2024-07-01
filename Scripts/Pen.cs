using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRC.SDKBase;

namespace Yamadev.VRCHandMenu.Script
{
    [Serializable]
    public class Pen
    {
        public string PenName;
        public VRC_Pickup TargetObject;
    }
}