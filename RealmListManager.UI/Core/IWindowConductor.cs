using System;
using System.Windows;
using Caliburn.Micro;

namespace RealmListManager.UI.Core
{
    public interface IWindowConductor
    {
        void Show<T>(Action<T> initAction = null) where T : IScreen;
        T ShowDialog<T>(Action<T> initAction = null) where T : IScreen;
        T ShowWindow<T>(Action<T> initAction = null) where T : IScreen;
        T ShowPopup<T>(Action<T> initAction = null) where T : IScreen;
        MessageBoxResult ShowMessageBox(string message, string title = null, MessageBoxButton buttonType = MessageBoxButton.OKCancel);
    }
}
