using System;
using Hsr.CloudSolutions.SmartKitchen.Devices;
using Hsr.CloudSolutions.SmartKitchen.Devices.Communication;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices.Core;
using Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine;
using Hsr.CloudSolutions.SmartKitchen.Util;

namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Devices
{
    public class SimOven : BaseSimDevice<Oven>
    {
        public SimOven(Guid id, Point coordinate, Point size) 
            : base(id, "Oven", coordinate, size)
        {
            _temperature = SimulationEnvironment.Current.RoomTemperature;
            TargetTemperature = 0;
            Door = DoorState.Closed;
        }

        protected override void Prepare(Oven device)
        {
            device.Door = Door;
            device.Temperature = Temperature;
        }

        protected override void OnCommandReceived(ICommand<Oven> command)
        {
            switch (command.Type)
            {
                case Commands.ChangeTemperature:
                    if (command.HasDeviceInfo)
                    {
                        ChangeTemperatureTo(command.DeviceState.Temperature);
                    }
                    break;
                case Commands.EmergencyStop:
                    ChangeTemperatureTo(0);
                    break;
            }
        }

        public void ChangeTemperatureTo(double temperature)
        {
            TargetTemperature = temperature;
        }

        private double _targetTemperature;

        public double TargetTemperature
        {
            get => _targetTemperature;
            private set
            {
                if (_targetTemperature == value)
                {
                    return;
                }
                _targetTemperature = value;
                SimulateTemperatureChange();
                OnPropertyChanged(nameof(TargetTemperature));
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
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private DoorState _door;

        public DoorState Door
        {
            get => _door;
            set
            {
                if (_door == value)
                {
                    return;
                }
                _door = value;
                SimulateTemperatureChange();
                OnPropertyChanged(nameof(Door));
            }
        }

        private ISimulation _temperatureSimulation;
        private DeviceTemperatureState _temperatureState = DeviceTemperatureState.None;

        private void SimulateTemperatureChange()
        {
            if (!ChangeSimulation() && Door == DoorState.Closed)
            {
                return;
            }
            _temperatureSimulation?.Dispose();

            if (SimulationTargetTemperature > Temperature)
            {
                _temperatureSimulation = Heating(TimeSpan.FromMilliseconds(1000));
            }
            else if (SimulationTargetTemperature < Temperature)
            {
                _temperatureSimulation = Cooling(TimeSpan.FromMilliseconds(2000));
            }
            else
            {
                _temperatureSimulation = Idle();
            }
        }

        private double SimulationTargetTemperature 
            => Door == DoorState.Closed
                ? TargetTemperature
                : SimulationEnvironment.Current.RoomTemperature;

        private bool ChangeSimulation()
        {
            switch (_temperatureState)
            {
                case DeviceTemperatureState.Heating:
                    return TargetTemperature <= Temperature;
                case DeviceTemperatureState.Cooling:
                    return TargetTemperature >= Temperature;
                default:
                    return true;
            }
        }

        private ISimulation Heating(TimeSpan interval)
        {
            if (Temperature.Equals(SimulationTargetTemperature))
            {
                return Idle();
            }
            return new IntervalSimulation<double>(
                dt => Temperature += dt, 
                () => 3.0, interval,
                () => Temperature >= SimulationTargetTemperature, 
                () => _temperatureState = DeviceTemperatureState.Heating,
                () => _temperatureState = DeviceTemperatureState.None
            );
        }

        private ISimulation Cooling(TimeSpan interval)
        {
            if (Temperature.Equals(SimulationTargetTemperature))
            {
                return Idle();
            }
            return new IntervalSimulation<double>(
                dt => Temperature -= dt, 
                () => .5, interval, 
                () => Temperature <= SimulationTargetTemperature, 
                () => _temperatureState = DeviceTemperatureState.Cooling, 
                () => _temperatureState = DeviceTemperatureState.None
            );
        }

        private ISimulation Idle()
        {
            return new NullSimulation(() => _temperatureState = DeviceTemperatureState.None);
        }
    }
}
