using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_N_Setter_InputField : UIBindN
{
    [SerializeField]
    InputField control;

    protected override void OnDataChange()
    {
        base.OnDataChange();

        if (int.Parse(control.text) != bindedData.Value)
            control.text = bindedData.Value.ToString();
    }

    public void OnValueChange(string _value)
    {
        bindedData.Value = int.Parse(_value);
    }

}
