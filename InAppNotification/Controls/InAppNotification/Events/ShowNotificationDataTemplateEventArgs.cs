using System;
using Windows.UI.Xaml;

namespace InAppNotification.Controls.InAppNotification.Events
{
    public class ShowNotificationDataTemplateEventArgs : EventArgs
    {
        public ShowNotificationDataTemplateEventArgs(DataTemplate dataTemplate, int duration = 0)
        {
            DataTemplate = dataTemplate;
            Duration = duration;
        }

        public DataTemplate DataTemplate { get; }
        public int Duration { get; }
    }
}
