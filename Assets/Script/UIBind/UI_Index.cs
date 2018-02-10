using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기존 사용중인 enum을 변경할 경우 
/// 인스펙터에서 직렬화 되어 있는 데이터의 경우 반영이 안되기 때문에 주의해야 함
/// </summary>

public enum N_UI_IDX
{
    NONE=0,
    Top_BindingSample,

    DynamicLayoutBtnCount = 100,

    DynamicListScrollCount = 200,

    UTC_Now_HourHand = 300,
    UTC_Now_MinuteHand,
    UTC_Now_SecondHand,

    UTC_Local_Now_HourHand,
    UTC_Local_Now_MinuteHand,
    UTC_Local_Now_SecondHand,
}

public enum F_UI_IDX
{
    NONE = 0,
    Top_BindingSample,
    Top_ProgressBarSample,

    Set_Local_UTC,
}

public enum L_UI_IDX
{
    NONE = 0,
}

public enum B_UI_IDX
{
    NONE = 0,
}

public enum D_UI_IDX
{
    NONE = 0,
}

public enum S_UI_IDX
{
    NONE = 0,
    Userstory_State,

    ToggleMode = 100,

    UTC_NowDate=300,
    UTC_NowTime,
    UTC_Local_NowDate,
    UTC_Local_NowTime,
}
