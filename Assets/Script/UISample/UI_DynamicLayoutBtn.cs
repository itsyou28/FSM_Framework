using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DynamicLayoutBtn : MonoBehaviour
{
    [SerializeField]
    GameObject[] rowPanel;

    List<GameObject> btnList = new List<GameObject>();

    const int max = 9;
    const int splitMin = 2;

    Bindable<int> curIdxBind;
    
    private void Awake()
    {
        UIBinder.Inst.SetCallbackCompleteRegist(() =>
        {
            curIdxBind = UIBinder.Inst.GetBindedData(N_UI_IDX.DynamicLayoutBtnCount);
            curIdxBind.Value = -1;
        });
    }

    public void OnClickAdd()
    {
        if(curIdxBind.Value == max)
        {
            return;
        }

        curIdxBind.Value++;

        if (curIdxBind.Value >= splitMin && !rowPanel[1].activeInHierarchy)
            rowPanel[1].SetActive(true);

        if(btnList.Count == curIdxBind.Value)
        {
            GameObject obj = Instantiate(Resources.Load("UIPrefab/DynamicBtnOrigin") as GameObject);
            obj.GetComponentInChildren<Text>().text = "Button " + (curIdxBind.Value+1).ToString();
            btnList.Add(obj);
        }

        ArrangeRow();
    }

    public void OnClickRemove()
    {
        if(curIdxBind.Value == -1)
        {
            return;
        }
        
        curIdxBind.Value--;

        if (curIdxBind.Value < splitMin && rowPanel[1].activeInHierarchy)
            rowPanel[1].SetActive(false);

        ArrangeRow();
    }

    void ArrangeRow()
    {
        int half = Mathf.CeilToInt((curIdxBind.Value+1) * 0.5f);

        Debug.Log(half);
        for (int i = 0; i < btnList.Count; i++)
        {
            if (i > curIdxBind.Value)
            {
                btnList[i].SetActive(false);
                continue;
            }
            else
                btnList[i].SetActive(true);

            if (i < half || i < splitMin)
                btnList[i].transform.SetParent(rowPanel[0].transform, false);
            else
            {
                btnList[i].transform.SetParent(rowPanel[1].transform, false);
                btnList[i].transform.SetAsLastSibling();
            }
        }
    }
}
