using UnityEngine;
using System.Collections;
using System;

#pragma warning disable 0649

/// DataBase를 상속받는 클래스를 사용해 직렬화 파일 저장된 데이터 구조 변경 방법
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
class DataBase
{
    public int key;
    public string dataID;

    protected int curFileVersion=0;
    protected int savedFileVersion=0;

    public DataBase()
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

    [Serializable]
    class C : DataBase
    {
        public int[] arrInt;
        public float[] arrFloat;
        public string[] arrSTring;
    }

    [Serializable]
    class D : DataBase
    {
        public long longField;
        public long[] arrLong;
    }

    //x 변수명을 x2로 변경할 경우 대응 예제
    [Serializable]
    class StructChangeExample : DataBase
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