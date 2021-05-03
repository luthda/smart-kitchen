namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class Fridge 
        : DeviceBase
    {
        public DoorState Door { get; set; }
        public double Temperature { get; set; }

        public override string ToString()
        {
            return $"Temp.: {Temperature:##.00} / Door: {Door.ToString()}";
        }
    }
}
