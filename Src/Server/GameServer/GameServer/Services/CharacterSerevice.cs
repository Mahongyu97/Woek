using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Services;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    class CharacterSerevice : Singleton<CharacterSerevice>
    {
        public CharacterSerevice()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<SkillBridge.Message.UserCreateCharacterRequest>(OnCreatCharacter);
        }
        public void Init()
        {

        }
        private void OnCreatCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest message)
        {
            NetMessage rsp = new NetMessage();
            rsp.Response = new NetMessageResponse();
            rsp.Response.createChar = new UserCreateCharacterResponse();

            if (sender.Session.User == null)
            {
                rsp.Response.createChar.Result = Result.Failed;
                rsp.Response.createChar.Errormsg = "未登录，请重新登录";
            }
            else
            {
                if (/*Illegal(message.Class)*/false)
                {
                    rsp.Response.createChar.Result = Result.Failed;
                    rsp.Response.createChar.Errormsg = $"职业错误，不含职业Id{message.Class}";
                }
                else if(/*illegal(message.Name)*/false)
                {
                    rsp.Response.createChar.Result = Result.Failed;
                    rsp.Response.createChar.Errormsg = $"命名非法";
                }
                else
                {
                    rsp.Response.createChar.Result = Result.Success;
                    TCharacter character = new TCharacter();
                    character.Name = message.Name;
                    character.Class = (int)message.Class;
                    character.TID = (int)message.Class;
                    character.MapID = 1;
                    character.MapPosX = 5000;
                    character.MapPosY = 5000;
                    character.MapPosZ = 5000;
                    sender.Session.User.Player.Characters.Add(character);
                    DBService.Instance.Entities.SaveChanges();
                    foreach (var c in sender.Session.User.Player.Characters)
                    {
                        NCharacterInfo info = new NCharacterInfo();
                        info.Id = c.ID;
                        info.Name = c.Name;
                        info.Class = (CharacterClass)c.Class;
                        rsp.Response.createChar.Characters.Add(info);
                    }
                }
                byte[] mes = PackageHandler.PackMessage(rsp);
                sender.SendData(mes, 0, mes.Length);
            }
        }
    }
}
