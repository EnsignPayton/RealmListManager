using System.Windows;
using RealmListManager.UI.Core.Models;

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

        private void Play_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is ShellViewModel vm)) return;
            if (!(sender is FrameworkElement element)) return;
            if (!(element.DataContext is LocationModel location)) return;

            vm.PlayLocation(location);
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is ShellViewModel vm)) return;
            if (!(sender is FrameworkElement element)) return;
            if (!(element.DataContext is LocationModel location)) return;

            vm.EditLocation(location);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is ShellViewModel vm)) return;
            if (!(sender is FrameworkElement element)) return;
            if (!(element.DataContext is LocationModel location)) return;

            vm.ConfirmDeleteLocation(location);
        }
    }
}
