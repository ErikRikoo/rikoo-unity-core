using System.Collections.Generic;
using Script.Runtime.Core.GlobalHandlerManagement;
using UnityEngine;

namespace Script.Runtime.Core.Notification
{
    public class GlobalNotificationHandler : GlobalHandler<GlobalNotificationHandler>, IStaticHandler
    {
        private GameNotificationHandler notificationHandler = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            GlobalNotificationHandler used = new();
            Debug.Log($"GlobalNotificationHandler initialization, instance is {Instance}.");
        }
        
        public static void RaiseNotification<T>(string type, T notification, MonoBehaviour sender) where T : IGameNotification
        {
            Instance.notificationHandler.RaiseNotification(type, notification, sender, true);
        }
        
        public static void RaiseNotification<T>(T notification, MonoBehaviour sender) where T : IGameNotification
        {
            Instance.notificationHandler.RaiseNotification(notification, sender, false);
        }
        
        public static void RaiseNotification<T>(MonoBehaviour sender) where T : IGameNotification, new()
        {
            Instance.notificationHandler.RaiseNotification<T>(sender, false);
        }
        
        public static void ChangeNotifiableState(IEnumerable<(string, INotificationReceiver.OnNotificationReceived)> notifiables, bool state)
        {
            foreach ((string type, INotificationReceiver.OnNotificationReceived notifiable) in notifiables)
            {
                ChangeNotifiableState(type, notifiable, state);
            }
        }
        
        public static void ChangeNotifiableState(string type, INotificationReceiver.OnNotificationReceived notifiable, bool state)
        {
            if (state)
            {
                RegisterNotifiable(type, notifiable);
            }
            else
            {
                UnregisterNotifiable(type, notifiable);
            }
        }
        
        public static void RegisterNotifiable(string type, INotificationReceiver.OnNotificationReceived notifiable)
        {
            Instance.notificationHandler.RegisterNotifiable(type, notifiable);
        }

        public static void UnregisterNotifiable(string type, INotificationReceiver.OnNotificationReceived notifiable)
        {
            Instance.notificationHandler.UnregisterNotifiable(type, notifiable);
        }
    }
}