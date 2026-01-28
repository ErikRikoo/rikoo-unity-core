using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Rikoo.Core.GlobalHandlerManagement;
using UnityEngine;

namespace Rikoo.Core.Lifecycle
{
    public partial class LifecycleHandler : GlobalMonoBehaviourHandler<LifecycleHandler>, IStaticHandler
    {
        private Dictionary<object, CancellationTokenSource> runningRoutines = new();

        private static Dictionary<object, CancellationTokenSource> RunningRoutines => Instance.runningRoutines;
        
        public static async UniTask RunInSequence<T>(object owner, IReadOnlyList<T> instances, float delay,
            Action<T, int> action, Action sequenceStartsDone = null)
        {
            // todo: Handle cancel
            CancellationTokenSource cancellationSource = RunRoutineOnObject(owner);
            for (var i = 0; i < instances.Count; i++)
            {
                var letterInstance = instances[i];
                action(letterInstance, i);
                await UniTask.WaitForSeconds(delay, cancellationToken: cancellationSource.Token).SuppressCancellationThrow();
            }
            sequenceStartsDone?.Invoke();
            StopRoutine(owner);
        }
        
        public static async UniTask RunInSequenceReverse<T>(object owner, IReadOnlyList<T> instances, float delay,
            Action<T, int> action, Action sequenceStartsDone = null)
        {
            // todo: Handle cancel
            CancellationTokenSource cancellationSource = RunRoutineOnObject(owner);
            for (var i = instances.Count - 1; i >= 0; i--)
            {
                var letterInstance = instances[i];
                action(letterInstance, i);
                await UniTask.WaitForSeconds(delay, cancellationToken: cancellationSource.Token).SuppressCancellationThrow();
            }
            sequenceStartsDone?.Invoke();
            StopRoutine(owner);
        }

        private static void StopRoutine(object owner)
        {
            if (RunningRoutines.TryGetValue(owner, out CancellationTokenSource source))
            {
                source.Cancel();
            }
        }

        private static  CancellationTokenSource RunRoutineOnObject(object owner)
        {
            if (RunningRoutines.TryGetValue(owner, out CancellationTokenSource oldSource))
            {
                oldSource.Cancel();
            }

            CancellationTokenSource source = new();
            RunningRoutines[owner] = source;
            return source;
        }

        protected  override void OnDestroy_Internal()
        {
            foreach (var source in runningRoutines.Values)
            {
                source.Cancel();
            }
        }

        public static async UniTask RunNextFrame(object owner, Action<object> action)
        {
            CancellationTokenSource cancellationSource = RunRoutineOnObject(owner);
            try
            {
                await UniTask.Yield(cancellationToken: cancellationSource.Token);
            }
            catch (OperationCanceledException _)
            {
                StopRoutine(owner);
                return;
            }
            action?.Invoke(owner);                
            StopRoutine(owner);
        }
        
        public static async UniTask RunAfterFrames(object owner, int frameCount, Action<object> action)
        {
            CancellationTokenSource cancellationSource = RunRoutineOnObject(owner);
            try
            {
                for (int i = 0; i < frameCount; i++)
                {
                    await UniTask.Yield(cancellationToken: cancellationSource.Token);
                }
            }
            catch (OperationCanceledException _)
            {
                StopRoutine(owner);
                return;
            }
            action?.Invoke(owner);                
            StopRoutine(owner);
        }

        public static async UniTask RunRoutine(object owner, params Func<CancellationToken, UniTask>[] routines)
        {
            CancellationTokenSource cancellationSource = RunRoutineOnObject(owner);
            try
            {
                foreach (var routine in routines)
                {
                    await routine(cancellationSource.Token);
                }
            }
            catch (OperationCanceledException _)
            {
                Debug.Log($"Cancelling routine for {owner}");
            }
            finally
            {
                StopRoutine(owner);
            }
        }
    }
}