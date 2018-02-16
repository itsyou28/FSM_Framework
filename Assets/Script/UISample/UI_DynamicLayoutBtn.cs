using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DynamicLayoutBtn : MonoBehaviour
{
    [SerializeField]
    GameObject[] rowPanel;
    [SerializeField]
    GameObject guideText;

    List<GameObject> btnList = new List<GameObject>();

    const int max = 9;
    const int splitMin = 2;

    Bindable<int> curIdxBind;
    int curIdx = -1;
    
    private void Awake()
    {
        curIdxBind = BindRepo.Inst.GetBindedData(N_Bind_Idx.DynamicLayoutBtnCount);
        curIdxBind.Value = 0;
    }

    public void OnClickAdd()
    {
        if(curIdx == max)
        {
            return;
        }

        curIdx++;
        curIdxBind.Value = curIdx + 1;

        if (curIdx > -1 && guideText.activeInHierarchy)
            guideText.SetActive(false);

        if (curIdx >= splitMin && !rowPanel[1].activeInHierarchy)
            rowPanel[1].SetActive(true);

        if(btnList.Count == curIdx)
        {
            GameObject obj = Instantiate(Resources.Load("UIPrefab/DynamicBtnOrigin") as GameObject);
            obj.GetComponentInChildren<Text>().text = "Button " + (curIdx+1).ToString();
            btnList.Add(obj);
        }

        ArrangeRow();
    }

    public void OnClickRemove()
    {
        if(curIdx == -1)
        {
            return;
        }
        
        curIdx--;
        curIdxBind.Value = curIdx + 1;

        if (curIdx == -1 && !guideText.activeInHierarchy)
            guideText.SetActive(true);

        if (curIdx < splitMin && rowPanel[1].activeInHierarchy)
            rowPanel[1].SetActive(false);

        ArrangeRow();
    }

    void ArrangeRow()
    {
        int half = Mathf.CeilToInt((curIdx+1) * 0.5f);
        
        for (int i = 0; i < btnList.Count; i++)
        {
            if (i > curIdx)
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
