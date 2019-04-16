using System;

namespace InAppNotification.Controls.InAppNotification
{
    /// <summary>
    /// A delegate for <see cref="InAppNotification"/> dismissing.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void InAppNotificationClosingEventHandler(object sender, InAppNotificationClosingEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="InAppNotification"/> Dismissing event.
    /// </summary>
    public class InAppNotificationClosingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotificationClosingEventArgs"/> class.
        /// </summary>
        /// <param name="dismissKind">Dismiss kind that triggered the closing event</param>
        public InAppNotificationClosingEventArgs(InAppNotificationDismissKind dismissKind, string id)
        {
            DismissKind = dismissKind;
            ID = id;
        }

        /// <summary>
        /// Get the ID of closing notification
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Gets the kind of action for the closing event.
        /// </summary>
        public InAppNotificationDismissKind DismissKind { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the notification should be closed.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
