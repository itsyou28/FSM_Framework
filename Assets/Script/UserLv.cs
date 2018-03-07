using UnityEngine;
using System.Collections;
using TableData;

public class UserLv : iUserData
{
    struct StructForFile
    {
        public int userLv;
        public int userExp;
    }

    Bindable<int> userLV, userEXP;

    Bindable<float> userEXP_Progress;
    Bindable<string> userExpText;

    int curLvExpStart=0, curLvExpEnd=1;

    LvTable lvTable;

    public UserLv()
    {
        SetBind();
        Load();
        UpdateLvTable();
    }

    public bool Load()
    {
        userLV.Value = PlayerPrefs.GetInt("userLV");
        userEXP.Value = PlayerPrefs.GetInt("userEXP");

        return true;
    }

    public bool Save()
    {
        PlayerPrefs.SetInt("userLV", userLV.Value);
        PlayerPrefs.SetInt("userEXP", userEXP.Value);

        return true;
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

        OnChangeUserExp();
    }

    private void OnChangeUserExp()
    {
        userExpText.Value = userEXP.Value.ToString() + " / " + curLvExpEnd.ToString();
        userEXP_Progress.Value = BK_Function.ConvertRangePercent(curLvExpStart, curLvExpEnd, userEXP.Value);
    }

    public void AddExp(int exp)
    {
        if (curLvExpEnd == 0)
            return;

        userEXP.Value += exp;

        if (userEXP.Value >= curLvExpEnd)
            LevelUP();
    }

    void LevelUP()
    {
        userLV.Value += 1;
        EMC_MAIN.Inst.NoticeEventOccurrence(EMC_CODE.DISP_MSG, "Level UP!!");
        UpdateLvTable();
    }
}
