using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Rikoo.Framework.Utilities.UI
{
    public static class VisualElementExtension
    {
        public static T AppendChild<T>(this VisualElement parent, ref T variable, string childName = null, string className = null, bool contentAdd = false )
            where T : VisualElement, new()
        {
            variable = new T();
            if (childName != null)
            {
                variable.name = childName;
            }
            if (className != null)
            {
                variable.AddToClassList(className);
            }

            if (contentAdd)
            {
                parent.Add(variable);
            }
            else
            {
                parent.hierarchy.Add(variable);
            }

            return variable;
        }
        
        public static T AppendDeclareChild<T>(this VisualElement parent, out T variable, string childName = null, string className = null, bool contentAdd = false )
            where T : VisualElement, new()
        {
            variable = new T();
            if (childName != null)
            {
                variable.name = childName;
            }
            if (className != null)
            {
                variable.AddToClassList(className);
            }
            if (contentAdd)
            {
                parent.Add(variable);
            }
            else
            {
                parent.hierarchy.Add(variable);
            }
            return variable;
        }
        
        public static T AppendChild<T>(this VisualElement parent, ref T variable, params string[] classNames)
            where T : VisualElement, new()
            => AppendChild(parent, ref variable, childName: null, classNames: classNames);

        public static T AppendChild<T>(this VisualElement parent, ref T variable, string childName = null, bool contentAdd = false, params string[] classNames)
            where T : VisualElement, new()
        {
            variable = new T();
            if (childName != null)
            {
                variable.name = childName;
            }
            for (var i = 0; i < classNames.Length; i++)
            {
                variable.AddToClassList(classNames[i]);
            }

            if (contentAdd)
            {
                parent.Add(variable);
            }
            else
            {
                parent.hierarchy.Add(variable);
            }
            return variable;
        }

        public static T AppendDeclareChild<T>(this VisualElement parent, out T variable, params string[] classNames)
            where T : VisualElement, new()
            => AppendDeclareChild(parent, out variable, childName: null, classNames: classNames);

        public static T AppendDeclareChild<T>(this VisualElement parent, out T variable, string childName = null, bool contentAdd = true, params string[] classNames)
            where T : VisualElement, new()
        {
            T element = null;
            AppendChild(parent, ref element, childName, contentAdd, classNames);
            variable = element;
            return variable;
        }
        
        public static VisualElement ChangeDisplayed(this VisualElement element, bool displayed)
        {
            element.style.display = displayed ? DisplayStyle.Flex : DisplayStyle.None;
            return element;
        }

        public static VisualElement ChangeCanBePicked(this VisualElement element, bool canBePicked)
        {
            element.pickingMode = canBePicked ? PickingMode.Position : PickingMode.Ignore;
            return element;
        }
        
        public static VisualElement ReactToMouseHover(this VisualElement element, Action<bool> onHoverChanged)
        {
            element.RegisterCallback<MouseOverEvent>((_) => onHoverChanged(true));
            element.RegisterCallback<MouseOutEvent>((_) => onHoverChanged(false));

            return element;
        }

        public static VisualElement WithClass(this VisualElement element, params string[] classNames)
        {
            foreach (var className in classNames)
            {
                element.AddToClassList(className);
            }

            return element;
        }

        public static VisualElement TrackPropertyValueAndCall(
            this VisualElement instance, 
            SerializedProperty property,
            Action<SerializedProperty> callback)
        {
            instance.TrackPropertyValue(property, callback);
            callback?.Invoke(property);
            return instance;
        }
        
        public static VisualElement TrackSerializedObjectValueAndCall(
            this VisualElement instance, 
            SerializedObject serializedObject,
            Action<SerializedObject> callback)
        {
            instance.TrackSerializedObjectValue(serializedObject, callback);
            callback?.Invoke(serializedObject);
            return instance;
        }

        public static VisualElement WithSize(this VisualElement instance, float value,
            LengthUnit unit = LengthUnit.Pixel) => WithSize(instance, new Vector2(value, value), unit);
        
        public static VisualElement WithSize(this VisualElement instance, Vector2 value,
            LengthUnit unit = LengthUnit.Pixel)
        {
            instance.style.height = new Length(value.y, unit);
            instance.style.width = new Length(value.x, unit);
            return instance;
        }
        
        public static VisualElement SetPaddingTo0(this VisualElement element)
        {
            element.style.paddingBottom = 0;
            element.style.paddingTop = 0;
            element.style.paddingLeft = 0;
            element.style.paddingRight = 0;
            return element;
        }
    }
}