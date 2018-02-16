using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_F_Setter_InputField : UIBindF
{
    [SerializeField]
    InputField control;

    protected override void OnDataChange()
    {
        base.OnDataChange();

        if (!Mathf.Approximately(float.Parse(control.text), bindedData.Value))
            control.text = bindedData.Value.ToString();
    }

    public void OnValueChange(string _value)
    {
        bindedData.Value = float.Parse(_value);
    }
}
