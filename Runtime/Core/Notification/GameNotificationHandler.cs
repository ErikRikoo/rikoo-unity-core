using System.Collections.Generic;
using UnityEngine;

namespace Rikoo.Core.Notification
{
    public class GameNotificationHandler
    {
        private Dictionary<string, INotificationReceiver.OnNotificationReceived> notifiablePerEvent = new();

        public void ChangeNotifiableState(IEnumerable<(string, INotificationReceiver.OnNotificationReceived)> notifiables, bool state)
        {
            foreach ((string type, INotificationReceiver.OnNotificationReceived notifiable) in notifiables)
            {
                ChangeNotifiableState(type, notifiable, state);
            }
        }
        
        public void ChangeNotifiableState(string type, INotificationReceiver.OnNotificationReceived notifiable, bool state)
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
        
        public void RegisterNotifiable(string type, INotificationReceiver.OnNotificationReceived notifiable)
        {
            if (!notifiablePerEvent.ContainsKey(type))
            {
                notifiablePerEvent[type] = null;
            }
            
            notifiablePerEvent[type] += notifiable;
        }
        
        public void UnregisterNotifiable(string type, INotificationReceiver.OnNotificationReceived notifiable)
        {
            if (!notifiablePerEvent.ContainsKey(type))
            {
                return;
            }
            notifiablePerEvent[type] -= notifiable;
        }

        public void RaiseNotification<T>(string type, T notification, MonoBehaviour sender, bool isGlobal) where T : IGameNotification
        {
            if (!notifiablePerEvent.ContainsKey(type))
            {
                return;
            }
            notifiablePerEvent[type]?.Invoke(notification, sender, isGlobal);
        }
        
        public void RaiseNotification<T>(T notification, MonoBehaviour sender, bool isGlobal) where T : IGameNotification
        {
            if (!notifiablePerEvent.ContainsKey(notification.Name))
            {
                return;
            }
            notifiablePerEvent[notification.Name]?.Invoke(notification, sender, isGlobal);
        }
        
        public void RaiseNotification<T>(MonoBehaviour sender, bool isGlobal) where T : IGameNotification, new()
        {
            T notification = new T();
            if (!notifiablePerEvent.ContainsKey(notification.Name))
            {
                return;
            }
            notifiablePerEvent[notification.Name]?.Invoke(notification, sender, isGlobal);
        }
    }
}