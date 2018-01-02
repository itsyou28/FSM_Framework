using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FiniteStateMachine;

public class FSM_ManagerSamle : MonoBehaviour
{

    private void Awake()
    {

        FSM_Layer.Inst.RegisterEventChangeLayerState(FSM_LAYER_ID.UserStory, OnChangeUserStory);
        FSM_Layer.Inst.RegisterEventChangeLayerState(FSM_LAYER_ID.MainUI, OnChangeMainUI);
        FSM_Layer.Inst.RegisterEventChangeLayerState(FSM_LAYER_ID.PopupUI, OnChangePopupUI);

        RegistFSM(FSM_LAYER_ID.UserStory, FSM_ID.USMain);
        RegistFSM(FSM_LAYER_ID.MainUI, FSM_ID.UIMain);
        //RegistFSM(FSM_LAYER_ID.PopupUI, FSM_ID.PopupUI);

    }
    private void Start()
    {
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.UserStory, TRANS_PARAM_ID.TRIGGER_RESET);
        
    }
    void RegistFSM(FSM_LAYER_ID layer, FSM_ID id)
    {
        FSM tFSM = FileManager.Inst.ResourceLoad("FSMData/" + id.ToString()) as FSM;

        if(tFSM == null)
        {
            Debug.LogWarning("No FSM Data " + id.ToString());
            return;
        }

        tFSM.InitNonSerializedField();

        FSM_Layer.Inst.AddFSM(layer, tFSM, id);
    }

    void OnChangeUserStory(TRANS_ID transId, STATE_ID stateId, STATE_ID preStateId)
    {
        UDL.Log("UserStory current State : " + stateId);
        FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.MainUI, TRANS_PARAM_ID.INT_USERSTORY_STATE, (int)stateId);
        FSM_Layer.Inst.SetInt_NoCondChk(FSM_LAYER_ID.MainUI, TRANS_PARAM_ID.INT_USERSTORY_PRE_STATE, (int)preStateId);
        FSM_Layer.Inst.SetTrigger(FSM_LAYER_ID.MainUI, TRANS_PARAM_ID.TRIGGER_CHECK_ANY_CONDITION);

        switch(stateId)
        {
            case STATE_ID.USMain_Loading:
                //ui prefab loading;
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
        TestInputKey();
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