using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;
namespace GameServer.Managers
{
    class EntityManager : Singleton<EntityManager>
    {
        int entityIndex = 0;

        List<Entity> allEntities = new List<Entity>();
        Dictionary<int, List<Entity>> mapEntities = new Dictionary<int, List<Entity>>();


        public void AddEntity(int mapId, Entity entity)
        {
            entity.EntityData.Id = ++entityIndex;
            allEntities.Add(entity);

            if (!mapEntities.ContainsKey(mapId))
            {
                mapEntities.Add(mapId, new List<Entity>());
            }
            mapEntities[mapId].Add(entity);
        }

        public void RemoveEntity(int mapId, Entity entity)
        {
            int id = entity.entityId;
            allEntities.Remove(entity);
            mapEntities[mapId].Remove(entity);
        }

        public void TransEntityMap(int mapId,int targetmap, Entity entity)
        {
            if (!mapEntities.ContainsKey(targetmap))
            {
                mapEntities.Add(targetmap, new List<Entity>());
            }
            mapEntities[mapId].Remove(entity);
            mapEntities[targetmap].Add(entity);
        }
    }
}
