using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    class MinimapManager : Singleton<MinimapManager>
    {
        public UIMinimap UIMinimap;
        private BoxCollider mapBoundBox;
        public BoxCollider MapBoundBox { get => mapBoundBox; }


        public Transform Player
        {
            get
            {
                GameObject player = Models.User.Instance.CurrentCharacterObject;
                return player==null?null: player.transform;
            }
        }

       

        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }

        public void UpdateMiniMap(BoxCollider mapBoundingBox)
        {
            this.mapBoundBox = mapBoundingBox;
            if (this.UIMinimap != null) this.UIMinimap.UpdateMap();
        }
    }
}
