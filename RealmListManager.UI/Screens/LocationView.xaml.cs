using System.Windows;
using System.Windows.Controls;
using RealmListManager.UI.Core.Models;

namespace RealmListManager.UI.Screens
{
    /// <summary>
    /// Interaction logic for LocationView.xaml
    /// </summary>
    public partial class LocationView : UserControl
    {
        public LocationView()
        {
            InitializeComponent();
        }

        private void Play_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is LocationViewModel vm)) return;
            if (!(sender is FrameworkElement element)) return;
            if (!(element.DataContext is RealmlistModel realmlist)) return;

            vm.Play(realmlist);
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is LocationViewModel vm)) return;
            if (!(sender is FrameworkElement element)) return;
            if (!(element.DataContext is RealmlistModel realmlist)) return;

            vm.EditRealmlist(realmlist);
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is LocationViewModel vm)) return;
            if (!(sender is FrameworkElement element)) return;
            if (!(element.DataContext is RealmlistModel realmlist)) return;

            vm.DeleteRealmlist(realmlist);
        }
    }
}
