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
    void CreateFieldsInfo(object targetData)
    {
        PushBackAllEditRow();

        //필드목록 생성
        FieldInfo[] arrFields = targetData.GetType().GetFields();

        for (int idx = 0; idx < arrFields.Length; idx++)
        {
            object fieldValue = arrFields[idx].GetValue(targetData);

            if (!arrFields[idx].FieldType.IsArray)
            {
                //Primitive type이 아니고 값이 null일 경우 인스턴스 생성
                if (fieldValue == null && arrFields[idx].FieldType != typeof(string))
                {
                    fieldValue = CreateInstance(arrFields[idx].FieldType);
                    arrFields[idx].SetValue(targetData, fieldValue);
                }

                CreateRow(arrFields[idx], fieldValue, targetData);
            }
            else
            {
                //변수가 배열이고 null일 경우
                if (fieldValue == null)
                {
                    //길이가 1인 배열 인스턴스 생성
                    fieldValue = CreateInstance(arrFields[idx].FieldType, true);
                    arrFields[idx].SetValue(targetData, fieldValue);
                }

                CreateRow(arrFields[idx], fieldValue, targetData, true);
            }

            if (arrFields[idx].FieldType.IsArray)//배열일 경우 배열 순회
                CreateArrayRow(arrFields[idx], targetData);
            else if (arrFields[idx].FieldType.IsClass && arrFields[idx].FieldType != typeof(string))//클래스 타입일 경우 재귀
                CreateFieldsInfo(fieldValue);
        }
    }

    /// <summary>
    /// 배열 객체를 순회하며 Row 생성
    /// </summary>
    void CreateArrayRow(FieldInfo arrFieldInfo, object targetData)
    {
        Array arr = (Array)arrFieldInfo.GetValue(targetData);

        for (int i = 0; i < arr.Length; i++)
        {
            object arrElement = arr.GetValue(i);

            //배열 객체가 비어있을 경우 객체 생성
            if (arrElement == null )
            {
                if (arrFieldInfo.FieldType.GetElementType() == typeof(string))
                    arrElement = "";
                else
                    arrElement = CreateInstance(arrFieldInfo.FieldType, true, false);

                arr.SetValue(arrElement, i);
            }

            if(arrElement.GetType().IsPrimitive || arrFieldInfo.FieldType.GetElementType() == typeof(string))
            {
                CreateRow(arr, i);
                continue;
            }

            FieldInfo[] arrSubFields = arrElement.GetType().GetFields();

            for (int idx = 0; idx < arrSubFields.Length; idx++)
            {
                //클래스이거나 배열인 경우
                if (arrSubFields[idx].FieldType.IsClass && arrSubFields[idx].FieldType != typeof(string))
                {
                    if (arrSubFields[idx].FieldType.IsArray)
                    {
                        object subElement = arrSubFields[idx].GetValue(arrElement);

                        if (subElement == null)
                        {
                            //배열 객체 생성
                            subElement = CreateInstance(arrSubFields[idx].FieldType, true);
                            arrSubFields[idx].SetValue(arrElement, subElement);
                        }
                        CreateRow(arrSubFields[idx], arrSubFields[idx].GetValue(arrElement), arrElement, arrSubFields[idx].FieldType.IsArray);

                        //해당 배열 객체의 필드 표시를 위한 재귀 호출
                        CreateArrayRow(arrSubFields[idx], arrElement);
                    }
                    else
                    {
                        object subElement = arrSubFields[idx].GetValue(arrElement);

                        if (subElement == null)
                        {
                            //객체 생성
                            subElement = CreateInstance(arrSubFields[idx].FieldType);
                            arrSubFields[idx].SetValue(arrElement, subElement);
                        }
                        CreateRow(arrSubFields[idx], arrSubFields[idx].GetValue(arrElement), arrElement, arrSubFields[idx].FieldType.IsArray);

                        //해당 객체내의 필드 표시를 위한 재귀 호출
                        CreateFieldsInfo(subElement);
                    }

                }
                //일반 변수인 경우
                else
                    CreateRow(arrSubFields[idx], arrSubFields[idx].GetValue(arrElement), arrElement, arrSubFields[idx].FieldType.IsArray);
            }
        }
    }

    /// <summary>
    /// type에 해당하는 instance 생성
    /// </summary>
    /// <param name="targetType"></param>
    /// <param name="isArray">요청 타입이 배열=true</param>
    /// <param name="outIsArray">생성 타입이 배열=true</param>
    /// <param name="targetArrLength">배열일 경우 배열 길이 지정</param>
    /// <returns></returns>
    object CreateInstance(Type targetType, bool isArray = false, bool outIsArray = true, int targetArrLength = 1)
    {
        if (isArray && outIsArray)
        {
            return Array.CreateInstance(targetType.GetElementType(), targetArrLength);
        }

        return Activator.CreateInstance(targetType);
    }

    /// <summary>
    /// 필드 이름 및 해당 필드 변수를 변경할 수 있는 UI Input Field를 생성
    /// </summary>
    void CreateRow(FieldInfo fieldInfo, object targetValue = null, object targetData = null, bool isArray = false)
    {
        if (targetData != null && targetValue != null)
            fieldInfo.SetValue(targetData, targetValue);

        GameObject obj = Instantiate(editRowOrigin);
        obj.transform.SetParent(editRowPanel, false);

        Text labelText = obj.transform.GetChild(0).GetComponent<Text>();
        InputField input = obj.transform.GetChild(1).GetComponent<InputField>();

        if (isArray)
            labelText.text = "(" + fieldInfo.FieldType.Name + ")" + fieldInfo.Name + " (array Size)";
        else
            labelText.text = "(" + fieldInfo.FieldType.Name + ")" + fieldInfo.Name;

        input.onEndEdit.RemoveAllListeners();

        if (isArray)
            input.onEndEdit.AddListener((str) => OnEndEditArr(str, fieldInfo, targetData));
        else
            input.onEndEdit.AddListener((str) => OnEndEdit(str, fieldInfo, targetData));

        if (fieldInfo.Name == "dataID")
            input.onEndEdit.AddListener((str) => bindCurDataID.Value = str);

        if (targetValue != null)
            input.text = targetValue.ToString();

        if (isArray)
        {
            Array arr = targetValue as Array;

            input.text = arr != null ? arr.Length.ToString() : "Array Size Here";
        }

        obj.SetActive(true);
    }

    /// <summary>
    /// 필드 이름 및 해당 필드 변수를 변경할 수 있는 UI Input Field를 생성
    /// </summary>
    void CreateRow(Array targetArr, int targetIdx)
    {
        GameObject obj = Instantiate(editRowOrigin);
        obj.transform.SetParent(editRowPanel, false);

        Text labelText = obj.transform.GetChild(0).GetComponent<Text>();
        InputField input = obj.transform.GetChild(1).GetComponent<InputField>();

        labelText.text = targetIdx.ToString();

        input.onEndEdit.RemoveAllListeners();

        input.onEndEdit.AddListener((str) =>
        {
            targetArr.SetValue(StringParseByTypeCode(str, Type.GetTypeCode(targetArr.GetValue(targetIdx).GetType())), targetIdx);
        });

        input.text = targetArr.GetValue(targetIdx).ToString();

        obj.SetActive(true);
    }

    /// <summary>
    /// 일반 변수 타입 값이 수정됐을 경우 데이터 적용
    /// </summary>
    /// <param name="editString"></param>
    /// <param name="fieldInfo"></param>
    /// <param name="targetData"></param>
    public void OnEndEdit(string editString, FieldInfo fieldInfo, object targetData)
    {
        try
        {
            fieldInfo.SetValue(targetData, StringParseByTypeCode(editString, Type.GetTypeCode(fieldInfo.FieldType)));
        }
        catch (Exception e)
        {
            Debug.LogError(editString + " // " + (fieldInfo != null ? fieldInfo.Name : "") + " // " + (targetData != null ? targetData.GetType().Name : ""));
            Debug.LogException(e);
        }
    }

    object StringParseByTypeCode(string str, TypeCode typeCode)
    {
        switch (typeCode)
        {
            case TypeCode.Int16:
            case TypeCode.Int32:
                int n;
                if (int.TryParse(str, out n))
                    return n;
                throw new ArgumentException("Fail Int32 Parse " + str);
            case TypeCode.Int64:
                long l;
                if (long.TryParse(str, out l))
                    return l;
                throw new ArgumentException("Fail Int64 Parse " + str);
            case TypeCode.UInt32:
                uint un;
                if (uint.TryParse(str, out un))
                    return un;
                throw new ArgumentException("Fail UInt32 Parse " + str);
            case TypeCode.UInt64:
                ulong ul;
                if (ulong.TryParse(str, out ul))
                    return ul;
                throw new ArgumentException("Fail UInt64 Parse " + str);
            case TypeCode.Single:
                float f;
                if (float.TryParse(str, out f))
                    return f;
                throw new ArgumentException("Fail Single Parse " + str);
            case TypeCode.Double:
                double d;
                if (double.TryParse(str, out d))
                    return d;
                throw new ArgumentException("Fail Double Parse " + str);
            default:
                throw new ArgumentException("Not support type " + typeCode.ToString());

        }
    }

    /// <summary>
    /// 배열 Size 변경시 배열 확장
    /// 기존 배열 데이터가 삭제됨!!
    /// </summary>
    public void OnEndEditArr(string editString, FieldInfo fieldInfo, object targetData)
    {
        int nLength;
        int.TryParse(editString, out nLength);

        Debug.Log("OnEndEditArr : " + nLength);
        fieldInfo.SetValue(targetData, CreateInstance(fieldInfo.FieldType, true, true, nLength));
        
        CreateFieldsInfo(curData);
    }

}
