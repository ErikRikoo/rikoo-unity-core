using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Script.Runtime.Core.GlobalHandlerManagement;
using UnityEngine;

namespace Script.Runtime.Core.Lifecycle
{
    public partial class LifecycleHandler
    {
        public static class Animation
        {
            public static async UniTask Animate(object owner, float duration, Action<object, float> animation, Action<object> onEnd = null)
            {
                float inverseDuration = 1 / duration;
                CancellationTokenSource cancellationSource = RunRoutineOnObject(owner);
                for (float time = 0; time < duration; time += Time.deltaTime)
                {
                    animation(owner, time * inverseDuration);
                    try
                    {
                        await UniTask.Yield(cancellationToken: cancellationSource.Token);
                    }
                    catch (OperationCanceledException _)
                    {
                        animation(owner, 1);
                        onEnd?.Invoke(owner);
                        StopRoutine(owner);
                        return;
                    }
                }
                animation(owner, 1);
                onEnd?.Invoke(owner);
                StopRoutine(owner);
            }

            public static async UniTask Animate(object target, float duration, Action<object, float> animation,
                CancellationToken token)
            {
                float inverseDuration = 1 / duration;
                for (float time = 0; time < duration; time += Time.deltaTime)
                {
                    animation(target, time * inverseDuration);
                    try
                    {
                        await UniTask.Yield(cancellationToken: token);
                    }
                    catch (OperationCanceledException exception)
                    {
                        animation(target, 1);
                        throw;
                    }
                }
            }
        }
    }
}