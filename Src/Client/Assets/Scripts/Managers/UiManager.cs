using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Managers
{
    public class UiElement
    {
        public string ResPath;
        public bool IsCache;
        public GameObject Instance;
    }

    public class UiManager : Singleton<UiManager>
    {
        private Dictionary<Type, UiElement> UiDic;

        public UiManager()
        {
            UiDic = new Dictionary<Type, UiElement>();
            //在此加入需要的Ui
            UiDic.Add(typeof(TestUi), new UiElement { ResPath = "UI/TestUi", IsCache = false });
            UiDic.Add(typeof(UiLoading), new UiElement { ResPath = "UI/UiLoading", IsCache = false });
        }

        public T Show<T>(object param=null)where T:Component
        {
            Type type = typeof(T);
            if (UiDic.ContainsKey(type))
            {
                UiElement uiElement = UiDic[type];
                if (uiElement.Instance != null)
                {
                    uiElement.Instance.SetActive(true);
                }
                else
                {
                    UnityEngine.Object window = Resources.Load(uiElement.ResPath);
                    if (window == null)
                    {
                        Debug.LogError(type.ToString()+",Can not find window prefab at:" + uiElement.ResPath);
                        return default(T);
                    }
                    uiElement.Instance = GameObject.Instantiate(window) as GameObject;
                }
                if (uiElement.Instance.GetComponent<T>() == null)
                {
                   uiElement.Instance.AddComponent<T>();
                }
                T ui = uiElement.Instance.GetComponent<T>();
                (ui as UiWindow).Open(param);
                return ui;
            }
            return default(T);
        }

        public void Close(Type windowType)
        {
            if (UiDic.ContainsKey(windowType))
            {
                UiElement uiElement = UiDic[windowType];
                if (uiElement.IsCache)
                {
                    uiElement.Instance.SetActive(false);
                }
                else
                {
                    GameObject.Destroy(uiElement.Instance);
                    uiElement.Instance = null;
                }
            }
        }
    }
}