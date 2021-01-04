using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;

namespace Services
{
    class CharacterService : Singleton<CharacterService>, IDisposable
    {
        public UnityEngine.Events.UnityAction<Result, string,List<NCharacterInfo>> OnCreat;

        public CharacterService()
        {
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserLogin);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserLogin);
        }

        public void Init()
        {

        }



        public void SendCreat(string name, int chClass)
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();
            message.Request.createChar.Class = (CharacterClass)chClass;
            message.Request.createChar.Name = name;
            NetClient.Instance.SendMessage(message);
        }

        void OnUserLogin(object sender, UserCreateCharacterResponse response)
        {
            if (this.OnCreat != null)
            {
                Models.User.Instance.SetupCharacter(response.Characters);
                this.OnCreat(response.Result, response.Errormsg, response.Characters);
            }
        }
    }
}
