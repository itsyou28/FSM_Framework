using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class UI_Stopwatch : MonoBehaviour
{
    Bindable<string> elapseTimeStr;

    [SerializeField]
    Text leftText;
    [SerializeField]
    Text rightText;
    [SerializeField]
    Text recordText;
    
    float elapseTime = 0;

    bool running = false;

    private void Awake()
    {
        elapseTimeStr = BindRepo.Inst.GetBindedData(S_Bind_Idx.Stopwatch_ElapseTime);
        elapseTimeStr.Value = TimeFormat(0);
    }

    int hh, mm, ss, fff;

    void Update ()
    {
        if(running)
        {
            elapseTime += Time.deltaTime;
            elapseTimeStr.Value = TimeFormat(elapseTime);
        }
    }

    string TimeFormat(float time)
    {
        hh = Mathf.FloorToInt(time / 3600);
        mm = Mathf.FloorToInt((time - hh) / 60);
        ss = Mathf.FloorToInt(time - (hh * 3600) - (mm * 60));
        fff = Mathf.FloorToInt((time - Mathf.FloorToInt(elapseTime)) * 1000);

        return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", hh, mm, ss, fff);
    }
    
    public void OnClickLeft()
    {
        if (running)
        {
            //record
            recordText.text += TimeFormat(elapseTime) + "\n";
        }
        else
        {
            //Start
            ChangeState(true);
        }
    }

    public void OnClickRight()
    {
        if(running)
        {
            //stop
            ChangeState(false);
        }
        else
        {
            //reset
            elapseTime = 0;
            elapseTimeStr.Value = TimeFormat(0);
            recordText.text = "";
        }
    }

    void ChangeState(bool bValue)
    {
        if (bValue)
        {
            leftText.text = "Record";
            rightText.text = "Pause";
        }
        else
        {
            leftText.text = "Start";
            rightText.text = "Reset";
        }

        running = bValue;
    }
}
