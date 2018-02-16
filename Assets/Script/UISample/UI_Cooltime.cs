using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UI_Cooltime : MonoBehaviour
{
    [SerializeField]
    Button coolBtn;
    [SerializeField]
    GameObject coolBtn_Effect;
    [SerializeField]
    GameObject useBtn;

    Bindable<float> cooltime;
    Bindable<float> fRemainTime;
    Bindable<float> maxTime;
    Bindable<float> fPercent;
    Bindable<int> shotCost;
    Bindable<int> nPercent;
    Bindable<int> nRemainTime;
    Bindable<int> mode;
    Bindable<string> coolState;

    CooltimeState curState = CooltimeState.Ready;

    float cost;

    private void Awake()
    {
        cooltime = BindRepo.Inst.GetBindedData(F_Bind_Idx.Cooltime_CurTime);
        maxTime = BindRepo.Inst.GetBindedData(F_Bind_Idx.Cooltime_MaxTime);
        fRemainTime = BindRepo.Inst.GetBindedData(F_Bind_Idx.Cooltime_RemainTime);
        fPercent = BindRepo.Inst.GetBindedData(F_Bind_Idx.Cooltime_Percent);
        shotCost = BindRepo.Inst.GetBindedData(N_Bind_Idx.Cooltime_ShotCost);
        nPercent = BindRepo.Inst.GetBindedData(N_Bind_Idx.Cooltime_Percent);
        nRemainTime = BindRepo.Inst.GetBindedData(N_Bind_Idx.Cooltime_RemainTime);
        coolState = BindRepo.Inst.GetBindedData(S_Bind_Idx.CooltimeState);
        mode = BindRepo.Inst.GetBindedData(N_Bind_Idx.Cooltime_Mode);

        cooltime.valueChanged += OnChangeCooltime;
        maxTime.valueChanged += OnChangeCost;
        shotCost.valueChanged += OnChangeCost;
    }

    private void OnChangeCost()
    {
        cost = maxTime.Value * (shotCost.Value * 0.01f);
    }

    private void OnChangeCooltime()
    {
        fPercent.Value = cooltime.Value == 0 ? 0 : cooltime.Value / maxTime.Value;
        nPercent.Value = Mathf.FloorToInt(fPercent.Value * 100);
        fRemainTime.Value = maxTime.Value - cooltime.Value;
        nRemainTime.Value = Mathf.CeilToInt(fRemainTime.Value);
        
        if (fPercent.Value >= 1)
            coolBtn_Effect.SetActive(true);
        else if (mode.Value == (int)CooltimeMode.MultiShot && cooltime.Value >= cost)
            coolBtn_Effect.SetActive(true);
        else
            coolBtn_Effect.SetActive(false);
    }

    private void Start()
    {
        mode.Value = (int)CooltimeMode.OneShot;
        shotCost.Value = 10;
        maxTime.Value = 5;
        cooltime.Value = maxTime.Value;

        coolState.Value = CooltimeState.Ready.ToString();
    }

    public void OnClick()
    {
        switch(curState)
        {
            case CooltimeState.Ready:
                if (mode.Value == (int)CooltimeMode.OneShot)
                {
                    cooltime.Value = 0;
                    ChangeCoolState(CooltimeState.Cooling);
                }
                else
                    ChangeCoolState(CooltimeState.MultiActive);
                break;
            case CooltimeState.Cooling:
                if (mode.Value == (int)CooltimeMode.MultiShot && cooltime.Value >= cost)
                    ChangeCoolState(CooltimeState.MultiActive);
                break;
            case CooltimeState.MultiActive:
                if (fPercent.Value < 1.0f)
                    ChangeCoolState(CooltimeState.Cooling);
                else
                    ChangeCoolState(CooltimeState.Ready);
                break;
        }
    }

    private void Update()
    {
        if(curState == CooltimeState.Cooling)
        {
            if (cooltime.Value + Time.deltaTime >= maxTime.Value)
                ChangeCoolState(CooltimeState.Ready);
            else
                cooltime.Value += Time.deltaTime;
        }
    }

    void ChangeCoolState(CooltimeState targetState)
    {
        switch(targetState)
        {
            case CooltimeState.Ready:
                if (!coolBtn.interactable)
                    coolBtn.interactable = true;
                useBtn.SetActive(false);
                cooltime.Value = maxTime.Value;
                break;
            case CooltimeState.Cooling:
                if (mode.Value == (int)CooltimeMode.OneShot)
                    coolBtn.interactable = false;
                else
                {
                    if (!coolBtn.interactable)
                        coolBtn.interactable = true;
                }
                useBtn.SetActive(false);
                break;
            case CooltimeState.MultiActive:
                if (!coolBtn.interactable)
                    coolBtn.interactable = true;
                useBtn.SetActive(true);
                break;
        }

        curState = targetState;
        coolState.Value = targetState.ToString();
    }

    public void OnClickUseBtn()
    {
        cooltime.Value -= cost;

        if (cooltime.Value < cost)
            ChangeCoolState(CooltimeState.Cooling);
    }

    public void OnChangeMode(int mode)
    {
        this.mode.Value = mode;

        if (mode == (int)CooltimeMode.OneShot && fPercent.Value < 1)
            ChangeCoolState(CooltimeState.Cooling);
    }
}
