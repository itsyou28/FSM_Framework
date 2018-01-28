using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DynamicListScrollRow : MonoBehaviour
{
    [SerializeField]
    Text[] arrText;

    [SerializeField]
    Image icon;

    DynamicListRowData data;

    public void SetData(DynamicListRowData data)
    {
        this.data = data;

        arrText[0].text = data.text1;
        arrText[1].text = data.text2;
    }

    public void OnClickRow()
    {
        Debug.Log(data.text1);        
    }

    public void OnClickRemove()
    {
        Destroy(this.gameObject);
    }

    public void OnDestroy()
    {
        data.callbackDestroy();   
    }
}
