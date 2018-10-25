using System;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using DikeCADSocketShared.Classes;
using Newtonsoft.Json;

namespace DikeCADSocketClient
{
    public class DikeCADSocketClient : BaseScript
    {
        public DikeCADSocketClient()
        {
            EventHandlers["playerSpawned"] += new Action(SendUpdateMessage);
            
            Tick += OnTick;
        }

        private static bool _firstDataSent;
        private static async Task OnTick()
        {
            if (! _firstDataSent) await Delay(1000);

            SendUpdateMessage();

            await Delay(5000);
        }

        private static void SendUpdateMessage()
        {
            Player player = Game.Player;

            PlayerData playerData = new PlayerData(
                player.ServerId,
                player.Name,
                player.Character.Model.Hash,
                player.Character.Health,
                player.Character.Armor,
                player.Character.Position,
                player.Character.Heading,
                player.Character.Velocity,
                "",
                player.Character.IsAlive,
                player.Character.CurrentVehicle == null ? null : player.Character.CurrentVehicle.LocalizedName,
                player.Character.CurrentVehicle == null ? null : player.Character.CurrentVehicle.Model.Hash.ToString(),
                player.Character.CurrentVehicle == null ? (bool?) null : player.Character.CurrentVehicle.HasSiren,
                GetEmergencyVehicleCode(player.Character.CurrentVehicle)
            );

            TriggerServerEvent("dike:receiveData", JsonConvert.SerializeObject(playerData));

            _firstDataSent = true;
        }

        private static EEmergencyVehiclesCode GetEmergencyVehicleCode(Vehicle vehicle)
        {
            if (vehicle == null) return EEmergencyVehiclesCode.Code0;
            
            if (vehicle.ClassType != VehicleClass.Emergency) return EEmergencyVehiclesCode.Code0;
            
            return vehicle.IsSirenActive ? EEmergencyVehiclesCode.Code3 : EEmergencyVehiclesCode.Code1;
        }
    }
}