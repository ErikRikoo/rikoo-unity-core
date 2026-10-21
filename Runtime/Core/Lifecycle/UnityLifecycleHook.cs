using System;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace Rikoo.Core.Lifecycle
{
    [AutoStaticsCleanup]
    public partial class UnityLifecycleHook : MonoBehaviour
    {
        public static Action OnUpdate;
        public static Action OnLateUpdate;
        
        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}