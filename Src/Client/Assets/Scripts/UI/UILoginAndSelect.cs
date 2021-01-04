using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SkillBridge.Message;
public class UILoginAndSelect : MonoBehaviour
{
    private GameObject LoginPart;
    private GameObject SelectPart;

    void Start()
    {
        LoginPart = MyTools.FindFunc<Transform>(this.transform, "UILogin").gameObject;
        SelectPart = MyTools.FindFunc<Transform>(this.transform, "UICharacterSelect").gameObject;
        bool login = Models.User.Instance.Info == null;
        LoginPart.SetActive(login);
        SelectPart.SetActive(!login);
        Services.UserService.Instance.OnLogin += OnLogin;
    }
    
    void OnLogin(Result result, string message)
    {
        if (result == Result.Success)
        {
            LoginPart.SetActive(false);
            SelectPart.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (Services.UserService.Instance.OnLogin != null) Services.UserService.Instance.OnLogin -= OnLogin;
    }
}
