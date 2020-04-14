using System.Collections;
using System.Collections.Generic;

public class MyList<T> : List<T> {

    public delegate void AddEventHandle();
    public event AddEventHandle AddEvent;

    public new void Add(T item) {
        base.Add(item);
        AddEvent();
    }
}
