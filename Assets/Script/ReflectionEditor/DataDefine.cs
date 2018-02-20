using UnityEngine;
using System.Collections;
using System;

#pragma warning disable 0649

[Serializable]
class DataBase
{
    public int key;
    public string dataID;

    public DataBase()
    {
        key = DateTime.Now.GetHashCode() + SystemInfo.deviceUniqueIdentifier.GetHashCode();
        dataID = key.ToString();
    }
}

namespace Data
{
    [Serializable]
    class A : DataBase
    {
        public int a;
        public float b;
        public string c;
    }

    [Serializable]
    class B : DataBase
    {
        public int d;
        public float e;
        public string f;        
    }
}

#pragma warning restore 0649