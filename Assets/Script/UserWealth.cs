using UnityEngine;
using System.Collections;

public class UserWealth : iUserData
{
    Bindable<int> userGold;

    public UserWealth()
    {
        userGold = BindRepo.Inst.GetBindedData(N_Bind_Idx.UserGold);
        Load();
    }

    public void EarnGold(int earn)
    {
        userGold.Value += earn;
    }

    public void UseGold(int earn)
    {
        userGold.Value -= earn;
    }

    public bool Load()
    {
        userGold.Value = PlayerPrefs.GetInt("userGold");
        return true;
    }

    public bool Save()
    {
        PlayerPrefs.SetInt("userGold", userGold.Value);
        return true;
    }
}
