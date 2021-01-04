using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Text;
using System;
using System.IO;

using Common.Data;

using Newtonsoft.Json;
using SkillBridge.Message;
using Entities;

namespace Managers
{
    public class CharacterManager : Singleton<CharacterManager>, IDisposable
    {

        Dictionary<int, Character> characters = new Dictionary<int, Character>();

        public UnityAction<Character> OnCharacterEnter = null;
        public UnityAction<Character> OnCharacterLeave = null;
        public CharacterManager()
        {

        }

        public void Init()
        {

        }
        public void Dispose()
        {
            characters.Clear();
            OnCharacterEnter = null;
        }

        public Dictionary<int, Character> Characters
        {
            get
            {
                return characters;
            }
        }


        public void AddCharcter(SkillBridge.Message.NCharacterInfo character)
        {
            Character creatChaa = new Character(character);
            characters[creatChaa.entityId] = creatChaa;
            EntityManager.Instance.AddEntity(creatChaa);
            if (OnCharacterEnter != null) OnCharacterEnter.Invoke(characters[creatChaa.entityId]);
        }
        public void RemoveAcharacter(int entityId)
        {
            if (characters.ContainsKey(entityId))
            {
                Character character = characters[entityId];
                EntityManager.Instance.RemoveEntity(character);
                if (OnCharacterLeave != null) OnCharacterLeave.Invoke(character);
                characters.Remove(entityId);
            }
        }
        public void Clean()
        {
            foreach (var chara in characters)
            {
                EntityManager.Instance.RemoveEntity(chara.Value);
                if (OnCharacterLeave != null) OnCharacterLeave.Invoke(chara.Value);
            }
            characters.Clear();
        }
    }
}