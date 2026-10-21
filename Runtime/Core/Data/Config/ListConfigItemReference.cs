using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rikoo.Core.Data
{
    public interface IListConfigItemRef
    {
        bool HasConfig
        {
            get;
        }

        void CreateConfig();


        void GetPossibilities(out string[] guid, out string[] names);
    }
    
    [Serializable]
    public class ListConfigItemReference<T, U> : IListConfigItemRef
        where U : ListConfigData
        where T : SOListConfig<T, U>
    {
        [SerializeField] private string guid;

        public string Guid => guid;
        
        private U value;

        public U Value
        {
            get
            {
                Load();
                return UnsafeValue;
            }
        }

        public U UnsafeValue => value;

        public void Load()
        {
            if (value == null)
            {
                value = SOConfig<T>.Instance[guid];
            }
        }

        public bool HasConfig => SOConfigUtilities.ChecksExistence<T>();
        public void CreateConfig()
        {
            SOConfigUtilities.GetOrCreate<T>();
        }
        
        public void GetPossibilities(out string[] guid, out string[] names)
        {
            SOListConfig<T, U>.Instance.GetPossibilities(out guid, out names);
        }

    }
}