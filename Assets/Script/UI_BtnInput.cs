﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class UI_BtnInput : MonoBehaviour
{
    public void TriggerNext()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_NEXT);
    }
}