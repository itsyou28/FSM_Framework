using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public partial class UI_ReflectionEditor : MonoBehaviour
{
    [SerializeField]
    GameObject editRowOrigin;
    [SerializeField]
    Transform editRowPanel;

    ObjectPool<GameObject> editRowPool;

    void InitFieldEditor()
    {
        editRowPool = new ObjectPool<GameObject>(5, 3, CreateEditRow);
    }

    GameObject CreateEditRow()
    {
        GameObject newRow = Instantiate(editRowOrigin);

        newRow.transform.SetParent(editRowPanel, false);
        newRow.SetActive(false);

        return newRow;
    }

    void PushBackAllEditRow()
    {
        for (int i = 0; i < editRowPanel.transform.childCount; i++)
        {
            GameObject obj = editRowPanel.transform.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                editRowPool.Push(obj);
            }
        }
    }

    object curData;

    public void SetFieldRow(object data)
    {
        curData = data;
        CreateFieldsInfo(data);
    }

    /// <summary>
    /// _data가 null이어도 모든 Field를 표시하기 위해 해당 type의 instance를 자동생성시킨다. 
    /// </summary>
    void CreateFieldsInfo(object _obj)
    {
        PushBackAllEditRow();

        //필드목록 생성
        FieldInfo[] arrFields = _obj.GetType().GetFields();

        for (int idx = 0; idx < arrFields.Length; idx++)
        {
            object fieldValue = arrFields[idx].GetValue(_obj);

            if (!arrFields[idx].FieldType.IsArray)
            {
                //Primitive type이 아니고 값이 null일 경우 인스턴스 생성
                if (fieldValue == null && arrFields[idx].FieldType.Name != "String")
                {
                    fieldValue = CreateInstance(arrFields[idx].FieldType);
                    arrFields[idx].SetValue(_obj, fieldValue);
                }

                CreateRow(arrFields[idx], fieldValue, _obj);
            }
            else
            {
                //변수가 배열이고 null일 경우
                if (fieldValue == null)
                {
                    //길이가 1인 배열 인스턴스 생성
                    fieldValue = CreateInstance(arrFields[idx].FieldType, true);

                    arrFields[idx].SetValue(_obj, fieldValue);
                }

                CreateRow(arrFields[idx], fieldValue, _obj, true);
            }

            if (arrFields[idx].FieldType.IsArray)//배열일 경우 배열 순회
                CreateArrayRow(arrFields[idx], _obj);
            else if (arrFields[idx].FieldType.IsClass && arrFields[idx].FieldType.Name != "String")//클래스 타입일 경우 재귀
                CreateFieldsInfo(fieldValue);
        }
    }

    /// <summary>
    /// 배열 객체를 순회하며 Row 생성
    /// </summary>
    void CreateArrayRow(FieldInfo _fieldInfo, object _data)
    {
        Array arr = (Array)_fieldInfo.GetValue(_data);

        for (int i = 0; i < arr.Length; i++)
        {
            object arrElement = arr.GetValue(i);

            //배열 객체가 비어있을 경우 객체 생성
            if (arrElement == null)
            {
                arrElement = CreateInstance(_fieldInfo.FieldType, true, false);
                arr.SetValue(arrElement, i);
            }

            FieldInfo[] arrFields = arrElement.GetType().GetFields();

            for (int idx = 0; idx < arrFields.Length; idx++)
            {
                //클래스이거나 배열인 경우
                if (arrFields[idx].FieldType.IsClass && arrFields[idx].FieldType.Name != "String")
                {
                    if (arrFields[idx].FieldType.IsArray)
                    {
                        object subArrElement = arrFields[idx].GetValue(arrElement);

                        if (subArrElement == null)
                        {
                            //배열 객체 생성
                            subArrElement = CreateInstance(arrFields[idx].FieldType, true);
                            arrFields[idx].SetValue(arrElement, subArrElement);
                        }
                        CreateRow(arrFields[idx], arrFields[idx].GetValue(arrElement), arrElement, arrFields[idx].FieldType.IsArray);

                        //해당 배열 객체의 필드 표시를 위한 재귀 호출
                        CreateArrayRow(arrFields[idx], arrElement);
                    }
                    else
                    {
                        object subElement = arrFields[idx].GetValue(arrElement);

                        if (subElement == null)
                        {
                            //객체 생성
                            subElement = CreateInstance(arrFields[idx].FieldType);
                            arrFields[idx].SetValue(arrElement, subElement);
                        }
                        CreateRow(arrFields[idx], arrFields[idx].GetValue(arrElement), arrElement, arrFields[idx].FieldType.IsArray);

                        //해당 객체내의 필드 표시를 위한 재귀 호출
                        CreateFieldsInfo(subElement);
                    }

                }
                //일반 변수인 경우
                else
                    CreateRow(arrFields[idx], arrFields[idx].GetValue(arrElement), arrElement, arrFields[idx].FieldType.IsArray);
            }
        }
    }

    /// <summary>
    /// type에 해당하는 instance 생성
    /// </summary>
    /// <param name="type"></param>
    /// <param name="_bIsArray">요청 타입이 배열=true</param>
    /// <param name="_bOutIsArray">생성 타입이 배열=true</param>
    /// <param name="_nArrLength">배열일 경우 배열 길이 지정</param>
    /// <returns></returns>
    object CreateInstance(Type type, bool _bIsArray = false, bool _bOutIsArray = true, int _nArrLength = 1)
    {
        if (_bIsArray && _bOutIsArray)
            return Array.CreateInstance(type, _nArrLength);

        return Activator.CreateInstance(type);
    }

    /// <summary>
    /// 필드 이름 및 해당 필드 변수를 변경할 수 있는 UI Input Field를 생성
    /// </summary>
    void CreateRow(FieldInfo _fieldInfo, object _value = null, object _data = null, bool isArray = false)
    {
        if (_data != null && _value != null)
            _fieldInfo.SetValue(_data, _value);

        GameObject obj = Instantiate(editRowOrigin);
        obj.transform.SetParent(editRowPanel, false);

        Text labelText = obj.transform.GetChild(0).GetComponent<Text>();
        InputField input = obj.transform.GetChild(1).GetComponent<InputField>();

        if (isArray)
            labelText.text = "(" + _fieldInfo.FieldType.Name + ")" + _fieldInfo.Name + " (array Size)";
        else
            labelText.text = "(" + _fieldInfo.FieldType.Name + ")" + _fieldInfo.Name;

        input.onEndEdit.RemoveAllListeners();

        if (isArray)
            input.onEndEdit.AddListener((str) => OnEndEditArr(str, _fieldInfo, _data));
        else
            input.onEndEdit.AddListener((str) => OnEndEdit(str, _fieldInfo, _data));

        if (_fieldInfo.Name == "dataID")
            input.onEndEdit.AddListener((str) => bindCurDataID.Value = str);

        if (_value != null)
            input.text = _value.ToString();

        if (isArray)
        {
            Array arr = _value as Array;

            input.text = (arr != null) ? arr.Length.ToString() : "Array Size Here";
        }

        obj.SetActive(true);
    }

    /// <summary>
    /// 일반 변수 타입 값이 수정됐을 경우 데이터 적용
    /// </summary>
    /// <param name="_editString"></param>
    /// <param name="_fieldInfo"></param>
    /// <param name="_data"></param>
    public void OnEndEdit(string _editString, FieldInfo _fieldInfo, object _data)
    {
        try
        {
            switch (Type.GetTypeCode(_fieldInfo.FieldType))
            {
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    int nData;
                    int.TryParse(_editString, out nData);
                    _fieldInfo.SetValue(_data, nData);
                    break;
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    long lData;
                    long.TryParse(_editString, out lData);
                    break;
                case TypeCode.String:
                    _fieldInfo.SetValue(_data, _editString);
                    break;
                case TypeCode.Single:
                    float fData;
                    float.TryParse(_editString, out fData);
                    Debug.Log(fData);
                    _fieldInfo.SetValue(_data, fData);
                    break;
                case TypeCode.Double:
                    double dData;
                    double.TryParse(_editString, out dData);
                    Debug.Log(dData);
                    _fieldInfo.SetValue(_data, dData);
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(_editString + " // " + (_fieldInfo != null ? _fieldInfo.Name : "") + " // " + (_data != null ? _data.GetType().Name : ""));
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// 배열 Size 변경시 배열 확장
    /// 기존 배열 데이터가 삭제됨!!
    /// </summary>
    public void OnEndEditArr(string _editString, FieldInfo _fieldInfo, object _data)
    {
        int nLength;
        int.TryParse(_editString, out nLength);

        Debug.Log("OnEndEditArr : " + nLength);
        _fieldInfo.SetValue(_data, CreateInstance(_fieldInfo.FieldType, true, true, nLength));
        
        CreateFieldsInfo(curData);
    }

}
