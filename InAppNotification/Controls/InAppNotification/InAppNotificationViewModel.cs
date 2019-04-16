using InAppNotification.Controls.InAppNotification.Events;
using InAppNotification.Infrastructure;
using InAppNotification.MVVM;
using System;
using Windows.UI.Xaml;

namespace InAppNotification.Controls.InAppNotification
{
    public interface IInAppNotificationViewModel
    {
        string ID { get; }
        event EventHandler<ShowNotificationTextEventArgs> ShowNotificationTextEventHandler;
        event EventHandler<ShowNotificationUIElementEventArgs> ShowNotificationUIElementEventHandler;
        event EventHandler<ShowNotificationDataTemplateEventArgs> ShowNotificationDataTemplateEventHandler;
        event EventHandler<ShowNotificationTypeEventArgs> ShowNotificationTypeEventHandler;
        event EventHandler DismissNotificationsEventHandler;

        void DismissNotification();
    }

    public class InAppNotificationViewModel : ViewModelBase, IInAppNotificationViewModel, INotificationClient
    {
        private readonly INotificationManager _NotificationManager;
        private readonly IDispatcherHelper _dispatcherHelper;

        public InAppNotificationViewModel(INotificationManager notificationManager,
                                          IDispatcherHelper dispatcherHelper)
        {
            _dispatcherHelper = dispatcherHelper;
            _NotificationManager = notificationManager;
            // _NotificationManager.RegisterNotificationClient(this);
            ID = Guid.NewGuid().ToString();
        }

        //TODO Change to commands?
        public event EventHandler<ShowNotificationTextEventArgs> ShowNotificationTextEventHandler;
        public event EventHandler<ShowNotificationUIElementEventArgs> ShowNotificationUIElementEventHandler;
        public event EventHandler<ShowNotificationDataTemplateEventArgs> ShowNotificationDataTemplateEventHandler;
        public event EventHandler<ShowNotificationTypeEventArgs> ShowNotificationTypeEventHandler;
        public event EventHandler DismissNotificationsEventHandler;

        private string _id = "";
        public string ID
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        public void ShowNotification(string text, int duration = 0)
        {
            _dispatcherHelper.RunAsync(() =>
            {
                ShowNotificationTextEventHandler?.Invoke(this, new ShowNotificationTextEventArgs(text, duration));
            });
        }

        public void ShowNotification(UIElement element, int duration = 0)
        {
            _dispatcherHelper.RunAsync(() =>
            {
                ShowNotificationUIElementEventHandler?.Invoke(this, new ShowNotificationUIElementEventArgs(element, duration));
            });
        }

        public void ShowNotification(DataTemplate dataTemplate, int duration = 0)
        {
            _dispatcherHelper.RunAsync(() =>
            {
                ShowNotificationDataTemplateEventHandler?.Invoke(this, new ShowNotificationDataTemplateEventArgs(dataTemplate, duration));
            });
        }

        public void DismissNotification()
        {
            _dispatcherHelper.RunAsync(() =>
            {
                DismissNotificationsEventHandler?.Invoke(this, new EventArgs());
            });
        }

        public void ShowNotification(InAppNotificationType notificationType, int duration = 0)
        {
            _dispatcherHelper.RunAsync(() => {
                ShowNotificationTypeEventHandler?.Invoke(this, new ShowNotificationTypeEventArgs(notificationType, duration));
            });
        }
    }
}
