using UnityEngine;

namespace Script.Runtime.Core.GameAction
{
    public struct GameActionContext
    {
        public MonoBehaviour Caller;
        public MonoBehaviour Receiver;
    }
}