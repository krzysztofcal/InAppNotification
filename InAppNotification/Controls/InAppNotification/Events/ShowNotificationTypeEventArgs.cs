using System;

namespace InAppNotification.Controls.InAppNotification.Events
{
    public class ShowNotificationTypeEventArgs : EventArgs
    {
        public ShowNotificationTypeEventArgs(InAppNotificationType notificationType, int duration = 0)
        {
            NotificationType = notificationType;
            Duration = duration;
        }
        
        public InAppNotificationType NotificationType { get; private set; }
        public int Duration { get; private set; }
    }
}
