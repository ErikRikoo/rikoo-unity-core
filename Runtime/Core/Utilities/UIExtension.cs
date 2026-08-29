using UnityEngine.UIElements;

namespace Rikoo.Framework.Core.Utilities
{
    public static class UIExtension
    {
        public static string InvisibleClassName = "hidden";
        
        public static void ChangeDisplayState(this VisualElement instance, bool isVisible)
        {
            if (isVisible)
            {
                instance.RemoveFromClassList(InvisibleClassName);
            }
            else
            {
                instance.AddToClassList(InvisibleClassName);
            }
        }
    }
}