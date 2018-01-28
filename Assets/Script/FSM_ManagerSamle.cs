using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class FSM_ManagerSamle : MonoBehaviour
{
    FSM fsmBtn;
    FSM fsmScroll;

    private void Awake()
    {
        RegistFSM(FSM_LAYER_ID.MainUI, FSM_ID.UIMain);
        fsmBtn  = RegistFSM(FSM_LAYER_ID.UserStory, FSM_ID.USBtn);
        fsmScroll = RegistFSM(FSM_LAYER_ID.UserStory, FSM_ID.USScroll);
        RegistFSM(FSM_LAYER_ID.UserStory, FSM_ID.USProgress);
        //RegistFSM(FSM_LAYER_ID.PopupUI, FSM_ID.PopupUI);

        RegistFSM(FSM_LAYER_ID.UserStory, FSM_ID.USMain);

    }

    Bindable<string> curUSState;

    IEnumerator Start()
    {
        UIBinder.Inst.SetCallbackCompleteRegist(OnCompleteRegist);


        fsmBtn.EventResume += OnResumeUS_FSM;
        State tstate = fsmBtn.GetState(STATE_ID.USBtn_End);
        tstate.EventStart += OnStart_US_EndState;
        tstate.EventResume += OnResume_US_EndState;


        fsmScroll.EventResume += OnResumeUS_FSM;
        tstate = fsmScroll.GetState(STATE_ID.USScroll_End);
        tstate.EventStart += OnStart_US_EndState;
        tstate.EventResume += OnResume_US_EndState;

        yield return true;
        
        //AnyState->USLoading
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_RESET);   
    }

    private void OnResumeUS_FSM(STATE_ID stateID)
    {
        if (stateID == STATE_ID.AnyState)
            FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_RESET);
    }
    
    void OnCompleteRegist()
    {
        //UI 로딩 및 Bind를 사용하는 객체들의 등록이 끝났다면 바인딩을 시작한다. 
        curUSState = UIBinder.Inst.GetBindedData(S_UI_IDX.Userstory_State);

        FSM_Layer.Inst.RegisterEventChangeLayerState(FSM_LAYER_ID.UserStory, OnChangeUserStory);
        FSM_Layer.Inst.RegisterEventChangeLayerState(FSM_LAYER_ID.MainUI, OnChangeMainUI);
        FSM_Layer.Inst.RegisterEventChangeLayerState(FSM_LAYER_ID.PopupUI, OnChangePopupUI);
        
        //USLoading -> USTouchWait
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_NEXT);
    }

    FSM RegistFSM(FSM_LAYER_ID layer, FSM_ID id)
    {
        FSM tFSM = FileManager.Inst.ResourceLoad("FSMData/" + id.ToString()) as FSM;

        if(tFSM == null)
        {
            Debug.LogWarning("No FSM Data " + id.ToString());
            return null;
        }

        tFSM.InitNonSerializedField();

        FSM_Layer.Inst.AddFSM(layer, tFSM, id);

        return tFSM;
    }

    void OnResume_US_EndState(STATE_ID stateId)
    {
        //[x]_End -> HistoryBack
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_NEXT);
    }
    void OnStart_US_EndState(TRANS_ID transId, STATE_ID stateId, STATE_ID preStateId)
    {
        FSM_Layer.Inst.ChangeFSM(FSM_LAYER_ID.UserStory, FSM_ID.USMain);
        //[curState]->USMain_OutroToMainMenu
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_ESCAPE);
    }

    void OnChangeUserStory(TRANS_ID transId, STATE_ID stateId, STATE_ID preStateId)
    {
        UDL.Log("UserStory current State : " + stateId);
        curUSState.Value = stateId.ToString();

        FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.MainUI, TRANS_PARAM_ID.INT_USERSTORY_STATE, (int)stateId);
        FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.MainUI, TRANS_PARAM_ID.INT_USERSTORY_PRE_STATE, (int)preStateId);
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.MainUI, TRANS_PARAM_ID.TRIGGER_CHECK_ANY_CONDITION);

        switch(stateId)
        {
            case STATE_ID.USMain_BtnSample:
                FSM_Layer.Inst.ChangeFSM(FSM_LAYER_ID.UserStory, FSM_ID.USBtn);
                break;
            case STATE_ID.USMain_ScrollSample:
                FSM_Layer.Inst.ChangeFSM(FSM_LAYER_ID.UserStory, FSM_ID.USScroll);
                break;
            default:
                break;
        }
    }


    void OnChangeMainUI(TRANS_ID transId, STATE_ID stateId, STATE_ID preStateId)
    {
        UDL.Log("MainUI current State : " + stateId);

        switch (stateId)
        {
            default:
                break;
        }
    }

    void OnChangePopupUI(TRANS_ID transId, STATE_ID stateId, STATE_ID preStateId)
    {
        UDL.Log("PopupUI current State : " + stateId);

        switch (stateId)
        {
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_ESCAPE);

        TestInputKey();

        FSM_Layer.Inst.Update();
    }

    private static void TestInputKey()
    {
        if (Input.GetKeyDown(KeyCode.N))
            FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_NEXT);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.INT_SELECT_MENU, 1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.INT_SELECT_MENU, 2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.INT_SELECT_MENU, 3);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.INT_SELECT_MENU, 4);
    }
}