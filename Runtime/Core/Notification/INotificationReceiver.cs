using System.Collections.Generic;
using UnityEngine;

namespace Rikoo.Core.Notification
{
    public interface INotificationReceiver
    {
        public IEnumerable<(string, OnNotificationReceived)> ListenedType
        {
            get
            {
                yield break;
            }
        }
        
        public IEnumerable<(string, OnNotificationReceived)> GlobalListenedType
        {
            get
            {
                yield break;
            }
        }

        public delegate void OnNotificationReceived(IGameNotification notification, MonoBehaviour sender, bool isGlobal);
    }
}