using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Hsr.CloudSolutions.SmartKitchen.UI;
using Microsoft.Extensions.DependencyInjection;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _services;
        private IDialogService _dialogService;

        protected override async void OnStartup(StartupEventArgs e)
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;

            _services = SetupDependencies();
            _dialogService = _services.GetService<IDialogService>();

            var viewModel = _services.GetService<MainWindowViewModel>();
            await viewModel.InitAsync();

            var view = _services.GetService<MainWindow>();
            view.DataContext = viewModel;
            view.Closing += OnClosing;

            MainWindow = view;
            MainWindow.Show();

            base.OnStartup(e);
        }

        private IServiceProvider SetupDependencies()
        {
            var serviceCollection = new ServiceCollection()
                .ConfigureControlPanel();

            return serviceCollection.BuildServiceProvider();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _dialogService.ShowException(e.Exception);
            e.Handled = true;
        }
    }
}