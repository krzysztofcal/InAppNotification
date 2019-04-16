using InAppNotification.Controls.InAppNotification;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace InAppNotification.Infrastructure
{
    public interface INotificationListViewModel
    {
        /// <summary>
        /// Command executed by the notification when it starts to closing. It is passing the unique id as a parameter
        /// </summary>
        ICommand ClosingCommand { get; }

        /// <summary>
        /// Command executed by the notification when it was closed (closing animation completed). It is passing the unique id as a parameter
        /// </summary>
        ICommand ClosedCommand { get; }

        /// <summary>
        /// Observable collection of the notifications
        /// </summary>
        ObservableCollection<IInAppNotificationViewModel> NotificationCollection { get; }
    }
}
