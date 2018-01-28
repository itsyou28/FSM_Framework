using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DynamicListRowData
{
    public string text1;
    public string text2;
    public System.Action callbackDestroy;
}

public class UI_DynamicListScroll : MonoBehaviour
{
    [SerializeField]
    RectTransform contentPanel;

    [SerializeField]
    GameObject guideText;

    Bindable<int> listCount;
    
    GameObject rowOrigin;

    int newRowID = 0;

    private void Awake()
    {
        rowOrigin = Resources.Load("UIPrefab/DynamicListRowOrigin") as GameObject;

        UIBinder.Inst.SetCallbackCompleteRegist(() =>
        {
            listCount = UIBinder.Inst.GetBindedData(N_UI_IDX.DynamicListScrollCount);
        });
    }

    private void OnEnable()
    {
        if (listCount != null)
            UpdateCountDisplay();
    }

    public void OnClickAdd()
    {
        GameObject row = Instantiate(rowOrigin);
        row.transform.SetParent(contentPanel.transform, false);

        newRowID++;

        DynamicListRowData newData = new DynamicListRowData();
        newData.text1 = "rowText" + newRowID.ToString();
        newData.text2 = newData.text1.GetHashCode().ToString();
        newData.callbackDestroy = OnRemoveRow;

        row.SendMessage("SetData", newData);

        row.GetComponentsInChildren<Button>()[1].onClick.AddListener(() =>
        {
            listCount.Value = contentPanel.transform.childCount;
        });

        UpdateCountDisplay();
    }

    public void OnClickRemove()
    {
        if (contentPanel.transform.childCount == 0)
            return;

        Destroy(contentPanel.transform.GetChild(contentPanel.transform.childCount - 1).gameObject);
    }

    private void OnRemoveRow()
    {
        if(gameObject.activeInHierarchy)
            StartCoroutine(WaitDestroyRow());
    }

    IEnumerator WaitDestroyRow()
    {
        yield return true;
        UpdateCountDisplay();
    }

    private void UpdateCountDisplay()
    {
        listCount.Value = contentPanel.transform.childCount;

        if (listCount.Value > 0 && guideText.activeInHierarchy)
            guideText.SetActive(false);
        else if (listCount.Value <= 0 && !guideText.activeInHierarchy)
            guideText.SetActive(true);
    }
}
