using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlovalVar
{
    public const string TableDataNamespace = "TableData";
    public const string TableEditDataPath = "TableEditData";
    public const string TableDataPath = "TableData";
}

public enum LogOption
{
    DEFAULT = 0,
    FSM = 1,
    FSM_Layer = 2,
    FSM_Reaction = 3,
    UI_Binder = 4,
}

public enum CooltimeState
{
    Ready,
    Cooling,
    MultiActive,
}

public enum CooltimeMode
{
    OneShot = 1,
    MultiShot = 2
}


public class SingleTon<T> where T : class, new()
{
    protected static object _instanceLock = new object();
    protected static volatile T _instance;
    public static T Inst
    {
        get
        {
            lock (_instanceLock)
            {
                if (null == _instance)
                {
                    _instance = new T();
                }
            }

            return _instance;
        }
    }
}