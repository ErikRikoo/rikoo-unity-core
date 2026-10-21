using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Rikoo.Core.Data
{
    [Serializable]
    public class ListConfigData
    {
        [HideInInspector]
        [SerializeField] private string guid;
        [SerializeField] private string  name;

        public string Guid => guid;

        public string Name => name;

        public void RefreshGUIDIfNeeded()
        {
            if (guid.IsNullOrWhitespace())
            {
                guid = GUID.Generate().ToString();
            }
        }
    }
    
    public class SOListConfig<U, T> : SOConfig<U>, IEnumerable<T> 
        where T : ListConfigData
        where U : SOListConfig<U, T>
    {
        [SerializeField] private List<T> data = new();

        [HideInInspector]
        [SerializeField] private string[] guid;
        [HideInInspector]
        [SerializeField] private string[] names;
        

        public int Count => data.Count;

        public T this[int index] => data[index];

        public T this[string guid]
        {
            get
            {
                foreach (T item in data)
                {
                    if (item.Guid == guid)
                    {
                        return item;
                    }
                }

                return null;
            }
        }
        
        
        public IEnumerator<T> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
        {
            return GetEnumerator();
        }

        [Button]
        public void RefreshInnerData()
        {
            foreach (var item in data)
            {
                item.RefreshGUIDIfNeeded();
            }

            if (names == null || names.Length != data.Count)
            {
                names = new string[data.Count];
            }
            if (guid == null || guid.Length != data.Count)
            {
                guid = new string[data.Count];
            }
            
            for (var i = 0; i < data.Count; i++)
            {
                names[i] = data[i].Name;
                guid[i] = data[i].Guid;
            }
        }

        public void GetPossibilities(out string[] guid, out string[] names)
        {
            guid = this.guid;
            names = this.names;
        }
    }
}