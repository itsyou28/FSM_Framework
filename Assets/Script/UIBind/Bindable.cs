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
