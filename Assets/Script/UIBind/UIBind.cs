using UnityEngine;
using System.Collections;

public class UIBind<T> : MonoBehaviour
{
    protected const int nLogOption = (int)LogOption.UI_Binder;

    public bool isDebug = false;

    protected Bindable<T> bindedData = new Bindable<T>();
    
    protected System.Type enumType;
    protected int UI_IDX; 

    protected virtual void Awake()
    {
        bindedData.valueChanged += OnDataChange;

        UDL.Log(gameObject.name + " // " + System.Enum.Format(enumType, UI_IDX, "g"), nLogOption);
    }

    public Bindable<T> GetData()
    {
        return bindedData;
    }

    /// <summary>
    /// Bind data 를 새로 셋팅한다. 
    /// 주의사항 기존에 해당 UI와 bind 되어 있던 링크가 모두 깨지게 된다. 
    /// </summary>
    public void SetData(Bindable<T> data)
    {
        UDL.Log("SetData : " + System.Enum.Format(enumType, UI_IDX, "g"), nLogOption); 
        bindedData = data;
        bindedData.valueChanged += OnDataChange;
    }

    protected virtual void OnDataChange()
    {
        UDL.LogWarning(System.Enum.Format(enumType, UI_IDX, "g") + " // bindedData.Value : " + bindedData.Value, nLogOption, isDebug);
    }

    private void OnEnable()
    {
        OnDataChange();
    }

    protected virtual void OnDestroy()
    {
        bindedData.valueChanged -= OnDataChange;
        UDL.LogWarning(System.Enum.Format(enumType, UI_IDX, "g") + " // OnDestroy!! // bindedData.Value : " + bindedData.Value, nLogOption, isDebug);
    }
}



