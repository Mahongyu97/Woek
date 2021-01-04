using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using Entities;
namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemove();
        void OnStateChange(EntityEvent entityEvent);
        void OnTransformChange();
    }
    class EntityManager : Singleton<EntityManager>
    {
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        Dictionary<int, IEntityNotify> noTifys = new Dictionary<int, IEntityNotify>();

        public void RegistNotify(int entityId, IEntityNotify entity)
        {
            noTifys[entityId] = entity;
        }
        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(Entity entity)
        {
            if (entities.ContainsKey(entity.entityId)) entities.Remove(entity.entityId);

            if (noTifys.ContainsKey(entity.entityId))
            {
                noTifys[entity.entityId].OnEntityRemove();
                noTifys.Remove(entity.entityId);
            }
        }

        public void mapEntiiesSync(List<NEntitySync> messageEntitySyncs)
        {
            foreach (var NEntitySync in messageEntitySyncs)
            {
                Entity entity = null;
                if (entities.TryGetValue(NEntitySync.Id,out entity))
                {
                    entity.EntityData = NEntitySync.Entity;
                }

                IEntityNotify notify = null;
                if (noTifys.TryGetValue(NEntitySync.Id,out notify))
                {
                    notify.OnStateChange(NEntitySync.Event);
                    notify.OnTransformChange();
                }
            }
        }
    }
}
