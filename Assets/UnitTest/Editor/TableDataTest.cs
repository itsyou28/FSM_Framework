using UnityEngine;
using System.Collections;
using NUnit.Framework;
using TableData;

public class TableDataTest : MonoBehaviour
{
    [Test]
    public void TestLVData()
    {
        LvTable lvTable = TableAccess.Inst.GetData<LvTable>(0);

        Assert.IsNotNull(lvTable);
        Assert.Greater(lvTable.arrNextLvExp.Length, 0);
    }

    [Test]    
    [Repeat(10)]
    public void AddExp()
    {
        UserLv.Inst.AddExp();
        //Assert.LessOrEqual(BindRepo.Inst.GetBindedData(F_Bind_Idx.User_ExpProgress).Value, 1);
    }
}
