using System.Threading.Tasks;
using Windows.UI.Core;

namespace InAppNotification.Infrastructure
{
    public interface IDispatcherHelper
    {
        bool HasAccess { get; }

        Task RunAsync(DispatchedHandler agileCallback);
        Task RunAsync(CoreDispatcherPriority priority, DispatchedHandler agileCallback);
    }
}
