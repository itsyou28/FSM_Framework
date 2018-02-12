using UnityEngine;
using System.Collections;

public class UIBind : MonoBehaviour
{
    protected const int nLogOption = (int)LogOption.UI_Binder;

    public bool isDebug = false;

#if DEBUG_LOG
    protected System.Type enumType;
    protected int UI_IDX;  
#endif

    protected virtual void Awake()
    {
    }    

    protected virtual void OnDataChange()
    {
    }

    protected virtual void OnEnable()
    {
        OnDataChange();
    }

    protected virtual void OnDestroy()
    {
    }
}

public class UIBind<T> : UIBind
{
    protected Bindable<T> bindedData;

    protected override void Awake()
    {
        if (bindedData == null)
            UDL.LogError("No binded data " + gameObject.name + " // " + System.Enum.Format(enumType, UI_IDX, "g"));

        bindedData.valueChanged += OnDataChange;

        UDL.Log(gameObject.name + " // " + System.Enum.Format(enumType, UI_IDX, "g"), nLogOption);
    }

    public Bindable<T> GetData()
    {
        return bindedData;
    }

    public void SetData(Bindable<T> data)
    {
        UDL.Log("SetData : " + System.Enum.Format(enumType, UI_IDX, "g"), nLogOption);
        bindedData = data;
        bindedData.valueChanged += OnDataChange;
    }
    protected override void OnDataChange()
    {
        UDL.LogWarning(System.Enum.Format(enumType, UI_IDX, "g") + " // bindedData.Value : " + bindedData.Value, nLogOption, isDebug);
    }

    protected override void OnDestroy()
    {
        bindedData.valueChanged -= OnDataChange;
        UDL.LogWarning(System.Enum.Format(enumType, UI_IDX, "g") + " // OnDestroy!! // bindedData.Value : " + bindedData.Value, nLogOption, isDebug);
    }
}

public class UIBind<T1, T2> : UIBind
{
    protected Bindable<T1, T2> bindedData;

    protected override void Awake()
    {
        if (bindedData == null)
            UDL.LogError("No binded data " + gameObject.name + " // " + System.Enum.Format(enumType, UI_IDX, "g"));

        bindedData.value1Changed += OnValue1Changed;
        bindedData.value2Changed += OnValue2Changed;

        UDL.Log(gameObject.name + " // " + System.Enum.Format(enumType, UI_IDX, "g"), nLogOption);
    }

    private void OnValue1Changed()
    {
        UDL.LogWarning(System.Enum.Format(enumType, UI_IDX, "g") + " // bindedData.Value1 : " + bindedData.Value1, nLogOption, isDebug);
    }

    private void OnValue2Changed()
    {
        UDL.LogWarning(System.Enum.Format(enumType, UI_IDX, "g") + " // bindedData.Value2 : " + bindedData.Value2, nLogOption, isDebug);
    }

    public Bindable<T1, T2> GetData()
    {
        return bindedData;
    }

    public void SetData(Bindable<T1, T2> data)
    {
        UDL.Log("SetData : " + System.Enum.Format(enumType, UI_IDX, "g"), nLogOption);
        bindedData = data;
        bindedData.value1Changed += OnValue1Changed;
        bindedData.value2Changed += OnValue2Changed;
    }

    protected override void OnDestroy()
    {
        bindedData.value1Changed -= OnValue1Changed;
        bindedData.value2Changed -= OnValue2Changed;
        UDL.LogWarning(System.Enum.Format(enumType, UI_IDX, "g") + " // OnDestroy!! ", nLogOption, isDebug);
    }
}

public class UIBindN : UIBind<int>
{
    [SerializeField]
    N_Bind_Idx bindTarget;

    protected override void Awake()
    {
#if DEBUG_LOG
        enumType = bindTarget.GetType();
        UI_IDX = (int)bindTarget;
#endif
        bindedData = BindableRepo.Inst.GetBindedData(bindTarget);

        base.Awake();
    }
}

public class UIBindF : UIBind<float>
{
    [SerializeField]
    F_Bind_Idx bindTarget;

    protected override void Awake()
    {
#if DEBUG_LOG
        enumType = bindTarget.GetType();
        UI_IDX = (int)bindTarget;
#endif
        bindedData = BindableRepo.Inst.GetBindedData(bindTarget);

        base.Awake();
    }
}

public class UIBindS : UIBind<string>
{
    [SerializeField]
    S_Bind_Idx bindTarget;

    protected override void Awake()
    {
#if DEBUG_LOG
        enumType = bindTarget.GetType();
        UI_IDX = (int)bindTarget;
#endif
        bindedData = BindableRepo.Inst.GetBindedData(bindTarget);

        base.Awake();
    }
}

public class UIBindFF : UIBind<float, float>
{
    [SerializeField]
    FF_Bind_Idx bindTarget;

    protected override void Awake()
    {
#if DEBUG_LOG
        enumType = bindTarget.GetType();
        UI_IDX = (int)bindTarget;
#endif
        bindedData = BindableRepo.Inst.GetBindedData(bindTarget);

        base.Awake();
    }
}