using System;
using System.Collections.Generic;
using Rikoo.Core.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Rikoo.Framework.Core.TraumaSystem
{
    [Serializable]
    public class TraumaReactInstance
    {
        [SerializeField] private ListConfigItemReference<TraumaItemList, TraumaItem> traumaConfig;
        [SerializeField] private float min;
        [SerializeField] private float max;


        public void Bind(TraumaItemInstance.OnValueChangedType onValueChanged)
        {
            TraumaSystem.Bind(traumaConfig.Guid, onValueChanged);
        }
        
        public void Unbind(TraumaItemInstance.OnValueChangedType onValueChanged)
        {
            TraumaSystem.Unbind(traumaConfig.Guid, onValueChanged);
        }

        [Button]
        public void AddTrauma(float value)
        {
            TraumaSystem.AddTrauma(traumaConfig.Guid, value);
        }
    }
    
    public class TraumaReact : MonoBehaviour
    {
        // TODO: Handle when multiple are changed at once (additive, multiply, etc.)
        [SerializeField] private List<TraumaReactInstance> instances = new();

        private void Awake()
        {
            foreach (var traumaReactInstance in instances)
            {
                traumaReactInstance.Bind(OnValueChanged);
            }
        }

        private void OnDestroy()
        {
            foreach (var traumaReactInstance in instances)
            {
                traumaReactInstance.Unbind(OnValueChanged);
            }
        }

        public void OnValueChanged(float linear, float interpolated)
        {
            Debug.Log($"On Value Chanded {linear} - {interpolated}.");
        }
    }
}