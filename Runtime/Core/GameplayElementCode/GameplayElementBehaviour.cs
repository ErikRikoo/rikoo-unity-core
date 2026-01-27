using NUnit.Framework.Constraints;
using Script.Runtime.Core.Notification;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core.GameplayElementCode
{
    public class GameplayElementBehaviour : MonoBehaviour, ISearchFilterable
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
            string typeName = GetType().Name.ToLower();
            return typeName.Contains(searchString.ToLower());
        }
    }
}