using System;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace InAppNotification.Infrastructure
{
    public class DispatcherHelper : IDispatcherHelper
    {
        private readonly CoreDispatcher _dispatcher;

        public DispatcherHelper()
        {
            _dispatcher = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher;
        }

        public bool HasAccess => _dispatcher.HasThreadAccess;

        public async Task RunAsync(DispatchedHandler agileCallback)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, agileCallback);
        }

        public async Task RunAsync(CoreDispatcherPriority priority, DispatchedHandler agileCallback)
        {
            await _dispatcher.RunAsync(priority, agileCallback);
        }
    }
}
