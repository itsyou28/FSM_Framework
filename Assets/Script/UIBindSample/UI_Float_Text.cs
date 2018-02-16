using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Float_Text : UIBindF
{
    [SerializeField]
    Text text;
    [SerializeField]
    string prefix;
    [SerializeField]
    string suffix;

    protected override void OnDataChange()
    {
        base.OnDataChange();

        text.text = bindedData.Value.ToString();
    }
}
