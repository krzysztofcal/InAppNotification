using System;

namespace InAppNotification.Controls.InAppNotification
{
    /// <summary>
    /// A delegate for <see cref="InAppNotification"/> dismissing.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void InAppNotificationClosedEventHandler(object sender, InAppNotificationClosedEventArgs e);

    public class InAppNotificationClosedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotificationClosedEventArgs"/> class.
        /// </summary>
        /// <param name="dismissKind">Dismiss kind that triggered the closing event</param>
        public InAppNotificationClosedEventArgs(InAppNotificationDismissKind dismissKind, string id)
        {
            DismissKind = dismissKind;
            ID = id;
        }

        public string ID { get; private set; }

        /// <summary>
        /// Gets the kind of action for the closing event.
        /// </summary>
        public InAppNotificationDismissKind DismissKind { get; private set; }
    }
}
