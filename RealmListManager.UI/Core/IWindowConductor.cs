using System;
using System.Windows;
using Caliburn.Micro;

namespace RealmListManager.UI.Core
{
    public interface IWindowConductor
    {
        /// <summary>
        /// Show a screen as the ActiveItem.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        void Show<T>(Action<T> initAction = null) where T : IScreen;

        /// <summary>
        /// Show a screen in a modal dialog.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        /// <returns>ViewModel</returns>
        T ShowDialog<T>(Action<T> initAction = null) where T : IScreen;

        /// <summary>
        /// Show a screen in a non-modal window.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        /// <returns>ViewModel</returns>
        T ShowWindow<T>(Action<T> initAction = null) where T : IScreen;

        /// <summary>
        /// Show a screen in a popup control at the current mouse position.
        /// </summary>
        /// <typeparam name="T">ViewModel</typeparam>
        /// <param name="initAction">Initialization Action</param>
        /// <returns>ViewModel</returns>
        T ShowPopup<T>(Action<T> initAction = null) where T : IScreen;

        /// <summary>
        /// Show a message box in the application's theme.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Title</param>
        /// <param name="buttonType">Button Type</param>
        /// <returns>Result</returns>
        MessageBoxResult ShowMessageBox(string message, string title = null, MessageBoxButton buttonType = MessageBoxButton.OKCancel);
    }
}
