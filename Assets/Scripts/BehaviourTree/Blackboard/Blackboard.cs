using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "new Blackboard", menuName = "Behaviour Tree/Blackboard")]
public class Blackboard : ScriptableObject
{
    public enum ValueType { Int, Float, Bool, Vector3, String }

    [SerializeField] private BlackboardData<int> intData;
    [SerializeField] private BlackboardData<float> floatData;
    [SerializeField] private BlackboardData<bool> boolData;
    [SerializeField] private BlackboardData<Vector3> vector3Data;
    [SerializeField] private BlackboardData<string> stringData;

    public void Add<T>(string key, T value)
    {
        if (typeof(T) == typeof(int))
        {
            int intValue = (int)((object)value);
            intData.Add(key, intValue);
        }
        else if (typeof(T) == typeof(float))
        {
            float floatValue = (float)((object)value);
            floatData.Add(key, floatValue);
        }
        else if (typeof(T) == typeof(bool))
        {
            bool boolValue = (bool)((object)value);
            boolData.Add(key, boolValue);
        }
        else if (typeof(T) == typeof(Vector3))
        {
            Vector3 vector3Value = (Vector3)((object)value);
            vector3Data.Add(key, vector3Value);
        }
        else if (typeof(T) == typeof(string))
        {
            string stringValue = (string)((object)value);
            stringData.Add(key, stringValue);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public void Remove(string key, ValueType type)
    {
        if (type == ValueType.Int)
        {
            intData.Remove(key);
        }
        else if (type == ValueType.Float)
        {
            floatData.Remove(key);
        }
        else if (type == ValueType.Bool)
        {
            boolData.Remove(key);
        }
        else if (type == ValueType.Vector3)
        {
            vector3Data.Remove(key);
        }
        else if (type == ValueType.String)
        {
            stringData.Remove(key);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public bool TryGetValue<T>(string key, out T value)
    {
        if (typeof(T) == typeof(int))
        {
            int intValue;
            bool result = intData.TryGetValue(key, out intValue);
            value = (T)((object)intValue);
            return result;
        }
        else if (typeof(T) == typeof(float))
        {
            float floatValue;
            bool result = floatData.TryGetValue(key, out floatValue);
            value = (T)((object)floatValue);
            return result;
        }
        else if (typeof(T) == typeof(bool))
        {
            bool boolValue;
            bool result = boolData.TryGetValue(key, out boolValue);
            value = (T)((object)boolValue);
            return result;
        }
        else if (typeof(T) == typeof(Vector3))
        {
            Vector3 vector3Value;
            bool result = vector3Data.TryGetValue(key, out vector3Value);
            value = (T)((object)vector3Value);
            return result;
        }
        else if (typeof(T) == typeof(string))
        {
            string stringValue;
            bool result = stringData.TryGetValue(key, out stringValue);
            value = (T)((object)stringValue);
            return result;
        }

        value = default;
        return false;
    }

    public T GetValue<T>(string key)
    {
        if (typeof(T) == typeof(int))
        {
            int intValue = intData.GetValue(key);
            T value = (T)((object)intValue);
            return value;
        }
        else if (typeof(T) == typeof(float))
        {
            float floatValue = floatData.GetValue(key);
            T value = (T)((object)floatValue);
            return value;
        }
        else if (typeof(T) == typeof(bool))
        {
            bool boolValue = boolData.GetValue(key);
            T value = (T)((object)boolValue);
            return value;
        }
        else if (typeof(T) == typeof(Vector3))
        {
            Vector3 vector3Value = vector3Data.GetValue(key);
            T value = (T)((object)vector3Value);
            return value;
        }
        else if (typeof(T) == typeof(string))
        {
            string stringValue = stringData.GetValue(key);
            T value = (T)((object)stringValue);
            return value;
        }

        T defaultValue = default;
        return defaultValue;
    }

    public void SetOrAddValue<T>(string key, T value)
    {
        if (typeof(T) == typeof(int))
        {
            int intValue = (int)((object)value);
            intData.SetOrAddValue(key, intValue);
        }
        else if (typeof(T) == typeof(float))
        {
            float floatValue = (float)((object)value);
            floatData.SetOrAddValue(key, floatValue);
        }
        else if (typeof(T) == typeof(bool))
        {
            bool boolValue = (bool)((object)value);
            boolData.SetOrAddValue(key, boolValue);
        }
        else if (typeof(T) == typeof(Vector3))
        {
            Vector3 vector3Value = (Vector3)((object)value);
            vector3Data.SetOrAddValue(key, vector3Value);
        }
        else if (typeof(T) == typeof(string))
        {
            string stringValue = (string)((object)value);
            stringData.SetOrAddValue(key, stringValue);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public void SetValue<T>(string key, T value)
    {
        if (typeof(T) == typeof(int))
        {
            int intValue = (int)((object)value);
            intData.SetValue(key, intValue);
        }
        else if (typeof(T) == typeof(float))
        {
            float floatValue = (float)((object)value);
            floatData.SetValue(key, floatValue);
        }
        else if (typeof(T) == typeof(bool))
        {
            bool boolValue = (bool)((object)value);
            boolData.SetValue(key, boolValue);
        }
        else if (typeof(T) == typeof(Vector3))
        {
            Vector3 vector3Value = (Vector3)((object)value);
            vector3Data.SetValue(key, vector3Value);
        }
        else if (typeof(T) == typeof(string))
        {
            string stringValue = (string)((object)value);
            stringData.SetValue(key, stringValue);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public bool ContainsKey(string key, ValueType type)
    {
        if (type == ValueType.Int)
        {
            return intData.ContainsKey(key);
        }
        else if (type == ValueType.Float)
        {
            return floatData.ContainsKey(key);
        }
        else if (type == ValueType.Bool)
        {
            return boolData.ContainsKey(key);
        }
        else if (type == ValueType.Vector3)
        {
            return vector3Data.ContainsKey(key);
        }
        else if (type == ValueType.String)
        {
            return stringData.ContainsKey(key);
        }

        return false;
    }

    public List<int> CountOccurences(string key, ValueType type)
    {
        List<int> occurences = new List<int>();

        if (type == ValueType.Int)
        {
            occurences = intData.CountOccurences(key);
        }
        else if (type == ValueType.Float)
        {
            occurences = floatData.CountOccurences(key);
        }
        else if (type == ValueType.Bool)
        {
            occurences = boolData.CountOccurences(key);
        }
        else if (type == ValueType.Vector3)
        {
            occurences = vector3Data.CountOccurences(key);
        }
        else if (type == ValueType.String)
        {
            occurences = stringData.CountOccurences(key);
        }

        return occurences;
    }
}
