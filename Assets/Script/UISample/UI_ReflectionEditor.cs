using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class UI_ReflectionEditor : MonoBehaviour
{
    [SerializeField]
    Transform controlPanel;
    [SerializeField]
    GameObject classBtnOrigin;

    [SerializeField]
    Transform dataRowPanel;
    [SerializeField]
    GameObject dataRowBtnOrigin;

    Type curClass;

    //데이터 사용시에는 검색만 사용하게 됨을 가정하여 최종 데이터는 Dictionary 형태로 export
    Dictionary<int, object> exportData;

    //삽입, 삭제가 빈번. 신규생성 데이터를 제일 앞에 삽입하므로 LinkedList 사용
    LinkedList<object> dataList;

    //데이터 선택 버튼을 위한 pool
    ObjectPool<GameObject> rowPool;

    private void Awake()
    {
        rowPool = new ObjectPool<GameObject>(1, 3, CreateRow);
        Debug.Log("Awake " + rowPool.Count);
    }

    GameObject CreateRow()
    {
        GameObject newRow = Instantiate(dataRowBtnOrigin);

        newRow.transform.SetParent(dataRowPanel, false);
        newRow.SetActive(false);

        return newRow;
    }

    void Start()
    {
        MakeSelectClassBtn();
    }

    void MakeSelectClassBtn()
    {
        var theList = Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => t.Namespace == "Data")
                      .ToList();

        int idx = 0;
        foreach (var t in theList)
        {
            GameObject obj = Instantiate(classBtnOrigin);
            obj.GetComponentInChildren<Text>().text = t.Name;

            string className = t.Name;
            Type typeClone = t;

            Toggle toggle = obj.GetComponentInChildren<Toggle>();
            toggle.onValueChanged.AddListener((value) =>
            {
                if(value)
                {
                    Debug.Log(className);
                    curClass = typeClone;
                    MakeDataRowBtn();
                }
            });

            if (idx == 0)
                toggle.isOn = true;
            else
                toggle.isOn = false;
            idx++;

            obj.transform.SetParent(controlPanel, false);
            obj.SetActive(true);
        }
    }

    void MakeDataRowBtn()
    {
        if (FileManager.Inst.CheckFileExists(curClass.Name))
        {
            dataList = FileManager.Inst.FileLoad("", curClass.Name) as LinkedList<object>;
        }
        else
        {
            dataList = new LinkedList<object>();

            object data = Activator.CreateInstance(curClass);

            dataList.AddLast(data);

            FileManager.Inst.FileSave("", curClass.Name, dataList);
        }

        PushBack();

        foreach(DataBase t in dataList)
        {
            SetRowData(t.dataID, t);
        }
    }

    private void PushBack()
    {
        for (int i = 0; i < dataRowPanel.transform.childCount; i++)
        {
            GameObject obj = dataRowPanel.transform.GetChild(i).gameObject;
            if(obj.activeSelf)
            {
                obj.SetActive(false);
                rowPool.Push(obj);
                Debug.Log("PushBack " + rowPool.Count);
            }
        }

        Debug.Log("PushBack " + rowPool.Count);
    }
    
    GameObject SetRowData(string dataID, object data)
    {
        GameObject go = rowPool.Pop();

        Text text = go.GetComponentInChildren<Text>();

        text.text = dataID;

        Toggle toggle = go.GetComponentInChildren<Toggle>();

        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((value) =>
        {
            if (value)
            {
                SetFieldRow(data, text);
            }
        });

        Button removeBtn = go.GetComponentInChildren<Button>();

        removeBtn.onClick.RemoveAllListeners();
        removeBtn.onClick.AddListener(() =>
        {
            toggle.isOn = false;
            go.gameObject.SetActive(false);
            rowPool.Push(go);
            dataList.Remove(data);
        });

        go.SetActive(true);

        return go;
    }

    public void AddData()
    {
        object newData = Activator.CreateInstance(curClass);
        DataBase t = newData as DataBase;

        dataList.AddFirst(newData);

        SetRowData(t.dataID, newData).transform.SetAsFirstSibling();
    }

    public void SaveData()
    {
    }

    public void SetFieldRow(object data, Text rowDataID)
    {

    }
}
