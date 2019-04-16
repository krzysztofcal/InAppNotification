using InAppNotification.Controls.InAppNotification.Events;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace InAppNotification.Controls.InAppNotification
{
    /// <summary>
    /// In App Notification defines a control to show local notification in the app. 
    /// </summary>
    [TemplateVisualState(Name = StateContentVisible, GroupName = GroupContent)]
    [TemplateVisualState(Name = StateContentCollapsed, GroupName = GroupContent)]
    [TemplatePart(Name = DismissButtonPart, Type = typeof(Button))]
    public sealed partial class InAppNotification : ContentControl
    {   
        #region Constans

        /// <summary>
        /// Key of the VisualStateGroup that show/dismiss content
        /// </summary>
        private const string GroupContent = "State";

        /// <summary>
        /// Key of the VisualState when content is showed
        /// </summary>
        private const string StateContentVisible = "Visible";

        /// <summary>
        /// Key of the VisualState when content is dismissed
        /// </summary>
        private const string StateContentCollapsed = "Collapsed";

        /// <summary>
        /// Key of the UI Element that dismiss the control
        /// </summary>
        private const string DismissButtonPart = "PART_DismissButton";

        #endregion

        private InAppNotificationDismissKind _lastDismissKind;
        private DispatcherTimer _animationTimer = new DispatcherTimer();
        private DispatcherTimer _dismissTimer = new DispatcherTimer();
        private Button _dismissButton;
        private VisualStateGroup _visualStateGroup;

        InAppNotificationViewModel ViewModel => DataContext as InAppNotificationViewModel;

        public InAppNotification()
        {
            this.DefaultStyleKey = typeof(InAppNotification);
            this.DataContextChanged += InAppNotification_DataContextChanged;
            this.Closing += InAppNotification_Closing;
            this.Closed += InAppNotification_Closed;

            _dismissTimer.Tick += DismissTimer_Tick;
        }

        private void InAppNotification_Closed(object sender, InAppNotificationClosedEventArgs e)
        {
            if (OnClosedCommand != null && OnClosedCommand.CanExecute(e.ID))
                OnClosedCommand.Execute(e.ID);
        }

        private void InAppNotification_Closing(object sender, InAppNotificationClosingEventArgs e)
        {
            if (OnClosingCommand != null && OnClosingCommand.CanExecute(e.ID))
                OnClosingCommand.Execute(e.ID);
        }

        private void InAppNotification_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (ViewModel == null)
                return;
            ViewModelRemoveEventHandlers();
            ViewModelAddEventHandlers();           
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            if (_dismissButton != null)
            {
                _dismissButton.Click -= DismissButton_Click;
            }

            _dismissButton = (Button)GetTemplateChild(DismissButtonPart);
            _visualStateGroup = (VisualStateGroup)GetTemplateChild(GroupContent);

            if (_dismissButton != null)
            {
                _dismissButton.Visibility = ShowDismissButton ? Visibility.Visible : Visibility.Collapsed;
                _dismissButton.Click += DismissButton_Click;
            }

            if (Visibility == Visibility.Visible)
            {
                VisualStateManager.GoToState(this, StateContentVisible, true);
            }
            else
            {
                VisualStateManager.GoToState(this, StateContentCollapsed, true);
            }

            base.OnApplyTemplate();
        }

        #region NotificationPanel View model event handlers

        private void ViewModelRemoveEventHandlers()
        {
            ViewModel.ShowNotificationTextEventHandler -= ViewModel_ShowNotificationTextEventHandler;
            ViewModel.ShowNotificationDataTemplateEventHandler -= ViewModel_ShowNotificationDataTemplateEventHandler;
            ViewModel.ShowNotificationUIElementEventHandler -= ViewModel_ShowNotificationUIElementEventHandler;
            ViewModel.ShowNotificationTypeEventHandler -= ViewModel_ShowNotificationTypeEventHandler;
            ViewModel.DismissNotificationsEventHandler -= ViewModel_DismissNotificationsEventHandler;
        }

        private void ViewModelAddEventHandlers()
        {
            ViewModel.ShowNotificationTextEventHandler += ViewModel_ShowNotificationTextEventHandler;
            ViewModel.ShowNotificationDataTemplateEventHandler += ViewModel_ShowNotificationDataTemplateEventHandler;
            ViewModel.ShowNotificationUIElementEventHandler += ViewModel_ShowNotificationUIElementEventHandler;
            ViewModel.ShowNotificationTypeEventHandler += ViewModel_ShowNotificationTypeEventHandler;
            ViewModel.DismissNotificationsEventHandler += ViewModel_DismissNotificationsEventHandler;
        }

        private void ViewModel_ShowNotificationTypeEventHandler(object sender, ShowNotificationTypeEventArgs e)
        {
            Show(e.NotificationType, e.Duration);
        }

        private void ViewModel_ShowNotificationTextEventHandler(object sender, ShowNotificationTextEventArgs e)
        {
            Show(e.Text, e.Duration);
        }

        private void ViewModel_ShowNotificationDataTemplateEventHandler(object sender, ShowNotificationDataTemplateEventArgs e)
        {
            Show(e.DataTemplate, e.Duration);
        }

        private void ViewModel_ShowNotificationUIElementEventHandler(object sender, ShowNotificationUIElementEventArgs e)
        {
            Show(e.UIElement, e.Duration);
        }

        private void ViewModel_DismissNotificationsEventHandler(object sender, EventArgs e)
        {
            Dismiss();
        }

        #endregion

        /// <summary>
        /// Show notification using the current template
        /// </summary>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(int duration = 0)
        {
            _animationTimer.Stop();
            _dismissTimer.Stop();

            var eventArgs = new InAppNotificationOpeningEventArgs();
            Opening?.Invoke(this, eventArgs);

            if (eventArgs.Cancel)
            {
                return;
            }

            Visibility = Visibility.Visible;
            VisualStateManager.GoToState(this, StateContentVisible, true);

            _animationTimer.Interval = AnimationDuration;
            _animationTimer.Tick += OpenAnimationTimer_Tick;
            _animationTimer.Start();

            if (duration > 0)
            {
                _dismissTimer.Interval = TimeSpan.FromMilliseconds(duration);
                _dismissTimer.Start();
            }
        }

        /// <summary>
        /// Show notification using text as the content of the notification
        /// </summary>
        /// <param name="text">Text used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(string text, int duration = 0)
        {
            ContentTemplate = null;
            Content = text;
            Show(duration);
        }

        /// <summary>
        /// Show notification using UIElement as the content of the notification
        /// </summary>
        /// <param name="element">UIElement used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(UIElement element, int duration = 0)
        {
            ContentTemplate = null;
            Content = element;
            Show(duration);
        }

        /// <summary>
        /// Show notification using DataTemplate as the content of the notification
        /// </summary>
        /// <param name="dataTemplate">DataTemplate used as the content of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(DataTemplate dataTemplate, int duration = 0)
        {
            ContentTemplate = dataTemplate;
            Content = null;
            Show(duration);
        }

        /// <summary>
        /// Show notification using predefined DataTemplate from ItemTemplateSelector
        /// </summary>
        /// <param name="notificationType">Type of the notification</param>
        /// <param name="duration">Displayed duration of the notification in ms (less or equal 0 means infinite duration)</param>
        public void Show(InAppNotificationType notificationType, int duration = 0)
        {
            var template = ItemTemplateSelector.SelectTemplate(notificationType) ?? null;
            ContentTemplate = template;
            Content = null;
            //TODO set datacontext if notification type would be recognized by appropriate ViewModel
            Show(duration);
        }

        /// <summary>
        /// Dismiss the notification
        /// </summary>
        public void Dismiss()
        {
            Dismiss(InAppNotificationDismissKind.Programmatic);
        }

        private void Dismiss(InAppNotificationDismissKind dismissKind)
        {
            if(Visibility == Visibility.Visible)
            {
                _animationTimer.Stop();
                
                var closingEventArgs = new InAppNotificationClosingEventArgs(dismissKind, NotificationId);
                Closing?.Invoke(this, closingEventArgs);
                if (closingEventArgs.Cancel)
                {
                    return;
                }

                VisualStateManager.GoToState(this, StateContentCollapsed, true);

                _lastDismissKind = dismissKind;

                _animationTimer.Interval = AnimationDuration;
                _animationTimer.Tick += DismissAnimationTimer_Tick;
                _animationTimer.Start();
            }
        }

        #region Events

        /// <summary>
        /// Event raised when the notification is opening
        /// </summary>
        public event InAppNotificationOpeningEventHandler Opening;

        /// <summary>
        /// Event raised when the notification is opened
        /// </summary>
        public event EventHandler Opened;

        /// <summary>
        /// Event raised when the notification is closing
        /// </summary>
        public event InAppNotificationClosingEventHandler Closing;

        /// <summary>
        /// Event raised when the notification is closed
        /// </summary>
        public event InAppNotificationClosedEventHandler Closed;

        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            Dismiss(InAppNotificationDismissKind.User);
        }

        private void DismissTimer_Tick(object sender, object e)
        {
            Dismiss(InAppNotificationDismissKind.Timeout);
            _dismissTimer.Stop();
        }

        private void OpenAnimationTimer_Tick(object sender, object e)
        {
            _animationTimer.Stop();
            Opened?.Invoke(this, EventArgs.Empty);
            _animationTimer.Tick -= OpenAnimationTimer_Tick;
        }

        private void DismissAnimationTimer_Tick(object sender, object e)
        {
            _animationTimer.Stop();
            Closed?.Invoke(this, new InAppNotificationClosedEventArgs(_lastDismissKind, NotificationId));
            _animationTimer.Tick -= DismissAnimationTimer_Tick;
        }

        #endregion

        #region Attached properties

        /// <summary>
        /// Gets the value of the KeyFrameDuration attached Property
        /// </summary>
        /// <param name="obj">the KeyFrame where the duration is set</param>
        /// <returns>Value of KeyFrameDuration</returns>
        public static TimeSpan GetKeyFrameDuration(DependencyObject obj)
        {
            return (TimeSpan)obj.GetValue(KeyFrameDurationProperty);
        }

        /// <summary>
        /// Sets the value of the KeyFrameDuration attached property
        /// </summary>
        /// <param name="obj">The KeyFrame object where the property is attached</param>
        /// <param name="value">The TimeSpan value to be set as duration</param>
        public static void SetKeyFrameDuration(DependencyObject obj, TimeSpan value)
        {
            obj.SetValue(KeyFrameDurationProperty, value);
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for KeyFrameDuration. This enables animation, styling, binding, etc
        /// </summary>
        public static readonly DependencyProperty KeyFrameDurationProperty =
            DependencyProperty.RegisterAttached(
                "KeyFrameDuration", 
                typeof(TimeSpan), 
                typeof(InAppNotification), 
                new PropertyMetadata(0, OnKeyFrameAnimationChanged));

        private static void OnKeyFrameAnimationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is TimeSpan ts)
            {
                var keyTimeFromAnimationDuration = KeyTime.FromTimeSpan(ts);
                if (d is DoubleKeyFrame dkf)
                {
                    dkf.KeyTime = KeyTime.FromTimeSpan(ts);
                }
                else if (d is ObjectKeyFrame okf)
                {
                    okf.KeyTime = KeyTime.FromTimeSpan(ts);
                }
            }
        }

        public static readonly DependencyProperty OnClosingCommandDependencyProperty = DependencyProperty.RegisterAttached(
            nameof(OnClosingCommand),
            typeof(ICommand),
            typeof(InAppNotification),
            null
        );

        public ICommand OnClosingCommand
        {
            get { return (ICommand)GetValue(OnClosingCommandDependencyProperty); }
            set { SetValue(OnClosingCommandDependencyProperty, value); }
        }

        public static readonly DependencyProperty OnClosedCommandDependecyProperty = DependencyProperty.RegisterAttached(
            nameof(OnClosedCommand),
            typeof(ICommand),
            typeof(InAppNotification),
            null
        );

        public ICommand OnClosedCommand
        {
            get { return (ICommand)GetValue(OnClosedCommandDependecyProperty); }
            set { SetValue(OnClosedCommandDependecyProperty, value); }
        }

        #endregion

        #region Properties

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.RegisterAttached(
            nameof(ItemTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(InAppNotification),
            new PropertyMetadata(null, ItemTemplateSelectorChanged)
        );

        private static void ItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as InAppNotification;
            if (instance == null) return;
            //recreate content here?
        }

        public static readonly DependencyProperty NotificationIdDependencyProperty = DependencyProperty.Register(
            nameof(NotificationId),
            typeof(string),
            typeof(InAppNotification),
            null
            );

        public string NotificationId
        {
            get { return (string)GetValue(NotificationIdDependencyProperty); }
            set { SetValue(NotificationIdDependencyProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ShowDismissButton"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowDismissButtonProperty =
            DependencyProperty.Register(
                nameof(ShowDismissButton),
                typeof(bool),
                typeof(InAppNotification),
                new PropertyMetadata(true, OnShowDismissButtonChanged)
                );

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(
                nameof(AnimationDuration),
                typeof(TimeSpan),
                typeof(InAppNotification),
                new PropertyMetadata(TimeSpan.FromMilliseconds(100))
            );

        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(
                nameof(VerticalOffset),
                typeof(double),
                typeof(InAppNotification),
                new PropertyMetadata(100)
            );

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register(
                nameof(HorizontalOffset),
                typeof(double),
                typeof(InAppNotification),
                new PropertyMetadata(0)
            );


        /// <summary>
        /// Gets or sets a value indicating whether to show the Dismiss button of the control.
        /// </summary>
        public bool ShowDismissButton
        {
            get { return (bool)GetValue(ShowDismissButtonProperty); }
            set { SetValue(ShowDismissButtonProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the duration of the popup animation (in milliseconds).
        /// </summary>
        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the vertical offset of the popup animation.
        /// </summary>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the horizontal offset of the popup animation.
        /// </summary>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        private static void OnShowDismissButtonChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inApNotification = d as InAppNotification;

            if (inApNotification._dismissButton != null)
            {
                bool showDismissButton = (bool)e.NewValue;
                inApNotification._dismissButton.Visibility = showDismissButton ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion
    }
}
