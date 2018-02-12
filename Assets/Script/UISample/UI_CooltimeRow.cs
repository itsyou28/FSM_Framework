using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_CooltimeRow : MonoBehaviour
{
    [SerializeField]
    Image img;
    [SerializeField]
    Slider slider;
    [SerializeField]
    Text elapseTime;
    [SerializeField]
    Text remainTime;

    Bindable<float> curTime;
    Bindable<float> maxTime;
    
    public void Bind(Bindable<float> curTime, Bindable<float> maxTime)
    {
        this.curTime = curTime;
        this.maxTime = maxTime;

        curTime.valueChanged += OnUpdateCurTime;
        OnUpdateCurTime();
    }
    
    void OnUpdateCurTime()
    {
        if (curTime.Value < 0 || curTime.Value > maxTime.Value)
            UDL.LogWarning("Range Over " + gameObject.name);
        else
        {
            img.fillAmount = curTime.Value / maxTime.Value;
            slider.value = img.fillAmount;
            elapseTime.text = (Mathf.Floor(curTime.Value * 100) * 0.01f).ToString();
            remainTime.text = (Mathf.Floor((maxTime.Value - curTime.Value) * 100) * 0.01f).ToString();
        }
    }
}
