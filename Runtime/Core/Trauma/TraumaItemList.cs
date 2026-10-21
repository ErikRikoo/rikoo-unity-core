using System;
using Rikoo.Core.Data;
using UnityEngine;

namespace Rikoo.Framework.Core.TraumaSystem
{
    [Serializable]
    public class TraumaItem : ListConfigData
    {
        [SerializeField] private float decreaseRate;
        [SerializeField] private InterpolationType interpolationType;

        public float DecreaseRate => decreaseRate;
        public InterpolationType InterpolationType => interpolationType;

        [SerializeField] private bool shouldClamp = true;
        [SerializeField] private float clampMin;
        [SerializeField] private float clampMax = 1;

        public float ClampIfNeeded(float value) => shouldClamp switch {
            true => Mathf.Clamp(value, clampMin, clampMax),
            false => value,
        };

        public float Interpolate(float value) => interpolationType.Evaluate(value);
    }
    
    [CreateAssetMenu(fileName = nameof(TraumaItemList), menuName = "Rikoo/Core/Trauma/Element List", order = 0)]
    public class TraumaItemList : SOListConfig<TraumaItemList, TraumaItem>
    {
        
    }
}