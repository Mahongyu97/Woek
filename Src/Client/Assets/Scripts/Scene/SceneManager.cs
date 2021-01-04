using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManager : MonoSingleton<SceneManager>
{
    UnityAction<float> onProgress = null;

    // Use this for initialization
    protected override void OnStart()
    {
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(name));
    }

    public IEnumerator LoadLevel(string name,UnityAction<float> onProgress=null)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        UiLoading ui = Managers.UiManager.Instance.Show<UiLoading>("加载场景");
        onProgress += ui.SetValue;
        yield return null;
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = true;
        async.completed += LevelLoadCompleted;
        if (onProgress != null) this.onProgress = onProgress;

        while (!async.isDone)
        {
            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        StartCoroutine(Complete(obj));
    }
    IEnumerator Complete(AsyncOperation obj)
    {
        yield return new WaitForEndOfFrame();
        if (onProgress != null)
            onProgress(1f);
        Debug.Log("LevelLoadCompleted:" + obj.progress);
        this.onProgress = null;
        Managers.UiManager.Instance.Close(typeof(UiLoading));
    }
}
