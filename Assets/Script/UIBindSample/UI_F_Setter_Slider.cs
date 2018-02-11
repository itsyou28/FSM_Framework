using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_F_Setter_Slider : UIBindF
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
