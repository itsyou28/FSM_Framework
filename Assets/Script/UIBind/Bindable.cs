using System.Collections;
using System.Collections.Generic;

public delegate void deleFunc();

public class Bindable<T>
{
    private T value;

    public event deleFunc valueChanged;

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
