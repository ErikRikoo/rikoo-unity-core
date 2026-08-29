using Rikoo.Core.GameplayElementCode;
using Rikoo.Framework.UI;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.GameplayElementCode
{
    [CustomEditor(typeof(GameplayElement))]
    public class GameplayElementEditor : Editor
    {
        private static string behaviorListName = "BehaviorList";
        private static string updatableBehaviorListName = "UpdatableBehaviors";
        private static string fixedUpdatableBehaviorListName = "FixedUpdatableBehaviors";
        private static string notifiableBehaviorListName = "NotifiableBehaviors";
        private static string updateHandlerName = "UpdateHandlerField";
        private static string rigidBodyName = "RigidbodyField";
        private static string artHolderName = "ArtHolderField";
        private static string fetchAllButtonName = "FetchAllButton";
        private static string easySetupButtonName = "EasySetupButton";
        
        [SerializeField] private VisualTreeAsset ui;
        
        private GameplayElement Target => target as GameplayElement;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = ui.CloneTree();

            LookForAndBindList(root, behaviorListName, GameplayElement.BehaviorFieldName);
            LookForAndBindList(root, updatableBehaviorListName, GameplayElement.UpdatableBehaviorFieldName);
            LookForAndBindList(root, fixedUpdatableBehaviorListName, GameplayElement.FixedUpdatableBehaviorFieldName);
            LookForAndBindList(root, notifiableBehaviorListName, GameplayElement.NotifiableBehaviorFieldName);

            LookForAndBindObjectField(root, rigidBodyName, GameplayElement.RigidbodyFieldName);
            LookForAndBindObjectField(root, updateHandlerName, GameplayElement.UpdateHandlerFieldName);
            LookForAndBindObjectField(root, artHolderName, GameplayElement.ArtHolderFieldName);

            root.Q<Button>(fetchAllButtonName).clicked += OnFetchAll;
            root.Q<Button>(easySetupButtonName).clicked += OnEasySetup;
            
            return root;
        }

        private void LookForAndBindObjectField(VisualElement root, string elementName, string fieldName)
        {
            var objectField = root.Q<ObjectField>(elementName);
            SerializedProperty property = serializedObject.FindProperty(fieldName);
            objectField.label = property.displayName;
            objectField.BindProperty(property);
        }

        private void LookForAndBindList(VisualElement root, string elementName, string fieldName)
        {
            var list = root.Q<ReadOnlySearchableListView>(elementName);
            list.Owner = this;
            list.BindProperty(serializedObject.FindProperty(fieldName));
        } 

        private void OnFetchAll()
        {
            Target.FetchAll();
            EditorUtility.SetDirty(Target);
            serializedObject.Update();
        }
        
        private void OnEasySetup()
        {
            Target.EasySetup();
            EditorUtility.SetDirty(Target);
        }
    }
}