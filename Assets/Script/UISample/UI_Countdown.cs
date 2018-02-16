using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Countdown : MonoBehaviour
{
    [SerializeField]
    UI_TextEffect effectText;
    [SerializeField]
    UI_TextEffect curCountText;

    Bindable<int> maxCountdown;

    private void Awake()
    {
        maxCountdown = BindRepo.Inst.GetBindedData(N_Bind_Idx.Set_Countdown);
        maxCountdown.Value = 3;
    }

    public void OnClickStart()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        int max = maxCountdown.Value;
        float effectShowTime = 0.7f;

        float elapseTime = 0;

        int curCount = 0;
        int remainCount = max;
        float effectCount = effectShowTime;

        while(elapseTime <= max)
        {
            elapseTime += Time.deltaTime;

            if(elapseTime - curCount >= 0)
            {
                remainCount = max - curCount;
                curCountText.StartCoroutine(curCountText.FlashShow(remainCount));
                curCount++;
            }

            if(elapseTime - effectCount >= 0)
            {
                effectText.StartCoroutine(effectText.ScaleAlphaShow(remainCount - 1));
                effectCount += 1;
            }

            yield return true;
        }
    }

}
