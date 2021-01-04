using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Common.Data;
using System;

public class TestManager :Singleton<TestManager>
{
    public void Init()
    {
        NPCManager.Instance.RegisterDelegate(Common.Data.ENPCType.Task, OnTaskHandle);
        NPCManager.Instance.RegisterDelegate(Common.Data.ENPCType.Functional, onFunctionHandle);
    }

    private bool onFunctionHandle(NPCDefine define)
    {
        UiManager.Instance.Show<TestUi>(define.Name);
        return true;
    }

    private bool OnTaskHandle(NPCDefine define)
    {
        UiManager.Instance.Show<TestUi>(define.Name);
        return true;
    }
}
