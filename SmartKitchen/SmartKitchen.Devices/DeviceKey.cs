using System;

namespace Hsr.CloudSolutions.SmartKitchen.Devices
{
    public class DeviceKey
    {
        public DeviceKey(string type, Guid id)
        {
            Type = type;
            Id = id;
        }
        public DeviceKey(Type type, Guid id)
        {
            Type = type.Name;
            Id = id;
        }

        public string Type { get; set; }
        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DeviceKey;
            if (other == null)
            {
                return false;
            }
            return Type == other.Type 
                && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return $"{Type}{Id}".GetHashCode();
        }

        public override string ToString()
        {
            return $"{Type}_{Id}";
        }
    }
}
