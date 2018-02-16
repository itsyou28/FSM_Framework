using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UI_UtcTimeSample : MonoBehaviour
{
    public Text curLocalUTC;

    Bindable<int> hourHand, minuteHand, secondHand;
    Bindable<int> day = new Bindable<int>();

    Bindable<int> local_hourHand, local_minuteHand, local_secondHand;
    Bindable<int> local_day = new Bindable<int>();

    Bindable<string> utcNowDate, utcNowTime, localNowDate, localNowTime;

    Bindable<float> curSetLocalUtc;

    void Start()
    {
        hourHand = BindRepo.Inst.GetBindedData(N_Bind_Idx.UTC_Now_HourHand);
        minuteHand = BindRepo.Inst.GetBindedData(N_Bind_Idx.UTC_Now_MinuteHand);
        secondHand = BindRepo.Inst.GetBindedData(N_Bind_Idx.UTC_Now_SecondHand);
        utcNowDate = BindRepo.Inst.GetBindedData(S_Bind_Idx.UTC_NowDate);
        utcNowTime = BindRepo.Inst.GetBindedData(S_Bind_Idx.UTC_NowTime);

        local_hourHand = BindRepo.Inst.GetBindedData(N_Bind_Idx.UTC_Local_Now_HourHand);
        local_minuteHand = BindRepo.Inst.GetBindedData(N_Bind_Idx.UTC_Local_Now_MinuteHand);
        local_secondHand = BindRepo.Inst.GetBindedData(N_Bind_Idx.UTC_Local_Now_SecondHand);
        localNowDate = BindRepo.Inst.GetBindedData(S_Bind_Idx.UTC_Local_NowDate);
        localNowTime = BindRepo.Inst.GetBindedData(S_Bind_Idx.UTC_Local_NowTime);

        curSetLocalUtc = BindRepo.Inst.GetBindedData(F_Bind_Idx.Set_Local_UTC);
        curSetLocalUtc.valueChanged += OnChangeGMTControl;

        day.valueChanged += DisplayNowDate;
        local_day.valueChanged += DisplayLocalNowDate;
        InvokeRepeating("DisplayNowTime", 0, 1);

        curSetLocalUtc.Value = 9;
    }

    void OnEnable()
    {
        InvokeRepeating("DisplayNowTime", 0, 1);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    DateTime utcNow;
    DateTime localNow;

    void DisplayNowDate()
    {
        utcNowDate.Value = utcNow.ToString("yyyy-MM-dd-ddd");
    }

    void DisplayLocalNowDate()
    {
        localNowDate.Value = localNow.ToString("yyyy-MM-dd-ddd");
    }

    void DisplayNowTime()
    {
        DisplayUtcTime();
        DisplayLocalTime();
    }

    private void DisplayUtcTime()
    {
        utcNow = DateTime.UtcNow;
        utcNowTime.Value = utcNow.ToString("hh:mm:ss");

        secondHand.Value = utcNow.Second;

        if (minuteHand.Value != utcNow.Minute)
        {
            minuteHand.Value = utcNow.Minute;
            hourHand.Value = utcNow.Hour * 60 + utcNow.Minute;
        }

        if (day.Value != utcNow.Day)
        {
            day.Value = utcNow.Day;
        }
    }

    private void DisplayLocalTime()
    {
        localNow = DateTime.UtcNow.AddHours(curSetLocalUtc.Value);

        localNowTime.Value = localNow.ToString("hh:mm:ss");
        local_secondHand.Value = localNow.Second;

        if (local_minuteHand.Value != localNow.Minute)
        {
            local_minuteHand.Value = localNow.Minute;
        }

        int hourConvert = localNow.Hour * 60 + localNow.Minute;
        if (local_hourHand.Value != hourConvert)
            local_hourHand.Value = hourConvert;

        if (local_day.Value != localNow.Day)
        {
            local_day.Value = localNow.Day;
        }
    }

    public void OnChangeGMTControl()
    {
        curLocalUTC.text = curSetLocalUtc.Value < 0 ? curSetLocalUtc.Value.ToString() :
            curSetLocalUtc.Value == 0 ? "0" : "+" + curSetLocalUtc.Value.ToString();

        DisplayLocalTime();
    }
}
