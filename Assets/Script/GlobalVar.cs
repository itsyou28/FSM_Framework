using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlovalVar
{
}

public enum LogOption
{
    DEFAULT = 0,
    FSM = 1,
    FSM_Layer = 2,
    FSM_Reaction = 3,
    UI_Binder = 4,
}

public enum CooltimeState
{
    Ready,
    Cooling,
    Using,
}