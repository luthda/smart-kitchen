using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Util;
using Microsoft.Azure.ServiceBus;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Communication
{
    public abstract class ClientBase 
        : IDisposable
    {
        protected void LogException(string message, Exception ex)
        {
            Debug.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}: {message}{Environment.NewLine}{ex.CreateExceptionDialogMessage()}");
        }

        protected Task LogMessageHandlerException(ExceptionReceivedEventArgs e)
        {
            Console.WriteLine($"Exception: '{e.Exception.Message}' {e.ExceptionReceivedContext.EntityPath}");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }
}
