using CitizenFX.Core;

namespace DikeCADSocketShared.Classes
{
    public class PlayerData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ModelHash { get; set; }
        
        public int Health { get; set; }
        public int Armor { get; set; }
        
        public Vector3 Position { get; set; }
        public float Heading { get; set; }
        public Vector3 Velocity { get; set; }
        
        public string Job { get; set; }
        
        public bool IsAlive { get; set; }
        
        public string VehicleLocalizedName { get; set; }
        public string VehicleModelName { get; set; }
        public bool? IsEmergencyVehicle { get; set; }
        public EEmergencyVehiclesCode Code { get; set; }

        public PlayerData(int id, string name, int modelHash, int health, int armor, Vector3 position, float heading,
            Vector3 velocity, string job, bool isAlive, string vehicleLocalizedName, string vehicleModelName, bool? isEmergencyVehicle,
            EEmergencyVehiclesCode code)
        {
            ID = id;
            Name = name;
            ModelHash = modelHash;
            Health = health;
            Armor = armor;
            Position = position;
            Heading = heading;
            Velocity = velocity;
            Job = job;
            IsAlive = isAlive;
            VehicleLocalizedName = vehicleLocalizedName;
            VehicleModelName = vehicleModelName;
            IsEmergencyVehicle = isEmergencyVehicle;
            Code = code;
        }
    }
}