using System;
using Rikoo.Framework.Utilities.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Rikoo.Framework.UI
{
    [UxmlElement]
    public partial class InlineObjectField : BoxGroup
    {
        private bool canUserModifyFieldValue = true;
        
        public bool CanUserModifyFieldValue
        {
            get => canUserModifyFieldValue;
            set
            {
                canUserModifyFieldValue = value;
                // TODO: Move to extension method
                field.Q(className: "unity-object-field__input").SetEnabled(canUserModifyFieldValue);
            }
        }
        
        private const string scriptFieldName = "m_Script";
        
        private Type type;

        [UxmlAttribute]
        public Type Type
        {
            get => type;
            set
            {
                type = value;
                field.objectType = value;
            }
        }

        public Editor Owner;
        private VisualElement headerContainer;
        private ObjectField field;
        private bool hasVisibleProperties = true;
        
        public InlineObjectField()
        {
            header.AppendChild(ref headerContainer, "extra-info", "header-container");
            headerContainer.AppendChild(ref field, "ObjectField", "flex-grow");
            field.AddToClassList("object-field");
            headerToggle.style.flexGrow = new StyleFloat(0.0f);
            
            field.RegisterValueChangedCallback(OnValueChanged);
            OnValueChanged();
        }

        private void OnValueChanged(ChangeEvent<Object> evt)
        {
            if (Owner != null && content.childCount == 0)
            {
                SerializedObject serializedObject = new SerializedObject(evt.newValue);
                if (serializedObject.HasVisibleProperties(scriptFieldName))
                {
                    hasVisibleProperties = true;
                    InspectorElement.FillDefaultInspector(content, new SerializedObject(evt.newValue), Owner, scriptFieldName);
                    content.Bind(new SerializedObject(evt.newValue));
                }
                else
                {
                    hasVisibleProperties = false;
                }
            }
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            IsContentValid = field.value != null && hasVisibleProperties;
        }

        public void BindProperty(SerializedProperty property)
        {
            field.label = property.displayName;
            field.BindProperty(property);
            field.value = property.objectReferenceValue;
            type = field.objectType;
        }
    }
}