using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Int_Text : UIBindN
{
    [SerializeField]
    Text text;

    protected override void OnDataChange()
    {
        base.OnDataChange();

        text.text = bindedData.Value.ToString();
    }
}
