using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BindableRepo
{
    private static BindableRepo instance = null;
    public static BindableRepo Inst
    {
        get
        {
            if (instance == null)
                instance = new BindableRepo();
            return instance;
        }
    }

    Dictionary<N_Bind_Idx, Bindable<int>> dic_N = new Dictionary<N_Bind_Idx, Bindable<int>>();
    Dictionary<F_Bind_Idx, Bindable<float>> dic_F = new Dictionary<F_Bind_Idx, Bindable<float>>();
    Dictionary<S_Bind_Idx, Bindable<string>> dic_S = new Dictionary<S_Bind_Idx, Bindable<string>>();
    Dictionary<FF_Bind_Idx, Bindable<float, float>> dic_FF = new Dictionary<FF_Bind_Idx, Bindable<float, float>>();

    public Bindable<int> GetBindedData(N_Bind_Idx idx)
    {
        Bindable<int> result;

        if (dic_N.TryGetValue(idx, out result))
            return result;

        dic_N.Add(idx, new Bindable<int>());

        return dic_N[idx];
    }

    public Bindable<float> GetBindedData(F_Bind_Idx idx)
    {
        Bindable<float> result;

        if (dic_F.TryGetValue(idx, out result))
            return result;

        dic_F.Add(idx, new Bindable<float>());

        return dic_F[idx];
    }

    public Bindable<string> GetBindedData(S_Bind_Idx idx)
    {
        Bindable<string> result;

        if (dic_S.TryGetValue(idx, out result))
            return result;

        dic_S.Add(idx, new Bindable<string>());

        return dic_S[idx];
    }

    public Bindable<float, float> GetBindedData(FF_Bind_Idx idx)
    {
        Bindable<float, float> result;

        if (dic_FF.TryGetValue(idx, out result))
            return result;

        dic_FF.Add(idx, new Bindable<float, float>());

        return dic_FF[idx];
    }
}