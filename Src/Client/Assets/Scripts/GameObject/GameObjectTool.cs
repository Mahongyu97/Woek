using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameObjectTool
{
    public const float UNIT_MULTIPLE = 100F;
    public static Vector3 LogicToWorld(NVector3 vector)
    {
        return new Vector3(vector.X / UNIT_MULTIPLE, vector.Z / UNIT_MULTIPLE, vector.Y / UNIT_MULTIPLE);
    }

    public static Vector3 LogicToWorld(Vector3Int vector)
    {
        return new Vector3(vector.x / UNIT_MULTIPLE, vector.z / UNIT_MULTIPLE, vector.y / UNIT_MULTIPLE);
    }

    public static float LogicToWorld(int val)
    {
        return val / UNIT_MULTIPLE;
    }

    public static int WorldToLogic(float val)
    {
        return Mathf.RoundToInt(val * UNIT_MULTIPLE);
    }

    public static NVector3 WorldToLogicN(Vector3 vector)
    {
        return new NVector3()
        {
            X = Mathf.RoundToInt(vector.x * UNIT_MULTIPLE),
            Y = Mathf.RoundToInt(vector.z * UNIT_MULTIPLE),
            Z = Mathf.RoundToInt(vector.y * UNIT_MULTIPLE)
        };
    }

    public static Vector3Int WorldToLogic(Vector3 vector)
    {
        return new Vector3Int()
        {
            x = Mathf.RoundToInt(vector.x * UNIT_MULTIPLE),
            y = Mathf.RoundToInt(vector.z * UNIT_MULTIPLE),
            z = Mathf.RoundToInt(vector.y * UNIT_MULTIPLE)
        };
    }


    public static bool EntityUpdate(NEntity entity,UnityEngine.Vector3 position, Quaternion rotation,float speed)
    {
        NVector3 pos = WorldToLogicN(position);
        NVector3 dir = WorldToLogicN(rotation.eulerAngles);
        int spd = WorldToLogic(speed);
        bool updated = false;
        if(!entity.Position.Equal(pos))
        {
            entity.Position = pos;
            updated = true;
        }
        if (!entity.Direction.Equal(dir))
        {
            entity.Direction = dir;
            updated = true;
        }
        if(entity.Speed!= spd)
        {
            entity.Speed = spd;
            updated = true;
        }
        return updated;
    }
}