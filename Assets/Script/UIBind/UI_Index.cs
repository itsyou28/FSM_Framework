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
}

public enum F_UI_IDX
{
    NONE = 0,
    Top_BindingSample,
    Top_ProgressBarSample,
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
}
