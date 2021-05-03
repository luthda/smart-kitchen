using System;
using Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public class StoveControllerViewModel : BaseDeviceControllerViewModel<Stove>
    {
        public StoveControllerViewModel(IControlPanelMessageClient<Stove> client) : base(client, d => (Stove)d)
        {
        }

        private bool _hasPan;

        public bool HasPan
        {
            get => _hasPan;
            private set
            {
                if (_hasPan == value)
                {
                    return;
                }
                _hasPan = value;
                OnPropertyChanged(nameof(HasPan));
            }
        }

        private const double TemperatureStepSize = 15.0;
        public int TemperatureStep
        {
            get => (int) Math.Round(Temperature / TemperatureStepSize);
            set
            {
                if (TemperatureStep == value)
                {
                    return;
                }
                Temperature = value*TemperatureStepSize;
            }
        }

        private double _temperature;
        public double Temperature
        {
            get => _temperature;
            private set
            {
                if (_temperature == value)
                {
                    return;
                }
                _temperature = value;
                SendCommand(Commands.ChangeTemperature);
                OnPropertyChanged(nameof(Temperature));
                OnPropertyChanged(nameof(TemperatureStep));
            }
        }

        private double _currentTemperature;

        public double CurrentTemperature
        {
            get => _currentTemperature;
            private set
            {
                if (_currentTemperature == value)
                {
                    return;
                }
                _currentTemperature = value;
                OnPropertyChanged(nameof(CurrentTemperature));
            }
        }



        protected override void Configure(Stove config)
        {
            HasPan = config.HasPan;
            Temperature = config.Temperature;
        }

        protected override void OnUpdate(Stove update)
        {
            HasPan = update.HasPan;
            CurrentTemperature = update.Temperature;
        }

        protected override void Prepare(Stove device)
        {
            device.Temperature = Temperature;
        }
    }
}
