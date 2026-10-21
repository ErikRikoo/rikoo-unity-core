using System;
using System.Collections.Generic;
using Rikoo.Core.Lifecycle;
using UnityEngine;

namespace Rikoo.Framework.Core.TraumaSystem
{
    public enum InterpolationType
    {
        Square, Cubic,
    }

    public static class InterpolationTypeExtension
    {
        public static float Evaluate(this InterpolationType instance, float value) => instance switch
        {
            InterpolationType.Square => value * value,
            InterpolationType.Cubic => value * value * value,
            _ => value
        };
    }
    
    public class TraumaItemInstance
    {
        public delegate void OnValueChangedType(float linear, float interpolated);
        
        public TraumaItem Data;
        public float TraumaValue;
        public float InterpolatedValue;
        public OnValueChangedType OnValueChanged;

        public bool HasChanged;
    }
    
    public static class TraumaSystem
    {
        private static Dictionary<string, TraumaItemInstance> instances;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void InitializeData()
        {
            instances = new(TraumaItemList.Instance.Count);
            for (var i = 0; i < TraumaItemList.Instance.Count; i++)
            {
                instances[TraumaItemList.Instance[i].Guid] = new TraumaItemInstance()
                {
                    Data = TraumaItemList.Instance[i], 
                };
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeUpdate()
        {
            UnityLifecycleHook.OnUpdate += OnUpdate;
            UnityLifecycleHook.OnLateUpdate += OnLateUpdate;
        }

        private static void OnUpdate()
        {
            foreach (var (_, value) in instances)
            {
                Tick(value);
            }
        }
        
        private static void OnLateUpdate()
        {
            foreach (var (_, value) in instances)
            {
                if (value.HasChanged)
                {
                    value.OnValueChanged?.Invoke(value.TraumaValue, value.InterpolatedValue);
                    value.HasChanged = false;
                }
            }
        }

        private static void Tick(TraumaItemInstance instance)
        {
            float oldValue = instance.TraumaValue;
            instance.TraumaValue =
                instance.Data.ClampIfNeeded(instance.TraumaValue - instance.Data.DecreaseRate * Time.deltaTime);
            if (!Mathf.Approximately(oldValue, instance.TraumaValue))
            {
                instance.InterpolatedValue = instance.Data.Interpolate(instance.TraumaValue);
                instance.HasChanged = true;
            }
        }

        public static void Bind(string guid, TraumaItemInstance.OnValueChangedType onValueChanged)
        {
            instances[guid].OnValueChanged += onValueChanged;
        }

        public static void Unbind(string guid, TraumaItemInstance.OnValueChangedType onValueChanged)
        {
            instances[guid].OnValueChanged -= onValueChanged;
        }

        public static void AddTrauma(string guid, float value)
        {
            instances[guid].TraumaValue = instances[guid].Data.ClampIfNeeded(instances[guid].TraumaValue + value);
            instances[guid].HasChanged = true;
        }

        public static void Read(string guid, out float linear, out float interpolated)
        {
            linear = instances[guid].TraumaValue;
            interpolated = instances[guid].InterpolatedValue;
        }
        
        public static void ClearTrauma(string guid)
        {
            instances[guid].TraumaValue = 0;
        }
    }
}