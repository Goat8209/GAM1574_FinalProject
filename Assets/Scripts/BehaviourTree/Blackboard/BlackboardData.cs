using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[Serializable]
public class BlackboardData<T>
{
    [SerializeField] private List<BlackboardEntry<T>> entries = new List<BlackboardEntry<T>>();
    private Dictionary<string, int> indexLookups = new Dictionary<string, int>();
    private bool isIndexLookupsBuilt = false;
    private bool isIndexLookupsDirty = true;


    public void Add(string key, T value)
    {
        entries.Add(new BlackboardEntry<T>(key, value));
        isIndexLookupsDirty = true;
    }

    public void Remove(string key)
    {
        int index = GetIndex(key);
        if (index != -1)
        {
            entries.RemoveAt(index);
            indexLookups.Remove(key);
            isIndexLookupsDirty = true;
        }
        else 
        {
            Debug.LogAssertion("BlackboardData<" + typeof(T) + "> Remove failed, invalid key: " + key);
        }
    }

    public bool TryGetValue(string key, out T value)
    {
        int index = GetIndex(key);
        if (index != -1)
        {
            value = entries[index].value;
            return true;
        }

        value = default;
        return false;
    }

    public T GetValue(string key)
    {
        int index = GetIndex(key);
        if (index != -1)
        {
            return entries[index].value;
        }
        else
        {
            Debug.LogAssertion("BlackboardData<" + typeof(T) + "> GetValue failed, invalid key: " + key);
        }

        T value = default;
        return value;
    }

    public void SetOrAddValue(string key, T value)
    {
        if (ContainsKey(key))
        {
            SetValue(key, value);
        }
        else
        {
            Add(key, value);
        }
    }


    public void SetValue(string key, T value)
    {
        int index = GetIndex(key);
        if (index != -1)
        {
            entries[index].value = value;
        }
        else
        {
            Debug.LogAssertion("BlackboardBlackboardDataList<" + typeof(T) + "> SetValue failed, invalid key: " + key);
        }
    }

    public bool ContainsKey(string key)
    {
        BuildIndexLookups();
        return indexLookups.ContainsKey(key);
    }

    public List<int> CountOccurences(string key)
    {
        List<int> occurences = new List<int>();

        for (int i = 0; i < entries.Count; ++i)
        {
            if (entries[i].key == key)
            {
                occurences.Add(i);
            }
        }

        return occurences;
    }

    private int GetIndex(string key)
    {
        BuildIndexLookups();

        int index = -1;
        if (indexLookups.TryGetValue(key, out index))
            return index;

        return -1;
    }

    private void BuildIndexLookups()
    {
        if (indexLookups.Count == 0 && entries.Count > 0)
            isIndexLookupsBuilt = false;

        if (!isIndexLookupsBuilt || isIndexLookupsDirty)
        {
            indexLookups.Clear();

            for (int i = 0; i < entries.Count; ++i)
            {
                if (indexLookups.ContainsKey(entries[i].key) == false)
                {
                    indexLookups.Add(entries[i].key, i);
                }
            }

            isIndexLookupsBuilt = true;
            isIndexLookupsDirty = false;
        }
    }
}
