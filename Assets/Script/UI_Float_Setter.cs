using UnityEngine;
using System.Collections;

public class UI_Float_Setter : UIBindF
{
    public void OnValueChange(float fValue)
    {
        bindedData.Value = fValue;
    }
}
