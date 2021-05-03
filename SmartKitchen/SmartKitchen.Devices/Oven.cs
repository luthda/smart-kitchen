namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class Oven : DeviceBase
    {
        public DoorState Door { get; set; }
        public double Temperature { get; set; }

        public override string ToString()
        {
            return $"Temp.: {Temperature:##.00} / Door: {Door.ToString()}";
        }
    }
}
