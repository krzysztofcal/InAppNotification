using InAppNotification.Controls.InAppNotification.Events;
using Windows.UI.Xaml;

namespace InAppNotification.Infrastructure
{
    public interface INotificationManager
    {
        /// <summary>
        /// Register Notification client to the Notification manager
        /// </summary>
        /// <param name="notificationClient"></param>
        void RegisterNotificationClient(INotificationClient notificationClient);

        /// <summary>
        /// Unregister notification client from the Notification manager
        /// </summary>
        /// <param name="notificationClient"></param>
        void UnregisterNotificationClient(INotificationClient notificationClient);

        /// <summary>
        /// Notify clients to show simple text notification
        /// </summary>
        /// <param name="text">Notification message</param>
        /// <param name="duration">Notification duration. If 0 notification will not dismiss</param>
        void ShowNotification(string text, int duration = 0);

        /// <summary>
        /// Notify clients to show notification with defined UIElement
        /// </summary>
        /// <param name="element">UIelement to be displayed in the notification message</param>
        /// <param name="duration">Notification duration. If 0 notification will not dismiss</param>
        void ShowNotification(UIElement element, int duration = 0);

        /// <summary>
        /// Notify clients to show notification with content defined by DataTemplate parameter
        /// </summary>
        /// <param name="dataTemplate">DataTemplate that defines content of the otification message</param>
        /// <param name="duration">Notification duration. If 0 notification will not dismiss</param>
        void ShowNotification(DataTemplate dataTemplate, int duration = 0);

        /// <summary>
        /// Notify clients to show notification with content predefined in the ItemTemplateSelector
        /// </summary>
        /// <param name="notificationType">Type of the notification</param>
        /// <param name="duration">Notification duration. If 0, notification never dismiss</param>
        void ShowNotification(InAppNotificationType notificationType, int duration = 0);

        /// <summary>
        /// Notify clients to dismiss notifications
        /// </summary>
        void DismissNotifications();
    }
}
