using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Managers;
public class UIMain : MonoSingleton<UIMain> {

    public Text avatarName;
    public Text avatarLevel;

    // Use this for initialization
    protected override void OnStart()
    {
        base.OnStart();
        this.UpdateAvatar();
        MyTools.FindFunc<Button>(transform, "Returnbtn").onClick.AddListener(() =>
        {
            UserLeave();
            
        });
    }

    void UpdateAvatar()
    {
        this.avatarName.text = string.Format("{0}[{1}]", User.Instance.CurrentCharacter.Name, User.Instance.CurrentCharacter.Id);
        this.avatarLevel.text = User.Instance.CurrentCharacter.Level.ToString();
    }
	
	public void UserLeave()
	{
        SceneManager.Instance.LoadScene("LoginAndSelect");
        Services.UserService.Instance.SendGameLeave();
	}
}
