using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;
        public Common.Data.MapDefine CurrentMapData;
        public UnityEngine.GameObject CurrentCharacterObject;
        public int CurrentCharacterDbId = 0;
        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        public void SetupCharacter(List<SkillBridge.Message.NCharacterInfo> info)
        {
            this.userInfo.Player.Characters.Clear();
            this.userInfo.Player.Characters.AddRange(info);
        }

        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }

    }
}
