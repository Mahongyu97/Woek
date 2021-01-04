using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestUi : UiWindow
{
    protected override void OnOpen(object param)
    {
        base.OnOpen(param);
        AddClick("CloseBtn", OnCloseClick);
        GetUi<Text>("TestTip").text = (string)param;
    }
}
