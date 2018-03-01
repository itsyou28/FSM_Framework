using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class UI_BtnInput : MonoBehaviour
{
    public void TriggerNext()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    public void IntSelectMainMenu(int nValue)
    {
        FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.INT_SELECT_MENU, nValue);
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_CHECK_ANY_CONDITION);
    }
    
    public void OnClickBackBtn()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_BACKBTN);
    }

    public void TriggerLeft()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_LEFT);
    }

    public void TriggerRight()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_RIGHT);
    }

    public void TriggerUp()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_UP);
    }

    public void TriggerDown()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_DOWN);
    }

    public void TriggerBackBtn()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_BACKBTN);
    }

    public void AddExp(int exp)
    {
        UserDataManager.Inst.AddExp(exp);
    }
}
