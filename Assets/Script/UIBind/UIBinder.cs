using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIBinder
{
    private static UIBinder instance = null;

    /// <summary>
    /// CallBackRegistComplete 함수를 사용해서 GetBindedData 접근 시점을 지정하세요. 
    /// 등록이 완료 되어있을 경우 즉시 callback을 호출하고 안 되어있을 경우 이 후 완료 시점에 callback을 호출합니다. 
    /// 
    /// UI Prefab 로딩이 완료되었을 때 Regist 과정이 완료된걸로 간주하고
    /// UI Prefab 로딩이 끝났을 때 호출 되는 OnEnableUI 이벤트 발생 시점에서 RegistComplete()이 호출됩니다. 
    /// </summary>
    public static UIBinder Inst
    {
        get
        {
            if (instance == null)
                instance = new UIBinder();
            return instance;
        }
    }
        
    Dictionary<N_UI_IDX, UIBind<int>> dic_N_UI = new Dictionary<N_UI_IDX, UIBind<int>>();
    Dictionary<F_UI_IDX, UIBind<float>> dic_F_UI = new Dictionary<F_UI_IDX, UIBind<float>>();
    Dictionary<L_UI_IDX, UIBind<long>> dic_L_UI = new Dictionary<L_UI_IDX, UIBind<long>>();
    Dictionary<B_UI_IDX, UIBind<bool>> dic_B_UI = new Dictionary<B_UI_IDX, UIBind<bool>>();
    Dictionary<D_UI_IDX, UIBind<double>> dic_D_UI = new Dictionary<D_UI_IDX, UIBind<double>>();
    Dictionary<S_UI_IDX, UIBind<string>> dic_S_UI = new Dictionary<S_UI_IDX, UIBind<string>>();

    bool isCompleteRegist = false;
    event deleFunc callbackCompleteRegist;

    protected const int nLogOption = 2;

    public void ClearAll()
    {
        isCompleteRegist = false;

        dic_N_UI.Clear();
        dic_F_UI.Clear();
        dic_L_UI.Clear();
        dic_B_UI.Clear();
        dic_D_UI.Clear();
        dic_S_UI.Clear();

        callbackCompleteRegist = null;
    }

    public void CompleteRegist()
    {
        UDL.LogWarning("Complete Regist UI_Binder", nLogOption);
        isCompleteRegist = true;

        if (callbackCompleteRegist != null)
        {
            callbackCompleteRegist();
            callbackCompleteRegist = null;
        }
    }

    public void SetCallbackCompleteRegist(deleFunc _callback)
    {
        if (isCompleteRegist)
            _callback();
        else
            callbackCompleteRegist += _callback;
    }
    
    #region RegistUI : 각 UI 컴포넌트에 붙어있는 UIBInd 상속 클래스들에서 자동으로 등록한다. 

    //public void RegistUI<T>(int idx, UIBind<T> ui)
    //{
    //    try
    //    {
    //        switch (Type.GetTypeCode(typeof(T)))
    //        {
    //            case TypeCode.Int64:
    //            case TypeCode.Int32:
    //                dic_N_UI.Add((N_UI_IDX)idx, ui as UIBind<int>);
    //                break;
    //            case TypeCode.Single:
    //                dic_F_UI.Add((F_UI_IDX)idx, ui as UIBind<float>);
    //                break;
    //            case TypeCode.UInt32:
    //            case TypeCode.UInt64:
    //                dic_L_UI.Add((L_UI_IDX)idx, ui as UIBind<long>);
    //                break;
    //            case TypeCode.Boolean:
    //                dic_B_UI.Add((B_UI_IDX)idx, ui as UIBind<bool>);
    //                break;
    //            case TypeCode.Double:
    //                dic_D_UI.Add((D_UI_IDX)idx, ui as UIBind<double>);
    //                break;
    //            case TypeCode.String:
    //                dic_S_UI.Add((S_UI_IDX)idx, ui as UIBind<string>);
    //                break;
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        UDL.LogError("UI_ID가 중복 설정되었습니다." + " // " + idx.ToString());
    //    }
    //}

    public void RegistUI(N_UI_IDX idx, UIBind<int> ui)
    {
        if (dic_N_UI.ContainsKey(idx))
        {
            UDL.LogError("UI_ID가 중복 설정되었습니다. " + idx.ToString());
            return;
        }

        dic_N_UI.Add(idx, ui);
    }
    public void RegistUI(F_UI_IDX idx, UIBind<float> ui)
    {
        if (dic_F_UI.ContainsKey(idx))
        {
            UDL.LogError("UI_ID가 중복 설정되었습니다. " + idx.ToString());
            return;
        }

        dic_F_UI.Add(idx, ui);
    }
    public void RegistUI(L_UI_IDX idx, UIBind<long> ui)
    {
        if (dic_L_UI.ContainsKey(idx))
        {
            UDL.LogError("UI_ID가 중복 설정되었습니다. " + idx.ToString());
            return;
        }

        dic_L_UI.Add(idx, ui);
    }
    public void RegistUI(B_UI_IDX idx, UIBind<bool> ui)
    {
        if (dic_B_UI.ContainsKey(idx))
        {
            UDL.LogError("UI_ID가 중복 설정되었습니다. " + idx.ToString());
            return;
        }

        dic_B_UI.Add(idx, ui);
    }
    public void RegistUI(D_UI_IDX idx, UIBind<double> ui)
    {
        if (dic_D_UI.ContainsKey(idx))
        {
            UDL.LogError("UI_ID가 중복 설정되었습니다. " + idx.ToString());
            return;
        }

        dic_D_UI.Add(idx, ui);
    }
    public void RegistUI(S_UI_IDX idx, UIBind<string> ui)
    {
        if (dic_S_UI.ContainsKey(idx))
        {
            UDL.LogError("UI_ID가 중복 설정되었습니다. " + idx.ToString());
            return;
        }

        dic_S_UI.Add(idx, ui);
    }

    #endregion

    #region GetBindedData : 등록되어있는 개체들에 바인딩 되어있는 값을 enum 인덱스를 통해 접근한다. 

    public Bindable<T> GetBindedData<T>(int idx)
    {
        switch (Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.Int64:
            case TypeCode.Int32:
                return dic_N_UI[(N_UI_IDX)idx].GetData() as Bindable<T>;
            case TypeCode.Single:
                return dic_F_UI[(F_UI_IDX)idx].GetData() as Bindable<T>;
            case TypeCode.UInt32:
            case TypeCode.UInt64:
                return dic_L_UI[(L_UI_IDX)idx].GetData() as Bindable<T>;
            case TypeCode.Boolean:
                return dic_B_UI[(B_UI_IDX)idx].GetData() as Bindable<T>;
            case TypeCode.Double:
                return dic_D_UI[(D_UI_IDX)idx].GetData() as Bindable<T>;
            case TypeCode.String:
                return dic_S_UI[(S_UI_IDX)idx].GetData() as Bindable<T>;
        }

        UDL.Log("request is not surport");

        return null;
    }

    public Bindable<int> GetBindedData(N_UI_IDX idx)
    {
        UIBind<int> result;

        if (dic_N_UI.TryGetValue(idx, out result))
            return result.GetData();

        UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());

        return new Bindable<int>();        
    }
    public Bindable<float> GetBindedData(F_UI_IDX idx)
    {
        UIBind<float> result;

        if (dic_F_UI.TryGetValue(idx, out result))
            return result.GetData();

        UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());

        return new Bindable<float>();
    }
    public Bindable<long> GetBindedData(L_UI_IDX idx)
    {
        UIBind<long> result;

        if (dic_L_UI.TryGetValue(idx, out result))
            return result.GetData();

        UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());

        return new Bindable<long>();
    }
    public Bindable<bool> GetBindedData(B_UI_IDX idx)
    {
        UIBind<bool> result;

        if (dic_B_UI.TryGetValue(idx, out result))
            return result.GetData();

        UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());

        return new Bindable<bool>();
    }
    public Bindable<double> GetBindedData(D_UI_IDX idx)
    {
        UIBind<double> result;

        if (dic_D_UI.TryGetValue(idx, out result))
            return result.GetData();

        UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());

        return new Bindable<double>();
    }
    public Bindable<string> GetBindedData(S_UI_IDX idx)
    {
        UIBind<string> result;

        if (dic_S_UI.TryGetValue(idx, out result))
            return result.GetData();

        UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());

        return new Bindable<string>();
    }

    #endregion
            
    #region Bind data 를 새로 셋팅한다. *주의사항 : 기존에 해당 UI와 bind 되어 있던 링크가 모두 깨지게 된다.

    public void Bind(Bindable<int> data, N_UI_IDX idx)
    {
        UIBind<int> result;

        if (dic_N_UI.TryGetValue(idx, out result))
            result.SetData(data);
        else
            UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());        
    }
    public void Bind(Bindable<float> data, F_UI_IDX idx)
    {
        UIBind<float> result;

        if (dic_F_UI.TryGetValue(idx, out result))
            result.SetData(data);
        else
            UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());
    }
    public void Bind(Bindable<long> data, L_UI_IDX idx)
    {
        UIBind<long> result;

        if (dic_L_UI.TryGetValue(idx, out result))
            result.SetData(data);
        else
            UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());
    }
    public void Bind(Bindable<bool> data, B_UI_IDX idx)
    {
        UIBind<bool> result;

        if (dic_B_UI.TryGetValue(idx, out result))
            result.SetData(data);
        else
            UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());
    }
    public void Bind(Bindable<double> data, D_UI_IDX idx)
    {
        UIBind<double> result;

        if (dic_D_UI.TryGetValue(idx, out result))
            result.SetData(data);
        else
            UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());
    }
    public void Bind(Bindable<string> data, S_UI_IDX idx)
    {
        UIBind<string> result;

        if (dic_S_UI.TryGetValue(idx, out result))
            result.SetData(data);
        else
            UDL.LogError("등록된 UI가 없습니다. " + idx.ToString());
    }     
    #endregion
}
