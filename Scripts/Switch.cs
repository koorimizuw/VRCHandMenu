using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yamadev.VRCHandMenu.Script
{
    [Serializable]
    public class Switch
    {
        public bool IsGlobal = false;
        public bool DefaultActive = true;
        public string ObjectName;
        public GameObject TargetObject;
    }
}