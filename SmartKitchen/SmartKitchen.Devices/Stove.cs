namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class Stove : DeviceBase
    {
        public double Temperature { get; set; }
        public bool HasPan { get; set; }

        public override string ToString()
        {
            return $"Temp.: {Temperature:##.00} / Pan: {HasPan}";
        }
    }
}
