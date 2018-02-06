using System.Windows;
using Caliburn.Micro;

namespace RealmListManager.UI.Dialogs
{
    public class MessageBoxViewModel : Screen
    {
        public string Message { get; set; }
        public string Title { get; set; }
        public MessageBoxButton ButtonType { get; set; } = MessageBoxButton.OKCancel;
        public MessageBoxResult Result { get; set; }

        public bool ShowRightButton => ButtonType != MessageBoxButton.OK;

        public string LeftButtonText
        {
            get
            {
                switch (ButtonType)
                {
                    case MessageBoxButton.OK:
                    case MessageBoxButton.OKCancel:
                        return "OK";
                    case MessageBoxButton.YesNoCancel:
                    case MessageBoxButton.YesNo:
                        return "Yes";
                    default:
                        return "";
                }
            }
        }

        public string RightButtonText
        {
            get
            {
                switch (ButtonType)
                {
                    case MessageBoxButton.OKCancel:
                        return "Cancel";
                    case MessageBoxButton.YesNo:
                    case MessageBoxButton.YesNoCancel:
                        return "NO";
                    default:
                        return "";
                }
            }
        }

        public void LeftButton()
        {
            switch (ButtonType)
            {
                case MessageBoxButton.OK:
                case MessageBoxButton.OKCancel:
                    Result = MessageBoxResult.OK;
                    break;
                case MessageBoxButton.YesNo:
                case MessageBoxButton.YesNoCancel:
                    Result = MessageBoxResult.Yes;
                    break;
                default:
                    Result = MessageBoxResult.None;
                    break;
            }

            TryClose();
        }

        public void RightButton()
        {
            switch (ButtonType)
            {
                case MessageBoxButton.OKCancel:
                    Result = MessageBoxResult.Cancel;
                    break;
                case MessageBoxButton.YesNo:
                case MessageBoxButton.YesNoCancel:
                    Result = MessageBoxResult.No;
                    break;
                default:
                    Result = MessageBoxResult.None;
                    break;
            }

            TryClose();
        }
    }
}
