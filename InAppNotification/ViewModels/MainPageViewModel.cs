using InAppNotification.Controls.InAppNotification;
using InAppNotification.Controls.InAppNotification.Events;
using InAppNotification.Infrastructure;
using InAppNotification.MVVM;
using System.Windows.Input;

namespace InAppNotification.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INotificationManager _notificationManager;
        private readonly IDispatcherHelper _dispatcherHelper;

        public MainPageViewModel()
        {
            //this should be initiaized within IoC
            _notificationManager = new NotificationManager();
            _dispatcherHelper = new DispatcherHelper();

            NotificationListViewModel = new NotificationListViewModel(_notificationManager, _dispatcherHelper);

            DisplayNotificationCommand = new Relay_Command(DisplayNotificationCommandExecute);
            DisplayDataTemplateNotificationCommand = new Relay_Command(DisplayDataTemplateNotificationCommandExecute);
            DisplayDataTemplateNotificationNoDismissCommand = new Relay_Command(DisplayDataTemplateNotificationNoDismissCommandExecute);
            DisplayErrorTypeNotificationCommand = new Relay_Command(DisplayErrorTypeNotificationCommandExecute);
            DisplayInfoTypeNotificationCommand = new Relay_Command(DisplayInfoTypeNotificationCommandExecute);
            DissmissAllNotificationsCommand = new Relay_Command(DissmissAllNotificationsCommandExecute);
        }

        public INotificationListViewModel NotificationListViewModel { get; }

        public ICommand DisplayNotificationCommand { get; private set; }

        public ICommand DisplayDataTemplateNotificationCommand { get; private set; }

        public ICommand DisplayDataTemplateNotificationNoDismissCommand { get; private set; }

        public ICommand DisplayErrorTypeNotificationCommand { get; private set; }

        public ICommand DisplayInfoTypeNotificationCommand { get; private set; }

        public ICommand DissmissAllNotificationsCommand { get; private set; }

        private void DisplayNotificationCommandExecute()
        {
            System.Diagnostics.Debug.WriteLine("DisplayNotificationCommandExecute");

            _notificationManager.ShowNotification("Text notification from the app settings", 3000);
        }

        private void DisplayDataTemplateNotificationCommandExecute()
        {
            System.Diagnostics.Debug.WriteLine("DisplayDataTemplateNotificationCommand");

            _dispatcherHelper.RunAsync(() => {
                var templatePresent = Windows.UI.Xaml.Application.Current.Resources.TryGetValue(
                    "NetworkUnavailableNotificationDataTemplate", out var networkUnavailableTemplate);
                if (templatePresent && networkUnavailableTemplate is Windows.UI.Xaml.DataTemplate)
                {
                    _notificationManager.ShowNotification(networkUnavailableTemplate as Windows.UI.Xaml.DataTemplate, 3000);
                }
                else
                    _notificationManager.ShowNotification("Internet not availiable", 3000);
            });
        }

        private void DisplayDataTemplateNotificationNoDismissCommandExecute()
        {
            System.Diagnostics.Debug.WriteLine("DisplayDataTemplateNotificationNoDismissCommand");

            _dispatcherHelper.RunAsync(() => {
                var templatePresent = Windows.UI.Xaml.Application.Current.Resources.TryGetValue(
                    "NetworkUnavailableNotificationDataTemplate", out var networkUnavailableTemplate);
                if (templatePresent && networkUnavailableTemplate is Windows.UI.Xaml.DataTemplate)
                {
                    _notificationManager.ShowNotification(networkUnavailableTemplate as Windows.UI.Xaml.DataTemplate);
                }
                else
                    _notificationManager.ShowNotification("Internet not availiable (no dismiss)");
            });
        }

        private void DisplayErrorTypeNotificationCommandExecute()
        {
            System.Diagnostics.Debug.WriteLine("DisplayErrorTypeNotificationCommandExecute");
            _notificationManager.ShowNotification(InAppNotificationType.Error, 3000);
        }

        private void DisplayInfoTypeNotificationCommandExecute()
        {
            System.Diagnostics.Debug.WriteLine("DisplayInfoTypeNotificationCommandExecute");
            _notificationManager.ShowNotification(InAppNotificationType.Info, 3000);
        }

        private void DissmissAllNotificationsCommandExecute()
        {
            System.Diagnostics.Debug.WriteLine("DissmissAllNotificationsCommandExecute");
            _notificationManager.DismissNotifications();
        }

    }
}
