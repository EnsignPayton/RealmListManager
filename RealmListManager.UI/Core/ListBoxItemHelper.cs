using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RealmListManager.UI.Core
{
    public static class ListBoxItemHelper
    {
        public static readonly DependencyProperty IsSelectableProperty = DependencyProperty.RegisterAttached(
            "IsSelectable",
            typeof(bool),
            typeof(ListBoxItemHelper),
            new UIPropertyMetadata(true, IsSelectableChanged));

        public static bool GetIsSelectable(ListBoxItem item)
        {
            return (bool) item.GetValue(IsSelectableProperty);
        }

        public static void SetIsSelectable(ListBoxItem item, bool value)
        {
            item.SetValue(IsSelectableProperty, value);
        }

        private static void IsSelectableChanged(DependencyObject doj, DependencyPropertyChangedEventArgs e)
        {
            if (!(doj is ListBoxItem item)) return;

            if (!(bool) e.NewValue && (bool) e.OldValue)
            {
                item.Selected -= OnItemSelected;
                item.Selected += OnItemSelected;
                BindingOperations.ClearBinding(item, ListBoxItem.IsSelectedProperty);

                if (item.IsSelected)
                    item.IsSelected = false;
            }
            else if ((bool) e.NewValue && !(bool) e.OldValue)
            {
                item.Selected -= OnItemSelected;
            }
        }

        private static void OnItemSelected(object sender, RoutedEventArgs e)
        {
            if (!(sender is ListBoxItem item)) return;

            item.IsSelected = false;
        }
    }
}
