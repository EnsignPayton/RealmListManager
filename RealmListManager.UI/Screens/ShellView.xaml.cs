using System;
using System.Windows;

namespace RealmListManager.UI.Screens
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void ShellView_OnStateChanged(object sender, EventArgs e)
        {
            if (!(DataContext is ShellViewModel viewModel)) return;
            if (!viewModel.MinimizeToTray) return;
            if (WindowState != WindowState.Minimized) return;

            ShowInTaskbar = false;
            Visibility = Visibility.Hidden;
            TaskbarIcon.Visibility = Visibility.Visible;
        }
    }
}
