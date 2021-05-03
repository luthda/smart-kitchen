namespace Hsr.CloudSolutions.SmartKitchen.Simulator.Common.Engine
{
    public class SimulationEnvironment
    {
        private static SimulationEnvironment _instance;

        public static SimulationEnvironment Current => _instance ??= new SimulationEnvironment();

        public double RoomTemperature => 27;
    }
}
