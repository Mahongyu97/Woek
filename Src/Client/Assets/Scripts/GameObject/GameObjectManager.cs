using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Managers;
class GameObjectManager:MonoSingleton<GameObjectManager>
{

    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();
    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        transform.position = Vector3.zero;
        CharacterManager.Instance.OnCharacterEnter += onCreateChar;
        CharacterManager.Instance.OnCharacterLeave += OnDeleteChar;
        StartCoroutine(InitCharacters());
    }

    IEnumerator InitCharacters()
    {
        foreach(var character in CharacterManager.Instance.Characters)
        {
            CreateCharacter(character.Value);
            yield return null;
        }
    }

    private void onCreateChar(Entities.Character character)
    {
        CreateCharacter(character);
    }
    private void OnDeleteChar(Entities.Character character)
    {
        if (!Characters.ContainsKey(character.entityId)) return;

        if (Characters[character.entityId] == null)
        {
            Characters.Remove(character.entityId);
            return;
        }
        Destroy(Characters[character.entityId]);
        Characters.Remove(character.entityId);
    }
    private void CreateCharacter(Entities.Character character)
    {
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            UnityEngine.Object obj = Resloader.Load<UnityEngine.Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj,this.transform);
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;

            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            Characters[character.entityId] = go;
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
        InitGameObj(character, Characters[character.entityId]);
    }

    private void InitGameObj(Entities.Character character, GameObject go)
    {
        EntityController ec = go.GetComponent<EntityController>();
        if (ec == null) ec = go.AddComponent<EntityController>();
        ec.entity = character;
        ec.isPlayer = character.IsPlayer;
        if (ec.anim == null) ec.anim = go.GetComponentInChildren<Animator>();
        if (ec.rb == null) ec.rb = go.GetComponentInChildren<Rigidbody>();
        if (ec.isPlayer)
        {
            PlayerInputController pc = go.GetComponent<PlayerInputController>();
            if (pc == null) pc = go.AddComponent<PlayerInputController>();
            //MainPlayerCamera.Instance.player = go;
            Models.User.Instance.CurrentCharacterObject = go;
            pc.enabled = true;
            pc.character = character;
            pc.entityController = ec;
            if (pc.rb == null) pc.rb = ec.rb;
        }
        else
        {
            PlayerInputController pc = go.GetComponent<PlayerInputController>();
            if (pc != null) pc.enabled = false;
        }
    }
}
