using InAppNotification.Controls.InAppNotification;
using InAppNotification.Controls.InAppNotification.Events;
using InAppNotification.MVVM;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace InAppNotification.Infrastructure
{
    public class NotificationListViewModel : ViewModelBase, INotificationListViewModel, INotificationClient
    {
        private readonly INotificationManager _NotificationManager;
        private readonly IDispatcherHelper _DispatcherHelper;

        public NotificationListViewModel(INotificationManager notificationManager, IDispatcherHelper dispatcherHelper)
        {
            _NotificationManager = notificationManager;
            _DispatcherHelper = dispatcherHelper;

            ClosingCommand = new RelayCommand<string>((str) => ClosingCommandExecute(str), (str) => !string.IsNullOrEmpty(str));
            ClosedCommand = new RelayCommand<string>((str) => ClosedCommandExecute(str), (str) => !string.IsNullOrEmpty(str));

            //registering this notification client to receive future notifications from the notification manager
            _NotificationManager.RegisterNotificationClient(this);
        }

        public ICommand ClosingCommand { get; private set; }

        public ICommand ClosedCommand { get; private set; }

        private void ClosingCommandExecute(string id)
        {
            Debug.WriteLine($"NotificationListViewModel.ClosingCommandExecute() with ID = {id}");
            //Doing nothing intencionally :D, can detach this command. Removing items is handled in ClosedCommandExecute
        }

        private async void ClosedCommandExecute(string id)
        {
            //this method is removing notifications from the list
            Debug.WriteLine($"NotificationListViewModel.ClosedCommandExecute() with ID = {id}");
            if (_notificationCollection.Where(x => x.ID == id).Count() > 0)
            {
                Debug.WriteLine($"Removing item with id = {id}");
                await _DispatcherHelper.RunAsync(() => {
                    var notification = _notificationCollection.FirstOrDefault(x => x.ID == id);
                    NotificationCollection.Remove(notification);
                    //TODO dispose notification - notification.Dispose();
                });
            }
        }

        private ObservableCollection<IInAppNotificationViewModel> _notificationCollection = new ObservableCollection<IInAppNotificationViewModel>();
        public ObservableCollection<IInAppNotificationViewModel> NotificationCollection
        {
            get { return _notificationCollection; }
            private set { Set(ref _notificationCollection, value); }
        }

        public async void DismissNotification()
        {
            Debug.WriteLine("NotificationListViewModel.DismissNotifications()");
            await _DispatcherHelper.RunAsync(() => {
                foreach (var notification in NotificationCollection)
                {
                    notification.DismissNotification();
                }
            });
        }

        public async void ShowNotification(string text, int duration = 0)
        {
            var vm = new InAppNotificationViewModel(_NotificationManager, _DispatcherHelper);
            await _DispatcherHelper.RunAsync(async () => {
                NotificationCollection.Add((IInAppNotificationViewModel)vm);
                await Task.Delay(250);  //stiupid workaround - we are waiting here until DataContext is set
                vm.ShowNotification(text, duration);
            });
        }

        public async void ShowNotification(UIElement element, int duration = 0)
        {
            var vm = new InAppNotificationViewModel(_NotificationManager, _DispatcherHelper);
            await _DispatcherHelper.RunAsync(async () =>
            {
                NotificationCollection.Add((IInAppNotificationViewModel)vm);
                await Task.Delay(250);  //stiupid workaround - we are waiting here until DataContext is set
                vm.ShowNotification(element, duration);
            });
        }

        public async void ShowNotification(DataTemplate dataTemplate, int duration = 0)
        {
            var vm = new InAppNotificationViewModel(_NotificationManager, _DispatcherHelper);
            await _DispatcherHelper.RunAsync(async () =>
            {
                NotificationCollection.Add((IInAppNotificationViewModel)vm);
                await Task.Delay(250);  //stiupid workaround - we are waiting here until DataContext is set
                vm.ShowNotification(dataTemplate, duration);
            });
        }

        public async void ShowNotification(InAppNotificationType notificationType, int duration = 0)
        {
            var vm = new InAppNotificationViewModel(_NotificationManager, _DispatcherHelper);
            await _DispatcherHelper.RunAsync(async () => {
                NotificationCollection.Add(vm);
                await Task.Delay(250);
                vm.ShowNotification(notificationType, duration);
            });
        }
    }
}
