using System.Windows;
using System.Windows.Controls;

namespace RealmListManager.UI.Controls
{
    public class HintTextBox : TextBox
    {
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText",
            typeof(string), typeof(HintTextBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register("IsError",
            typeof(bool), typeof(HintTextBox), new PropertyMetadata(false));

        public string HintText
        {
            get => (string) GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }

        public bool IsError
        {
            get => (bool) GetValue(IsErrorProperty);
            set => SetValue(IsErrorProperty, value);
        }
    }
}
