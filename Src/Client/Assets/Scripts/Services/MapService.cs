using Common;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Managers;
using SkillBridge.Message;
namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {

        public int CurrMapId = 0;

        public MapService()
        {
            MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapEnter);
            MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapCharacterLeaveResponse>(this.OnMapLeave);
            MessageDistributer.Instance.Subscribe<SkillBridge.Message.MapEntitySyncResponse>(this.OnMapEntitySync);
        }




        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapCharacterEnterResponse>(this.OnMapEnter);
            MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapCharacterLeaveResponse>(this.OnMapLeave);
            MessageDistributer.Instance.Unsubscribe<SkillBridge.Message.MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        public void Init()
        {

        }
        
        private void OnMapEnter(object sender, MapCharacterEnterResponse message)
        {
            foreach(var character in message.Characters)
            {
                if (Models.User.Instance.CurrentCharacter==null && character.Id == Models.User.Instance.CurrentCharacterDbId)
                {
                    Models.User.Instance.CurrentCharacter = character;
                }
                CharacterManager.Instance.AddCharcter(character);
            }
            if (CurrMapId != message.mapId)
            {
                EnterMap(message.mapId);
                CurrMapId = message.mapId;
            }
        }



        private void EnterMap(int id)
        {
            Common.Data.MapDefine define = null;
            if(DataManager.Instance.Maps.TryGetValue(id,out define))
            {
                Models.User.Instance.CurrentMapData = define;
                SceneManager.Instance.LoadScene(define.Resource);
            }
            else
            {
                Debug.LogError("Can not find map ID:" + id);
            }
        }
        private void OnMapLeave(object sender, MapCharacterLeaveResponse message)
        {
            Log.InfoFormat("MapCharacterLeaveResponse :{0}", message.characterId);
            if (Models.User.Instance.CurrentCharacter!=null && message.characterId == Models.User.Instance.CurrentCharacter.Entity.Id)
            {
                CharacterManager.Instance.Clean();
                CurrMapId = 0;
                Models.User.Instance.CurrentCharacter = null;
               // Models.User.Instance.CurrentCharacterDbId = 0;
            }
            else CharacterManager.Instance.RemoveAcharacter(message.characterId);
        }

        public void SendEntitySync(EntityEvent entityEvent,NEntity entity)
        {
            //Debug.LogFormat("SendEntitySync:{0},{1}", entityEvent, entity.String());
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync
            {
                Event = entityEvent,
                Id = entity.Id,
                Entity = entity
            };
            NetClient.Instance.SendMessage(message);
        }
        
        private void OnMapEntitySync(object sender, MapEntitySyncResponse message)
        {
            EntityManager.Instance.mapEntiiesSync(message.entitySyncs);
        }

        internal void SendMapTeleporter(int TeleporterId)
        {
            Debug.LogFormat("SendMapTeleporter:{0}", TeleporterId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = TeleporterId;
            NetClient.Instance.SendMessage(message);
        }
    }
}
