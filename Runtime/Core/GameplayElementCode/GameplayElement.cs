using System.Collections.Generic;
using Rikoo.Core.Data;
using Rikoo.Core.GameplayElementCode.Lifecycle;
using Rikoo.Core.Notification;
using UnityEngine;

namespace Rikoo.Core.GameplayElementCode
{
    public partial class GameplayElement : MonoBehaviour
    {
        [SerializeField] private GameplayElementBehaviour[] behaviours;
        [SerializeField] private Transform artHolder;

        [SerializeField] private List<GameplayElementBehaviour> updatableBehaviours  = new();
        [SerializeField] private List<GameplayElementBehaviour> fixedUpdatableBehaviours  = new();
        [SerializeField] private List<GameplayElementBehaviour> notifiableBehaviours  = new();
        [SerializeField] private GameplayElementUpdateHandler updateHandler;
        [SerializeField] private Rigidbody rigidbody;
        
        private GameNotificationHandler notificationHandler = new();
        private DataInitializationHandler dataHandler = new();

        public Transform ArtHolder => artHolder;

        public Rigidbody Rigidbody => rigidbody;

        public GameplayElementBehaviour[] Behaviours => behaviours;

        public List<GameplayElementBehaviour> UpdatableBehaviours => updatableBehaviours;

        public List<GameplayElementBehaviour> FixedUpdatableBehaviours => fixedUpdatableBehaviours;

        public List<GameplayElementBehaviour> NotifiableBehaviours => notifiableBehaviours;

        public EntityId ID => GetEntityId();

        public bool IsVisible
        {
            get => artHolder.gameObject.activeSelf;
            set => artHolder.gameObject.SetActive(value);
        }

        public bool IsUpdated
        {
            get => updateHandler.enabled;
            set => updateHandler.enabled = value;
        }
        
        private void OnEnable()
        {
            foreach (var behaviour in notifiableBehaviours)
            {
                if (!behaviour.enabled)
                {
                    continue;
                }
                notificationHandler.ChangeNotifiableState((behaviour as INotificationReceiver).ListenedType, true);
                GlobalNotificationHandler.ChangeNotifiableState((behaviour as INotificationReceiver).GlobalListenedType, true);
            }
        }
        
        private void OnDisable()
        {
            foreach (var behaviour in notifiableBehaviours)
            {
                if (!behaviour.enabled)
                {
                    continue;
                }
                notificationHandler.ChangeNotifiableState((behaviour as INotificationReceiver).ListenedType, false);
                GlobalNotificationHandler.ChangeNotifiableState((behaviour as INotificationReceiver).GlobalListenedType, false);
            }
        }
        
        public void UpdateBehaviors()
        {
            foreach (var behaviour in UpdatableBehaviours)
            {
                behaviour.OnUpdate();
            }
        }
        
        public void FixedUpdateBehaviors()
        {
            foreach (var behaviour in FixedUpdatableBehaviours)
            {
                behaviour.OnFixedUpdate();
            }
        }

        public bool TryGetBehavior<T>(out T behavior) where T : GameplayElementBehaviour
        {
            foreach (GameplayElementBehaviour iteratedBehaviour in behaviours)
            {
                if (iteratedBehaviour is not T castedBehavior)
                {
                    continue;
                }
                behavior = castedBehavior;
                return true;
            }

            behavior = null;
            return false;
        }
        
        public bool TryGetBehaviorByRef<T>(ref T behavior) where T : GameplayElementBehaviour
        {
            foreach (GameplayElementBehaviour iteratedBehaviour in behaviours)
            {
                if (iteratedBehaviour is not T castedBehavior)
                {
                    continue;
                }
                behavior = castedBehavior;
                return true;
            }

            behavior = null;
            return false;
        }
        
        public bool TryGetInterfaceByRef<T>(ref T behavior)
        {
            foreach (GameplayElementBehaviour iteratedBehaviour in behaviours)
            {
                if (iteratedBehaviour is not T castedBehavior)
                {
                    continue;
                }
                behavior = castedBehavior;
                return true;
            }

            behavior = default(T);
            return false;
        }
        
        public void GetInterfacesAsBehaviours<T>(List<GameplayElementBehaviour> items)
        {
            items.Clear();
            foreach (GameplayElementBehaviour iteratedBehaviour in behaviours)
            {
                if (iteratedBehaviour is not T castedBehavior)
                {
                    continue;
                }
                items.Add(iteratedBehaviour);
            }
        }
        
        public void RaiseNotification<T>(string type, T notification, MonoBehaviour sender) where T : IGameNotification
        {
            notificationHandler.RaiseNotification(type, notification, sender, false);
        }
        
        public void RaiseNotification<T>(T notification, MonoBehaviour sender) where T : IGameNotification
        {
            notificationHandler.RaiseNotification(notification, sender, false);
        }
        
        public void RaiseNotification<T>(MonoBehaviour sender) where T : IGameNotification, new()
        {
            notificationHandler.RaiseNotification<T>(sender, false);
        }

        public void RaiseErrorDirect(string label, MonoBehaviour sender,
            object errorData = null)
        {
            notificationHandler.RaiseNotification(
                label, 
                new ErrorNotification(errorData), sender, false
            );
        }
        
        public void RaiseError(string errorType, MonoBehaviour sender, string errorReason = "*",
            object errorData = null)
        {
            notificationHandler.RaiseNotification(
                ErrorNotification.MakeLabel(errorType, errorReason), 
                new ErrorNotification(errorData), sender, false
                );
        }

        public bool TryGetDataDirect(string id, out object data) => dataHandler.TryGetDataDirect(id, out data);
        
        public void TryGetData(string id, DataInitializationHandler.OnDataInitialized callback)
        {
            dataHandler.TryGetData(id, callback);
        }

        public void SetData(string id, object data)
        {
            dataHandler.SetData(id, data);
        }
    }
}