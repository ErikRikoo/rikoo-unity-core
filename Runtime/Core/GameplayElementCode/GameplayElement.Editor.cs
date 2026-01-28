using Rikoo.Core.GameplayElementCode.Lifecycle;
using Rikoo.Core.Notification;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Rikoo.Core.GameplayElementCode
{
    public partial class GameplayElement
    {
        #if UNITY_EDITOR
        [ButtonGroup("Setup"), Button, PropertyOrder(1)]
        public void FetchAll()
        {
            behaviours = GetComponentsInChildren<GameplayElementBehaviour>();
            if (!TryGetComponent(out updateHandler))
            {
                updateHandler = gameObject.AddComponent<GameplayElementUpdateHandler>();
            }
            updateHandler = GetComponent<GameplayElementUpdateHandler>();
            updateHandler.root = this;
            updateHandler.OnFetched();
            rigidbody = GetComponent<Rigidbody>();
            updatableBehaviours.Clear();
            fixedUpdatableBehaviours.Clear();
            notifiableBehaviours.Clear();
            foreach (var behaviour in behaviours)
            {
                behaviour.root = this;
                behaviour.OnFetched();
                if (behaviour is IUpdatableFlag)
                {
                    updatableBehaviours.Add(behaviour);
                }
                if (behaviour is IFixedUpdatableFlag)
                {
                    fixedUpdatableBehaviours.Add(behaviour);
                }

                if (behaviour is INotificationReceiver)
                {
                    notifiableBehaviours.Add(behaviour);
                }
            }
            updatableBehaviours.Sort((first, second) 
                    => (first as IUpdatableFlag).UpdateOrder.CompareTo((second as IUpdatableFlag).UpdateOrder)
                );
        }

        [ButtonGroup("Setup"), Button, PropertyOrder(1)]
        public void EasySetup()
        {
            FetchAll();
            GameObject root = new("Root");
            root.transform.parent = transform;
            GameObject visual = new("Visual");
            visual.transform.parent = root.transform;
            artHolder = visual.transform;
            GameObject behaviors = new("Behaviors");
            behaviors.transform.parent = root.transform;
        }

        [OnInspectorGUI, PropertyOrder(2)]
        public void GameplayElementStateInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            EditorGUILayout.BeginHorizontal();
            var buttonStyle = new GUIStyle(GUI.skin.button);
            IsVisible = GUILayout.Toggle(IsVisible, "Visible", buttonStyle);
            IsUpdated = GUILayout.Toggle(IsUpdated, "Updated", buttonStyle);
            EditorGUILayout.EndHorizontal();
        }
        
        [FoldoutGroup("Behaviour Debug"), OnInspectorGUI, PropertyOrder(0)]
        public void GameplayElementStateInspectorGUI2()
        {
            foreach (var behaviour in behaviours)
            {
                if (behaviour.HasCustomDebugData && behaviour.IsInlinedEditor)
                {
                    behaviour.DrawDebugEditor();
                }
            }
            
            foreach (var behaviour in behaviours)
            {
                if (behaviour.HasCustomDebugData && !behaviour.IsInlinedEditor)
                {
                    behaviour.isEditorExpanded = SirenixEditorGUI.Foldout(behaviour.isEditorExpanded, behaviour.GetType().Name);
                    if (!behaviour.isEditorExpanded)
                    {
                        continue;
                    }
                    ++EditorGUI.indentLevel;
                    behaviour.DrawDebugEditor();
                    --EditorGUI.indentLevel;
                }
            }
        }
#endif
    }
}