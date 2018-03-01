using UnityEngine;
using System.Collections;
using TableData;

public class UserLv : SingleTon<UserLv>
{
    Bindable<int> userLV, userEXP;

    Bindable<float> userEXP_Progress;
    Bindable<string> userExpText;

    int curLvExpStart, curLvExpEnd;

    LvTable lvTable;

    public UserLv()
    {
        SetBind();
        UpdateLvTable();
    }

    void SetBind()
    {
        userLV = BindRepo.Inst.GetBindedData(N_Bind_Idx.UserLevel);
        userEXP = BindRepo.Inst.GetBindedData(N_Bind_Idx.UserExp);
        userEXP_Progress = BindRepo.Inst.GetBindedData(F_Bind_Idx.User_ExpProgress);
        userExpText = BindRepo.Inst.GetBindedData(S_Bind_Idx.UserExp);

        userEXP.valueChanged += OnChangeUserExp;
    }

    private void UpdateLvTable()
    {
        if(lvTable == null)
            lvTable = TableAccess.Inst.GetData<LvTable>(0);

        curLvExpEnd = curLvExpStart = 0;

        if (lvTable != null)
        {
            if (lvTable.arrNextLvExp != null)
            {
                if (userLV.Value != lvTable.arrNextLvExp.Length)
                {
                    curLvExpStart = userLV.Value == 0?0:lvTable.arrNextLvExp[userLV.Value - 1];
                    curLvExpEnd = lvTable.arrNextLvExp[userLV.Value];
                }
            }
        }
        else
            curLvExpEnd = 0;
    }

    private void OnChangeUserExp()
    {
        userExpText.Value = userEXP.Value.ToString() + " / " + curLvExpEnd.ToString();
        userEXP_Progress.Value = BK_Function.ConvertRangePercent(curLvExpStart, curLvExpEnd, userEXP.Value);
    }

    public void AddExp(int earn = 1)
    {
        if (curLvExpEnd == 0)
            return;

        userEXP.Value += earn;

        if (userEXP.Value >= curLvExpEnd)
            LevelUP();
    }

    void LevelUP()
    {
        userLV.Value += 1;
        UpdateLvTable();
        OnChangeUserExp();
    }
}
