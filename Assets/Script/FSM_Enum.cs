using System.Collections;
using System.Collections.Generic;


namespace FiniteStateMachine
{

    public enum FSM_LAYER_ID
    {
        //레이어는 일반 배열을 사용하므로 Enum 변수는 반드시 0부터 순열로 정의되어야 한다. 
        Main = 0,
        UserStory,
        MainUI,
        SubUI,
        PopupUI
    }

    public enum FSM_ID
    {
        NONE = 0,
        Main,
    }

    public enum TRANS_PARAM_ID
    {
        TRIGGER_NONE = 1000,
        TRIGGER_RESET,
        TRIGGER_ESCAPE,
        TRIGGER_BACKBTN,
        TRIGGER_NEXT,
        TRIGGER_SUCCESS,
        TRIGGER_FAIL,
        TRIGGER_OPTION,
        TRIGGER_CHECK_OK,
        TRIGGER_CHECK_FAIL,
        TRIGGER_YES,
        TRIGGER_NO,
        TRIGGER_SKIP,
        TRIGGER_COMPLETE,
        TRIGGER_CHECK_CONDITION,
        TRIGGER_CHECK_ANY_CONDITION,

        INT_NONE = 2000,
        INT_USERSTORY_STATE,
        INT_SELECT_MENU,
        INT_USERSTORY_PRE_STATE,
        INT_FLAG,

        FLOAT_NONE = 3000,

        BOOL_NONE = 4000,
    }

    public enum STATE_ID
    {
        None = 0,
        AnyState,
        HistoryBack,

    }


    public enum TRANS_ID
    {
        NONE,
        TIME,
        HISTORY_BACK,
        RESET,
        ESCAPE_TO_MAIN,
        BACK_TO_MAIN,
        GAMEOVER,
        ESCAPE,
        SKIP,
    }


}