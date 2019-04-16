using InAppNotification.Controls.InAppNotification.Events;
using Windows.UI.Xaml;

namespace InAppNotification.Infrastructure
{
    public interface INotificationClient
    {
        /// <summary>
        /// Show simple notification with a text message
        /// </summary>
        /// <param name="text">Notification message</param>
        /// <param name="duration">Notification duration. If 0, notification never dismiss</param>
        void ShowNotification(string text, int duration = 0);

        /// <summary>
        /// Show notification with content defined by the UIElement
        /// </summary>
        /// <param name="element">UIElement that defines content of the notification</param>
        /// <param name="duration">Notification duration. If 0, notification never dismiss</param>
        void ShowNotification(UIElement element, int duration = 0);

        /// <summary>
        /// Show notification with content defined by the Datatemplate
        /// </summary>
        /// <param name="dataTemplate">DataTemplate that defines content of the notification</param>
        /// <param name="duration">Notification duration. If 0, notification never dismiss</param>
        void ShowNotification(DataTemplate dataTemplate, int duration = 0);

        /// <summary>
        /// Show notification with content predefined in the ItemTemplateSelector
        /// </summary>
        /// <param name="notificationType">Type of the notification</param>
        /// <param name="duration">Notification duration. If 0, notification never dismiss</param>
        void ShowNotification(InAppNotificationType notificationType, int duration = 0);

        /// <summary>
        /// Dismiss Notifications
        /// </summary>
        void DismissNotification();
    }
}
