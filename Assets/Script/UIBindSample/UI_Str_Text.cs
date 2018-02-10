using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Str_Text : UIBindS
{
    [SerializeField]
    Text text;
    
    protected override void OnDataChange()
    {
        base.OnDataChange();

        text.text = bindedData.Value;
    }
}
