using UnityEngine;
using System.Collections;

public class UIBindD : UIBind<double>
{
    [SerializeField]
    D_UI_IDX UI_ID;

    protected override void Awake()
    {
#if DEBUG_LOG
        enumType = UI_ID.GetType();
        UI_IDX = (int)UI_ID;
#endif
        base.Awake();

        UIBinder.Inst.RegistUI(UI_ID, this);
    }
}
