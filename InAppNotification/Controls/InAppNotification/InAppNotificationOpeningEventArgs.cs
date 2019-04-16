using System;

namespace InAppNotification.Controls.InAppNotification
{
    /// <summary>
    /// A delegate for <see cref="InAppNotification"/> opening.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The event arguments.</param>
    public delegate void InAppNotificationOpeningEventHandler(object sender, InAppNotificationOpeningEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="InAppNotification"/> Dismissing event.
    /// </summary>
    public class InAppNotificationOpeningEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InAppNotificationOpeningEventArgs"/> class.
        /// </summary>
        public InAppNotificationOpeningEventArgs()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the notification should be opened.
        /// </summary>
        public bool Cancel { get; set; }
    }
}
