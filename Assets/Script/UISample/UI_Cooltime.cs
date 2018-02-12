using UnityEngine;
using System.Collections;


public class UI_Cooltime : MonoBehaviour
{
    Bindable<float> row1Cooltime;
    Bindable<float> row1MaxTime;
    Bindable<string> row1CoolState;

    CooltimeState curState = CooltimeState.Ready;

    private void Awake()
    {
        UIBinder.Inst.SetCallbackCompleteRegist(() =>
        {
            ChangeCoolState(CooltimeState.Ready);
        });
    }

    public void OnClick(int idx)
    {

    }

    void ChangeCoolState(CooltimeState targetState)
    {
        switch(targetState)
        {
            case CooltimeState.Ready:
                row1Cooltime.Value = row1MaxTime.Value;
                break;
        }

        row1CoolState.Value = targetState.ToString();
    }
}
