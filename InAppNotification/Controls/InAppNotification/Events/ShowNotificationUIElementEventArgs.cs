using System;
using Windows.UI.Xaml;

namespace InAppNotification.Controls.InAppNotification.Events
{
    public class ShowNotificationUIElementEventArgs : EventArgs
    {
        public ShowNotificationUIElementEventArgs(UIElement uIElement, int duration = 0)
        {
            UIElement = uIElement;
            Duration = duration;
        }

        public UIElement UIElement { get; }
        public int Duration { get; }
    }
}
