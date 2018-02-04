using System.Windows;
using System.Windows.Controls;

namespace RealmListManager.UI.Controls
{
    public class HintTextBox : TextBox
    {
        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText",
            typeof(string), typeof(HintTextBox), new PropertyMetadata(string.Empty));

        public string HintText
        {
            get => (string) GetValue(HintTextProperty);
            set => SetValue(HintTextProperty, value);
        }
    }
}
