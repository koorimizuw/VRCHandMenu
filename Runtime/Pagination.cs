
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Pagination : UdonSharpBehaviour
    {
        [SerializeField]
        MenuHandle menuHandle;
        [SerializeField]
        Page targetPage;

        void Start()
        {
            
        }

        public void ShowTargetPage()
        {
            if (!targetPage) return;
            menuHandle.ShowTargetPage(targetPage);
        }
    }
}