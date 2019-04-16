using InAppNotification.Controls.InAppNotification.Events;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace InAppNotification.Controls.InAppNotification
{
    public class InAppNotificationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WarrningDataTemplate { get; set; }

        public DataTemplate ErrorDataTemplate { get; set; }

        public DataTemplate InfoDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return SelectTemplateCore(item, null);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if(!(item is InAppNotificationType))
            {
                throw new NotImplementedException();
            }
            var notificatioType = (InAppNotificationType)item;
            if (notificatioType == InAppNotificationType.Error) return ErrorDataTemplate;
            if (notificatioType == InAppNotificationType.Info) return InfoDataTemplate;
            if (notificatioType == InAppNotificationType.Warrning) return WarrningDataTemplate;
            return null;
        }
    }
}
