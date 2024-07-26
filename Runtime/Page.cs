
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Yamadev.VRCHandMenu
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Page : UdonSharpBehaviour
    {
        [SerializeField]
        Page parentPage;

        void Start()
        {

        }

        public Page ParentPage => parentPage;
    }

}