using InAppNotification.Controls.InAppNotification.Events;
using InAppNotification.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace InAppNotification.Controls.InAppNotification
{
    public class NotificationManager : INotificationManager
    {
        List<INotificationClient> _Clients = new List<INotificationClient>();
        SemaphoreSlim _ClientsLock = new SemaphoreSlim(1);

        public NotificationManager() { }    //parameterless ctor is necessary to register the manager in IoC container

        #region Client Registration

        public void RegisterNotificationClient(INotificationClient notificationClient)
        {
            _ClientsLock.Wait();
            try
            {
                if (!_Clients.Contains(notificationClient))
                    _Clients.Add(notificationClient);
            }
            finally
            {
                _ClientsLock.Release();
            }
        }

        public void UnregisterNotificationClient(INotificationClient notificationClient)
        {
            _ClientsLock.Wait();
            try
            {
                if (_Clients.Contains(notificationClient))
                    _Clients.Remove(notificationClient);
            }
            finally
            {
                _ClientsLock.Release();
            }
        }

        #endregion

        #region Notify clients about Notification events
        public void ShowNotification(string text, int duration = 0)
        {
            _ClientsLock.Wait();
            try
            {
                foreach (var cli in _Clients)
                    cli.ShowNotification(text, duration);
            }
            finally
            {
                _ClientsLock.Release();
            }
        }

        public void ShowNotification(UIElement element, int duration = 0)
        {
            _ClientsLock.Wait();
            try
            {
                foreach (var cli in _Clients)
                    cli.ShowNotification(element, duration);
            }
            finally
            {
                _ClientsLock.Release();
            }
        }

        public void ShowNotification(DataTemplate dataTemplate, int duration = 0)
        {
            _ClientsLock.Wait();
            try
            {
                foreach (var cli in _Clients)
                    cli.ShowNotification(dataTemplate, duration);
            }
            finally
            {
                _ClientsLock.Release();
            }
        }

        public void ShowNotification(InAppNotificationType notificationType, int duration = 0)
        {
            _ClientsLock.Wait();
            try
            {
                foreach (var cli in _Clients)
                    cli.ShowNotification(notificationType, duration);
            }
            finally
            {
                _ClientsLock.Release();
            }
        }

        public void DismissNotifications()
        {
            _ClientsLock.Wait();
            try
            {
                foreach (var cli in _Clients)
                    cli.DismissNotification();
            }
            finally
            {
                _ClientsLock.Release();
            }
        }
        
        #endregion
    }
}
