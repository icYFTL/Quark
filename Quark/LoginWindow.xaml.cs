using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;

namespace Quark
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : ModernWindow
    {
        public Notifier notifier;
        public LoginWindow()
        {
            InitializeComponent();

            this.notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: this,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = this.Dispatcher;
            });

            
        }

        void OnGroupsComboTextChanged(object sender, RoutedEventArgs e)
        {
            GroupsCombo.IsDropDownOpen = true;
            var tb = (TextBox)e.OriginalSource;
            tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(GroupsCombo.ItemsSource);
            cv.Filter = s =>
                ((string)s).IndexOf(GroupsCombo.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;

        }

        void OnStudentsComboTextChanged(object sender, RoutedEventArgs e)
        {
            StudentsCombo.IsDropDownOpen = true;
            var tb = (TextBox)e.OriginalSource;
            tb.Select(tb.SelectionStart + tb.SelectionLength, 0);
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(StudentsCombo.ItemsSource);
            cv.Filter = s =>
                ((string)s).IndexOf(StudentsCombo.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }


    }
}
