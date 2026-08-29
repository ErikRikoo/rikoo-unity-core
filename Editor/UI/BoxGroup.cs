using Rikoo.Framework.Core.Utilities;
using Rikoo.Framework.Utilities.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Rikoo.Framework.UI
{
    [UxmlElement]
    public partial class BoxGroup : VisualElement
    {
        private string label;

        [UxmlAttribute]
        public string Label
        {
            get => label;
            set
            {
                label = value;
                headerToggle.text = value;
            }
        }

        private bool isExpanded;

        [UxmlAttribute]
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnVisibilityChanged(value);
            }
        }

        private bool isContentValid = true;

        public bool IsContentValid
        {
            get => isContentValid;
            set
            {
                isContentValid = value;
                OnIsContentValidChanged();
            }
        }

        private void OnIsContentValidChanged()
        {
            OnVisibilityChanged(isContentValid && isExpanded);
            headerToggle.value = isContentValid && isExpanded;
            
            // Move to extension function
            // TODO: Gray out the "arrow"
            headerToggle.pickingMode = isContentValid ? PickingMode.Position : PickingMode.Ignore;
            headerToggle[0].pickingMode = isContentValid ? PickingMode.Position : PickingMode.Ignore;
            headerToggle[0].visible = isContentValid;
        }

        protected VisualElement header;
        protected Toggle headerToggle;
        protected VisualElement content;

        public BoxGroup()
        {
            AddToClassList("BoxGroup");
            this.AppendChild(ref header, "header", "header");
            header.AppendChild(ref headerToggle, "toggle", "unity-foldout__toggle");
            this.AppendChild(ref content, "content", "Content");

            headerToggle.RegisterCallback<ChangeEvent<bool>>(OnVisibilityChanged);
            OnIsContentValidChanged();
        }

        private void OnVisibilityChanged(ChangeEvent<bool> evt)
        {
            IsExpanded = evt.newValue;
        }
        
        private void OnVisibilityChanged(bool newVisibility)
        {
            content.ChangeDisplayState(newVisibility);
        }

        public void BindHeaderToggle(SerializedProperty property)
        {
            headerToggle.text = property.displayName;
            headerToggle.SetValueWithoutNotify(property.isExpanded);
            headerToggle.RegisterValueChangedCallback(evt =>
            {
                property.isExpanded = evt.newValue;
                property.serializedObject.ApplyModifiedProperties();
            });
            
            headerToggle.TrackPropertyValue(property, p =>
            {
                headerToggle.SetValueWithoutNotify(p.isExpanded);
            });
            
        }


        public override VisualElement contentContainer => content;
    }
}