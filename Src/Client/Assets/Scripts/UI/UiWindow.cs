using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public abstract class UiWindow : MonoBehaviour
{
    public enum WindowResult
    {
        None=0,
        Yes,
        No
    }
    private Dictionary<string, Component> compontCache;
    public System.Type Type { get { return this.GetType(); } }
    public delegate void OnCloseHandler(UiWindow sender, WindowResult result);
    public OnCloseHandler OnClose;
    private object param;
    public void Open(object param)
    {
        this.param = param;
        OnOpen(param);
        param = null;
    }
    private void Start()
    {
        //OnOpen(param);
        //param = null;
    }
    protected virtual void OnOpen(object param){}

    protected void Close(WindowResult result=WindowResult.None)
    {
        Managers.UiManager.Instance.Close(Type);
        if (OnClose != null) OnClose(this, result);

        this.OnClose = null;
        compontCache?.Clear();
    }

    protected void OnCloseClick()
    {
        this.Close();
    }
    protected void OnConfirmClick()
    {
        this.Close(WindowResult.Yes);
    }
    protected void OnCancelClick()
    {
        this.Close(WindowResult.No);
    }

    protected T GetUi<T>(string name) where T : Component
    {
        if (compontCache == null) compontCache = new Dictionary<string, Component>();
        return MyTools.FindFunc<T>(this.transform, name, ref compontCache);
    }

    protected void AddClick(string uiKey,UnityAction action,bool Replace=true)
    {
        Button button = GetUi<Button>(uiKey);
        if (button != null)
        {
            if (Replace) button.onClick.RemoveAllListeners();

            button.onClick.AddListener(action);
        }
    }
}
