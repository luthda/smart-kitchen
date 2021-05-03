using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices
{
    public interface ISimDevice 
        : IDisposable
        , INotifyPropertyChanged
    {
        Point Coordinate { get; }
        Point Size { get; }

        Task UnregisterAsync();
    }
}
