using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rikoo.Framework.Core.Utilities;
using Rikoo.Framework.Utilities.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UIElements.Toggle;

namespace Rikoo.Framework.UI
{
    [UxmlElement]
    public partial class ReadOnlySearchableListView : BoxGroup
    {
        private List<int> filteredIndexes = new();
        private SerializedProperty boundProperty;
        
        private bool areElementEnabled;

        [UxmlAttribute]
        public bool AreElementEnabled
        {
            get => areElementEnabled;
            set
            {
                areElementEnabled = value;
                // TODO: Update all child
            }
        }
        
        private string SearchableListViewClassName = "searchable-list-view";
        private string HeaderClassName = "header";
        private string HeaderPartClassName = "header-part";
        private string ListClassName = "list-view";
        
        private string currentSearch;
        
        private ListView list;
        private TextField searchField;
        private Label count;
        
        public Editor Owner;
        
        public ReadOnlySearchableListView()
        {
            content.AppendChild(ref list, "list", ListClassName);
            VisualElement extraInfo = null;
            headerToggle.AddToClassList("width-50");
            header.AppendChild(ref extraInfo, "extra-info", "width-50");
            extraInfo.AppendChild(ref searchField, "search", "width-75");
            extraInfo.AppendChild(ref count, "count", "text-right");
            count.AddToClassList("text-middle");
            count.AddToClassList("flex-grow");

            searchField.RegisterValueChangedCallback(OnSearchChanged);
        }

        private void OnSearchChanged(ChangeEvent<string> evt)
        {
            FilterList();
            UpdateCountLabel();
        }

        public void BindProperty(SerializedProperty property)
        {
            boundProperty = property;
            BindHeaderToggle(boundProperty);
            count.TrackPropertyValue(boundProperty, OnPropertyChanged);
            BindList();
            FilterList();
            UpdateCountLabel();
        }

        private void OnPropertyChanged(SerializedProperty obj)
        {
            UpdateCountLabel();
            FilterList();
        }

        private void FilterList()
        {
            filteredIndexes.Clear();
            if (boundProperty == null)
            {
                list.RefreshItems();
                return;
            }
            if (searchField.value == "")
            {
                filteredIndexes.AddRange(Enumerable.Range(0, boundProperty.arraySize));
                list.RefreshItems();
                return;
            }

            for (int i = 0; i < boundProperty.arraySize; i++)
            {
                var prop = boundProperty.GetArrayElementAtIndex(i);
                if (prop.objectReferenceValue is not IFilterable filterable)
                {
                    continue;
                }

                if (filterable.IsMatch(searchField.value))
                {
                    filteredIndexes.Add(i);
                }
            }
            list.RefreshItems();
        }

        private void BindList()
        {
            list.reorderable = false;
            list.showAddRemoveFooter = false;
            list.showBoundCollectionSize = false;
            list.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

            list.makeItem = () => new InlineObjectField();
            list.bindItem = (element, index) =>
            {
                var prop = boundProperty.GetArrayElementAtIndex(filteredIndexes[index]);
                InlineObjectField field = (InlineObjectField)element;
                field.Owner = Owner;
                field.CanUserModifyFieldValue = false;
                field.BindProperty(prop);
            };
            list.itemsSource = filteredIndexes;
        }

        public void UpdateCountLabel()
        {
            count.text = $"{filteredIndexes.Count} items";
        }
    }
}