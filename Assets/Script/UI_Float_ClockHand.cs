using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ClockHandType
{
    Second,
    Minute,
    Hour
}

public class UI_Float_ClockHand : UIBindN
{

    [SerializeField]
    ClockHandType chType;

    Vector3 rotate = Vector3.zero;

    protected override void OnDataChange()
    {
        base.OnDataChange();
        
        rotate.z = bindedData.Value == 0 ? 0 : chType == ClockHandType.Hour ? 
            bindedData.Value / 720.0f * -360 : bindedData.Value / 60.0f * -360;
        transform.localEulerAngles = rotate;
    }
}
