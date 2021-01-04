using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MyTools {
    
    /// <summary>
    /// 查找并更新缓存
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="targetName"></param>
    /// <param name="cache"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindFunc<T>(Transform parent, string targetName, ref Dictionary<string, Component> cache) where T : Component
    {
        if (cache.ContainsKey(targetName)) return cache[targetName] as T;
        T target = parent.GetComponent<T>();
        if (target != null && target.transform.name == targetName)
        {
            if (!cache.ContainsKey(targetName)) cache.Add(targetName, target);
            return target;
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            target = FindFunc<T>(child, targetName, ref cache);
            if (target != null)
            {
                return target;
            }
        }
        return null;
    }

    /// <summary>
    /// 查找组件
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="targetName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindFunc<T>(Transform parent, string targetName) where T : Component
    {
        T target = parent.GetComponent<T>();
        if (target != null && target.transform.name == targetName)
        {
            return target;
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            target = FindFunc<T>(child, targetName);
            if (target != null)
            {
                return target;
            }
        }
        return null;
    }
    /// <summary>
    /// 复制或隐藏子物体到相应数量
    /// </summary>
    /// <param name="root"></param>
    /// <param name="count"></param>
    /// <param name="setter"></param>
    public static void SetChildCount(Transform root,int count,UnityAction<int,GameObject> setter)
    {
        int childcount = root.childCount;
        if (childcount == 0) return;
        for(int i = 0; i < childcount; i++)
        {
            root.GetChild(i).gameObject.SetActive(false);
        }
        if (count> childcount)
        {
            for(int i=0;i< count- childcount; i++)
            {
                GameObject child = GameObject.Instantiate(root.GetChild(0).gameObject);
                RectTransform trans = child.GetComponent<RectTransform>();
                trans.localScale = Vector3.one;
                trans.SetParent(root,false);
                trans.SetAsLastSibling();
            }
        }
        for (int i = 0; i < count; i++)
        {
            GameObject child = root.GetChild(i).gameObject;
            RectTransform trans = child.GetComponent<RectTransform>();
            trans.localScale = Vector3.one;
            child.gameObject.SetActive(true);
            if (setter != null) setter(i, child);
        }
    }
}
