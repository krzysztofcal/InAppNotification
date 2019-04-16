using System;

namespace InAppNotification.Controls.InAppNotification.Events
{
    public class ShowNotificationTextEventArgs : EventArgs
    {
        public ShowNotificationTextEventArgs(string text, int duration)
        {
            Text = text;
            Duration = duration;
        }

        public string Text { get; }
        public int Duration { get; }
    }
}
