using System;
using UnityEngine;

namespace Script.Runtime.Core.GlobalHandlerManagement
{
    public class GlobalMonoBehaviourHandler<T> : MonoBehaviour where T : GlobalMonoBehaviourHandler<T>
    {
        private static T instance;
        
        public static T Instance => instance;

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                Debug.LogError($"Scrabble RTS - Core: Multiple instances of GlobalMonoBehaviourHandler detected with {GetType().Name}");
                return;
            }
            instance = this as T;
            OnAwake();
        }
        
        public virtual void OnAwake() {}

        private void OnDestroy()
        {
            if (instance != this)
            {
                return;
            }
            instance = null;
        }

        protected virtual void OnDestroy_Internal()
        {
            
        }
    }
}