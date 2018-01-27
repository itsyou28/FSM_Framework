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
    int curIdx = -1;

    int x1 = 0;
    int x2 = 0;

    private void Awake()
    {
        UIBinder.Inst.SetCallbackCompleteRegist(() =>
        {
            curIdxBind = UIBinder.Inst.GetBindedData(N_UI_IDX.DynamicLayoutBtnCount);
        });
    }

    public void OnClickAdd()
    {
        if(curIdx == max)
        {
            return;
        }

        curIdx++;
        curIdxBind.Value = curIdx;

        if (curIdx > 1)
            rowPanel[1].SetActive(true);

        if(btnList.Count == curIdx)
        {
            GameObject obj = Instantiate(Resources.Load("UIPrefab/DynamicBtnOrigin") as GameObject);
            obj.GetComponentInChildren<Text>().text = "Button " + curIdx.ToString();

            if(x1 < splitMin || x1 == x2)
            {
                x1++;
                obj.transform.SetParent(rowPanel[0].transform, false);
            }
            else
            {
                x2++;
                obj.transform.SetParent(rowPanel[1].transform, false);
            }

            //Debug.Log(x1.ToString() + " // " + x2.ToString());
            obj.SetActive(true);
            btnList.Add(obj);
        }
        else
        {
            btnList[curIdx].SetActive(true);
        }
        
    }

    public void OnClickRemove()
    {
        if(curIdx == -1)
        {
            return;
        }

        btnList[curIdx].SetActive(false);

        curIdx--;
        curIdxBind.Value = curIdx;

        if (curIdx <= 1)
            rowPanel[1].SetActive(false);

        if (x1 >= splitMin && x1 == x2)
            x2--;
        else
            x1--;
    }
}
