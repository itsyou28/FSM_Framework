using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

#pragma warning disable 0649

/// TableDataBase를 상속받는 클래스를 사용해 직렬화 파일 저장된 데이터 구조 변경 방법
///     -> 변수명을 변경할 경우
///         -> 생성자에서 curFileVersion을 증가
///         -> 변경하는 기존 변수에 Obsolet, NonSerialze 어트리뷰트 지정
///         -> 변경할 이름을 사용하여 새 변수를 추가
///         -> UpdateLatestDataStruct 함수를 이용하여 기존 변수값을 변경 변수에 대입         
///         -> 해당 데이터를 파일 저장 후 파일에 해당 변수값이 남아있지 않을 경우 변수를 삭제
///     -> 변수를 제거할 경우
///         -> Obsolet, NonSerialize 사용
///         -> 해당 데이터를 파일 저장 후 파일에 해당 변수가 남아있지 않을 경우 변수를 삭제
///     -> 변수를 추가할 경우
///         -> 별도의 작업 없이 변수 추가
[Serializable]
public class TableDataBase
{
    public int key;
    public string dataID;

    protected int curFileVersion=0;
    protected int savedFileVersion=0;

    public TableDataBase()
    {
        key = DateTime.Now.GetHashCode() + SystemInfo.deviceUniqueIdentifier.GetHashCode();
        dataID = key.ToString();
    }

    public virtual bool UpdateLatestDataStruct()
    {
        if (savedFileVersion == curFileVersion)
            return false;

        savedFileVersion = curFileVersion;
        return true;
    }
}

public class TableAccess
{
    private static TableAccess instance = null;
    public static TableAccess Inst
    {
        get
        {
            if (instance == null)
                instance = new TableAccess();
            return instance;
        }
    }

    private TableAccess()
    {
        var classList = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == GlovalVar.TableDataNamespace)
                      .ToList();

        foreach (var t in classList)
            LoadTableData(t.Name);
    }

    Dictionary<string, Dictionary<int, object>> tableContainer = new Dictionary<string, Dictionary<int, object>>();

    void LoadTableData(string name)
    {
        Dictionary<int, object> dataList = null;

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //에디터일 경우 Resources 하위에서 파일을 로드한다
            if (FileManager.Inst.CheckFileExists("Resources/"+ GlovalVar.TableDataPath+"/" + name + ".bytes"))
            {
                dataList = FileManager.Inst.FileLoad("Resources/" + GlovalVar.TableDataPath, name + ".bytes") as Dictionary<int, object>;
            }
        }
        else
        {
            //에디터가 아닐 경우 PersistentDataPath에서 파일을 체크하고 없을 경우 리소스에서 파일을 읽어온다.
            dataList = FileManager.Inst.ResourceLoad(GlovalVar.TableDataPath+"/" + name) as Dictionary<int, object>;
            if (dataList == null)
                dataList = FileManager.Inst.FileLoad(GlovalVar.TableDataPath, name + ".bytes") as Dictionary<int, object>;
        }

        if(dataList != null)
            tableContainer.Add(name, dataList);
    }

    public T GetData<T>(int key) where T : TableDataBase
    {
        Dictionary<int, object> p;
        object result = null;
                
        if(tableContainer.TryGetValue(typeof(T).Name, out p))
            p.TryGetValue(key, out result);

        return result as T;
    }
}

namespace TableData
{
    [Serializable]
    public class LvTable : TableDataBase
    {
        public int lv;
        public int[] arrNextLvExp;
    }

    [Serializable]
    public class A : TableDataBase
    {
        public int a;
        public float b;
        public string c;
    }

    [Serializable]
    public class B : TableDataBase
    {
        public int d;
        public float e;
        public string f;        
    }

    [Serializable]
    public class C : TableDataBase
    {
        public int[] arrInt;
        public float[] arrFloat;
        public string[] arrSTring;
    }

    [Serializable]
    public class D : TableDataBase
    {
        public long longField;
        public long[] arrLong;
    }

    //x 변수명을 x2로 변경할 경우 대응 예제
    [Serializable]
    public class StructChangeExample : TableDataBase
    {
        public StructChangeExample()
        {
            curFileVersion = 1;
            savedFileVersion = curFileVersion;
        }

        public int x2;

        [Obsolete]
        [NonSerialized]
        public int x;
        
        public override bool UpdateLatestDataStruct()
        {
            if(base.UpdateLatestDataStruct())
            {
                x2 = x;
            }

            return true;
        }
    }
}

#pragma warning restore 0649