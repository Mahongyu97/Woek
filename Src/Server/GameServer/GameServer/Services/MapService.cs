using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;
namespace GameServer.Services
{
    public class MapService : Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(this.OnEntitySyncReq);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(this.OnMapTeleportRequest);
        }



        public void Init()
        {
            MapManager.Instance.Init();
        }

        private void OnEntitySyncReq(NetConnection<NetSession> sender, MapEntitySyncRequest message)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat("OnEntitySyncReq:{0} {1} {2}", character.Id,character.Info.Name, message.entitySync.Entity.String());
            MapManager.Instance[character.Info.mapId].UpdateSync(message.entitySync);
        }


        internal void SendEntitySync(NetConnection<NetSession> connection, NEntitySync entitySync)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(entitySync);
            byte[] data = PackageHandler.PackMessage(message);
            connection.SendData(data, 0, data.Length);
        }

        private void OnMapTeleportRequest(NetConnection<NetSession> sender, MapTeleportRequest message)
        {
            int TeleportId = message.teleporterId;
            
            if (!DataManager.Instance.Teleporters.ContainsKey(TeleportId))
            {
                return;
            }
            Common.Data.TeleporterDefine from = DataManager.Instance.Teleporters[TeleportId];
            if (from.LinkTo==0 ||!DataManager.Instance.Teleporters.ContainsKey(from.LinkTo))
            {
                return;
            }
            Common.Data.TeleporterDefine to = DataManager.Instance.Teleporters[from.LinkTo];

            Character character = sender.Session.Character;
            Managers.MapManager.Instance[from.MapID].CharacterLeave(character);
            character.Position = to.Position;
            character.Direction = to.Direction;
            Managers.MapManager.Instance[to.MapID].CharacterEnter(sender,character);
            EntityManager.Instance.TransEntityMap(from.MapID, to.MapID, character);
        }
    }
}
