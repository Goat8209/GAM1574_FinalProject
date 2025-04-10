using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlackboardEntry<T>
{
    public string key;
    public T value;

    public BlackboardEntry(string key, T value)
    {
        this.key = key;
        this.value = value;
    }
}
