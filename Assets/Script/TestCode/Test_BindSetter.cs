using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_BindSetter : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        UIBinder.Inst.CompleteRegist();
    }
}
