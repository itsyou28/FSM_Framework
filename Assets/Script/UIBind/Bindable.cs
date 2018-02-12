using System.Collections;
using System.Collections.Generic;
using System;

public class Bindable<T>
{
    private T value;

    public event Action valueChanged;

    public T Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            OnValueChange();
        }
    }

    void OnValueChange()
    {
        if (valueChanged != null)
            valueChanged();
    }
}

public class Bindable<T1,T2>
{
    private T1 value1;
    private T2 value2;

    public event Action value1Changed;
    public event Action value2Changed;

    public T1 Value1
    {
        get
        {
            return value1;
        }
        set
        {
            value1 = value;
            OnValue1Change();
        }
    }

    public T2 Value2
    {
        get
        {
            return value2;
        }
        set
        {
            value2 = value;
            OnValue2Change();
        }
    }

    void OnValue1Change()
    {
        if (value1Changed != null)
            value1Changed();
    }

    void OnValue2Change()
    {
        if (value2Changed != null)
            value2Changed();
    }
}
