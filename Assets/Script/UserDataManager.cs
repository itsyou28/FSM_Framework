using UnityEngine;
using System.Collections;

public class UserDataManager
{
    private static UserDataManager instance = null;
    public static UserDataManager Inst
    {
        get
        {
            if (instance == null)
                instance = new UserDataManager();
            return instance;
        }
    }

    iUserData[] arrUserData;

    UserLv userLv = new UserLv();
    UserWealth userWealth = new UserWealth();
    
    private UserDataManager()
    {
    }

    public void Initialize()
    {
        arrUserData = new iUserData[2];
        arrUserData[0] = userLv;
        arrUserData[1] = userWealth;
    }

    public void Save()
    {
        for (int i = 0; i < arrUserData.Length; i++)
        {
            arrUserData[i].Save();
        }
    }

    public void AddExp(int exp=1)
    {
        userLv.AddExp(exp);
    }

    public void EarnGold(int gold)
    {
        userWealth.EarnGold(gold);
    }

    public void UseGold(int gold)
    {
        userWealth.UseGold(gold);
    }
}
