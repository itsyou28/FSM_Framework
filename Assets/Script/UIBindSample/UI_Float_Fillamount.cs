using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Float_Fillamount : UIBindF
{
    [SerializeField]
    Image img;

    protected override void OnDataChange()
    {
        base.OnDataChange();

        if (bindedData.Value < 0 || bindedData.Value > 1)
            UDL.LogWarning("fillamount range over");
        else
            img.fillAmount = bindedData.Value;
    }
}
