using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Float_Fillamount : UIBindF
{
    [SerializeField]
    Image img;
    [SerializeField]
    bool reverse = false;

    protected override void OnDataChange()
    {
        base.OnDataChange();

        if (bindedData.Value < 0)
        {
            UDL.LogWarning("fillamount range over");
            img.fillAmount = 0;
        }
            else if(bindedData.Value > 1)
        {
            UDL.LogWarning("fillamount range over");
            img.fillAmount = 1;
        }

        if (reverse)
            img.fillAmount = 1 - bindedData.Value;
        else
            img.fillAmount = bindedData.Value;
    }
}
