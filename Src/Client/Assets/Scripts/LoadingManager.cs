using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Common;
using SkillBridge.Message;
using ProtoBuf;
using Services;
using Managers;
public class LoadingManager : MonoBehaviour {

    public GameObject UITips;
    public GameObject UILoading;

    public Slider progressBar;
    public Text progressText;
    public Text progressNumber;

    // Use this for initialization
    IEnumerator Start()
    {
        DontDestroyOnLoad(this.gameObject);
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");
        UITips.SetActive(true);
        UILoading.SetActive(false);
        Setprogress(0);
        progressText.text = "加载配置";
        yield return new WaitForSeconds(2f);
        UILoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        UITips.SetActive(false);
        yield return DataManager.Instance.LoadData();
        Setprogress(0.5f);
        progressText.text = "初始化";
        //Init basic services
        MapService.Instance.Init();
        UserService.Instance.Init();
        NPCManager.Instance.Init();
        TestManager.Instance.Init();
        yield return null;
        Setprogress(0.7f);
        progressText.text = "加载场景";
        yield return SceneManager.Instance.LoadLevel("LoginAndSelect", (garg) =>
        {
            Setprogress(0.7f+garg*0.3f);
        });
       
        yield return null;
        //UILoading.SetActive(false);
        Destroy(this.gameObject);
    }

    private void Setprogress(float value)
    {
        progressBar.value = value;
        progressNumber.text = string.Format("{0}%", (int)(progressBar.value * 100));
    }
    // Update is called once per frame
}
