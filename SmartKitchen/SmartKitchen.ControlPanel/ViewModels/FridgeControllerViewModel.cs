using Hsr.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;

namespace Hsr.CloudSolutions.SmartKitchen.ControlPanel.ViewModels
{
    public class FridgeControllerViewModel : BaseDeviceControllerViewModel<Fridge>
    {
        public FridgeControllerViewModel(IControlPanelMessageClient<Fridge> client) : base(client, d => (Fridge)d)
        {
            
        }

        private DoorState _door;
        public DoorState Door
        {
            get => _door;
            private set
            {
                if (_door == value)
                {
                    return;
                }
                _door = value;
                OnPropertyChanged(nameof(Door));
            }
        }

        private double _temperature;
        public double Temperature
        {
            get => _temperature;
            set
            {
                if (_temperature == value)
                {
                    return;
                }
                _temperature = value;
                SendCommand(Commands.ChangeTemperature);
                OnPropertyChanged(nameof(Temperature));
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

        protected override void Configure(Fridge config)
        {
            Door = config.Door;
            Temperature = config.Temperature;
            CurrentTemperature = config.Temperature;
        }

        protected override void OnUpdate(Fridge update)
        {
            Door = update.Door;
            CurrentTemperature = update.Temperature;
        }

        protected override void Prepare(Fridge device)
        {
            device.Temperature = Temperature;
        }
    }
}
