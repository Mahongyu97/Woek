using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Data;


namespace Managers
{
    public class NPCManager : Singleton<NPCManager>
    {
        public delegate bool NPCHandle(NPCDefine define);
        private Dictionary<ENPCType, NPCHandle> eventMap = new Dictionary<ENPCType, NPCHandle>();

        public void Init()
        {

        }

        public void RegisterDelegate(ENPCType type, NPCHandle handle)
        {
            if (eventMap.ContainsKey(type))
            {
                eventMap[type] += handle;
            }
            else
            {
                eventMap[type] = handle;
            }
        }

        public NPCDefine GetDefine(int id)
        {
            NPCDefine define = null;
            Managers.DataManager.Instance.NPCs.TryGetValue(id, out define);

            return define;
        }

        public bool TryNpcResponse(int id)
        {
            NPCDefine define = null;
            if (!Managers.DataManager.Instance.NPCs.TryGetValue(id, out define))
            {
                return false;
            }
            NPCHandle handle = null;
            if (!eventMap.TryGetValue(define.Type, out handle))
            {
                return false;
            }
            if (handle == null)
            {
                return false;
            }

            return handle(define);
        }
    }
}