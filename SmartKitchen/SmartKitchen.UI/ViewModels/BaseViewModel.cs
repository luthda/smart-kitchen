using System;
using System.ComponentModel;

namespace Hsr.CloudSolutions.SmartKitchen.UI.ViewModels
{
    public abstract class BaseViewModel 
        : INotifyPropertyChanged
        , IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose() { }
    }
}
