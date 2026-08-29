using System.Linq;
using Rikoo.Framework.Core.Utilities;
using UnityEngine;

namespace Rikoo.Core.GameplayElementCode
{
    public class GameplayElementBehaviour : MonoBehaviour, IFilterable
    {
        [HideInInspector]
        public GameplayElement root;

        public int ID => root.ID;

        public string GPEName => root.gameObject.name;
        
        public virtual void OnUpdate() {}
        public virtual void OnFixedUpdate() {}
        
        public virtual void OnFetched() {}

        public virtual bool HasCustomDebugData => false;
        public virtual bool IsInlinedEditor => false;
        
        public virtual void DrawDebugEditor() {}

        [HideInInspector]
        public bool isEditorExpanded;
        
        public bool IsMatch(string searchString)
        {
            string typeName = GetType().Name;

            return typeName.ToLower().StartsWith(searchString.ToLower()) ||
                   typeName.SplitWords().Any(s => s.ToLower().StartsWith(searchString.ToLower()));
        }
    }
}