using System.Collections;
using System.Collections.Generic;
using Common.Data;
using Managers;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MapTools
{
    [MenuItem("Map Tool/ExportTeleporter")]
    public static void ExportTeleporter()
    {
        Scene currScene = EditorSceneManager.GetActiveScene();
        if (currScene.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请先保存当前场景", "确定");
            return;
        }

        string currSceneName = currScene.path;
        DataManager.Instance.Load();
        int mapCount = DataManager.Instance.Maps.Count;
        int mapIndex = 0;
        foreach (var map in DataManager.Instance.Maps)
        {
            mapIndex++;
            string mapPath = $"Assets/Levels/{map.Value.Resource}.unity";
            if (!System.IO.File.Exists(mapPath))
            {
                Debug.LogError($"map'{map.Value.Resource}' no exist");
                continue;
            }

            EditorSceneManager.OpenScene(mapPath, OpenSceneMode.Single);

            TeleporterObj[] teleporterObjs = GameObject.FindObjectsOfType<TeleporterObj>();
            int count = teleporterObjs.Length;
            int index = 0;
            foreach (var teleporter in teleporterObjs)
            {
                index++;
                if (!DataManager.Instance.Teleporters.ContainsKey(teleporter.ID))    
                {
                    Debug.LogError($"没有配置ID为{teleporter.ID}的传送点");
                    continue;
                }

                TeleporterDefine define = DataManager.Instance.Teleporters[teleporter.ID];
                if (define.MapID!=map.Value.ID)
                {
                    Debug.LogError($"配置ID为{teleporter.ID}的传送点的MapID" +
                                   $"与实际该传送点所在地图ID不一致");
                    continue;
                }

                define.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                define.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
                EditorUtility.DisplayProgressBar("提示",string.Format("Scene:{0} ({1}/{2})", map.Value.Name,mapIndex,mapCount), index * 1.0f / count);
            }
        }
        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene(currSceneName, OpenSceneMode.Single);
        EditorUtility.ClearProgressBar();
        EditorUtility.DisplayDialog("提示", "导出完成", "确定");
    }
}
