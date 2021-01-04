using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiLoading : UiWindow
{
    private Slider Slider;
    private Text Tip;
    protected override void OnOpen(object param)
    {
        base.OnOpen(param);
        DontDestroyOnLoad(gameObject);
        GetUi<Text>("ProgressText").text = (string)param;
        Slider = GetUi<Slider>("Slider");
        Tip = GetUi<Text>("ProgressNumber");
    }

    public void SetValue(float value)
    {
        Slider.value = value;
        Tip.text = string.Format("{0}%", (int)(value * 100));
    }
}
