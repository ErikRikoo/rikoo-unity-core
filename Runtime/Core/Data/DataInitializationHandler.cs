using System.Collections.Generic;

namespace Rikoo.Core.Data
{
    public class DataInitializationHandler
    {
        private Dictionary<string, (object, OnDataInitialized)> dataHolder = new();
        
        public delegate void OnDataInitialized(object value);

        public bool TryGetDataDirect(string id, out object data)
        {
            if(!dataHolder.TryGetValue(id, out var tuple) || tuple.Item1 == null)
            {
                data = null;
                return false;
            }

            data = tuple.Item1;
            return true;
        }
        
        public void TryGetData(string id, OnDataInitialized callback)
        {
            if(!dataHolder.TryGetValue(id, out var tuple))
            {
                dataHolder[id] = (null, callback);
            } else if (tuple.Item1 == null)
            {
                tuple.Item2 += callback;
                return;
            }
            callback?.Invoke(tuple.Item1);            
        }

        public void SetData(string id, object data)
        {
            if (dataHolder.TryGetValue(id, out var tuple))
            {
                tuple.Item2?.Invoke(data);
            }
            dataHolder[id] = (data, null);
        }
    }
}