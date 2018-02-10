using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_Float_Setter : UIBindF
{
    [SerializeField]
    Slider slider;

    protected override void OnDataChange()
    {
        base.OnDataChange();
        
        if(!Mathf.Approximately(slider.value, bindedData.Value))
            slider.value = bindedData.Value;
    }

    public void OnValueChange(float fValue)
    {
        bindedData.Value = fValue;
    }
}
