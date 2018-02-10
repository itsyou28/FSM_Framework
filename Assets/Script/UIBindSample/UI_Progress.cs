using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Progress : UIBindF
{
    [SerializeField]
    Slider slider;

    protected override void OnDataChange()
    {
        base.OnDataChange();

        slider.value = bindedData.Value;
    }
}
