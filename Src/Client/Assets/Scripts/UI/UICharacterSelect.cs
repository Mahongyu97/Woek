using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Services;
using SkillBridge.Message;
using Managers;
public class UICharacterSelect : MonoBehaviour {

    public GameObject panelCreate;
    public GameObject panelSelect;

    public GameObject btnCreateCancel;

    public InputField charName;
    CharacterClass charClass;

    public Transform uiCharList;
    public GameObject uiCharInfo;

    public List<GameObject> uiChars = new List<GameObject>();

    public Image[] titles;

    public Text descs;


    public Text[] names;

    private int selectCharacterIdx = -1;

    public UICharacterView characterView;
    public Button play;

    // Use this for initialization
    void Start()
    {
        play = MyTools.FindFunc<Button>(panelSelect.transform, "ButtonPlay");
        InitCharacterSelect(true);
        CharacterService.Instance.OnCreat += OnCharacterCreate;
    }
    private void OnDestroy()
    {
        CharacterService.Instance.OnCreat -= OnCharacterCreate;
    }

    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);
        if (init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();
            NUserInfo user = Models.User.Instance.Info;
            MyTools.SetChildCount(uiCharList, user.Player.Characters == null ? 0 : user.Player.Characters.Count, (index, item) =>
            {
                SetCharacterItem(item, user.Player.Characters[index], index);
            });
            if(user.Player.Characters!=null&& user.Player.Characters.Count > 0)
            {
                SetCharacterItem(uiCharList.GetChild(0).gameObject, user.Player.Characters[0], 0);
            }
        }
    }
    private void SetCharacterItem(GameObject item,NCharacterInfo info,int index)
    {
        item.GetComponent<UICharInfo>().info = info;
        item.GetComponent<UICharInfo>().Set();
        item.GetComponent<Button>().onClick.RemoveAllListeners();
        item.GetComponent<Button>().onClick.AddListener(() =>
        {
            characterView.CurrectCharacter = info.Tid;
            play.onClick.RemoveAllListeners();
            play.onClick.AddListener(() =>
            {
                selectCharacterIdx = index;
                User.Instance.CurrentCharacterDbId =  User.Instance.Info.Player.Characters[index].Id;
                OnClickPlay();
            });
        });
    }
    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
    }
    

    public void OnClickCreate()
    {
        CharacterService.Instance.SendCreat(charName.text, (int)charClass);
    }

    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;

        characterView.CurrectCharacter = charClass;
    }


    void OnCharacterCreate(Result result, string message, List<NCharacterInfo> characterInfos)
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true);
            characterView.CurrectCharacter = -1;
            MessageBox.Show("创建成功");
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }

    public void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        User.Instance.CurrentCharacterDbId = cha.Id;
        characterView.CurrectCharacter = idx;
    }
    public void OnClickPlay()
    {
        if (selectCharacterIdx >= 0)
        {
            UserService.Instance.SendGameIn(selectCharacterIdx);
        }
    }
}
