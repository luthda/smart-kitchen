using System;
using System.Windows;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.UI
{
    public class DialogService
        : IDialogService
    {
        private readonly Window _window;

        public DialogService(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
        }

        public void ShowException(Exception exception)
        {
            _window.Dispatcher?.Invoke(() =>
            {
                var message = exception.CreateExceptionDialogMessage();
                MessageBox.Show(_window, message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}