using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using Unity.VisualScripting.ReorderableList;
using System.ComponentModel;
using UnityEditor.UIElements;


[CustomEditor(typeof(Blackboard))]
public class BlackboardEditor : Editor
{
    Blackboard blackboard;
    ReorderableList reorderableListInt;
    ReorderableList reorderableListFloat;
    ReorderableList reorderableListBool;
    ReorderableList reorderableListVector3;
    ReorderableList reorderableListString;

    void OnEnable()
    {
        // Get the blackboard object
        blackboard = (Blackboard)target;

        // Create a ReorderableList for the intData
        SerializedProperty intData = serializedObject.FindProperty("intData");
        SerializedProperty intEntries = intData.FindPropertyRelative("entries");
        reorderableListInt = new ReorderableList(serializedObject, intEntries, false, true, true, false);
        reorderableListInt.elementHeightCallback += OnGetElementHeight;
        reorderableListInt.drawNoneElementCallback += OnDrawNoneElement;
        reorderableListInt.drawHeaderCallback += OnDrawHeaderInt;
        reorderableListInt.onAddCallback += OnAddInt;
        reorderableListInt.drawElementCallback += OnDrawElementInt;


        // Create a ReorderableList for the floatData
        SerializedProperty floatData = serializedObject.FindProperty("floatData");
        SerializedProperty floatEntries = floatData.FindPropertyRelative("entries");
        reorderableListFloat = new ReorderableList(serializedObject, floatEntries, false, true, true, false);
        reorderableListFloat.elementHeightCallback += OnGetElementHeight;
        reorderableListFloat.drawNoneElementCallback += OnDrawNoneElement;
        reorderableListFloat.drawHeaderCallback += OnDrawHeaderFloat;
        reorderableListFloat.onAddCallback += OnAddFloat;
        reorderableListFloat.drawElementCallback += OnDrawElementFloat;


        // Create a ReorderableList for the boolData
        SerializedProperty boolData = serializedObject.FindProperty("boolData");
        SerializedProperty boolEntries = boolData.FindPropertyRelative("entries");
        reorderableListBool = new ReorderableList(serializedObject, boolEntries, false, true, true, false);
        reorderableListBool.elementHeightCallback += OnGetElementHeight;
        reorderableListBool.drawNoneElementCallback += OnDrawNoneElement;
        reorderableListBool.drawHeaderCallback += OnDrawHeaderBool;
        reorderableListBool.onAddCallback += OnAddBool;
        reorderableListBool.drawElementCallback += OnDrawElementBool;


        // Create a ReorderableList for the vector3Data
        SerializedProperty vector3Data = serializedObject.FindProperty("vector3Data");
        SerializedProperty vector3Entries = vector3Data.FindPropertyRelative("entries");
        reorderableListVector3 = new ReorderableList(serializedObject, vector3Entries, false, true, true, false);
        reorderableListVector3.elementHeightCallback += OnGetElementHeight;
        reorderableListVector3.drawNoneElementCallback += OnDrawNoneElement;
        reorderableListVector3.drawHeaderCallback += OnDrawHeaderVector3;
        reorderableListVector3.onAddCallback += OnAddVector3;
        reorderableListVector3.drawElementCallback += OnDrawElementVector3;


        // Create a ReorderableList for the stringData
        SerializedProperty stringData = serializedObject.FindProperty("stringData");
        SerializedProperty stringEntries = stringData.FindPropertyRelative("entries");
        reorderableListString = new ReorderableList(serializedObject, stringEntries, false, true, true, false);
        reorderableListString.elementHeightCallback += OnGetElementHeight;
        reorderableListString.drawNoneElementCallback += OnDrawNoneElement;
        reorderableListString.drawHeaderCallback += OnDrawHeaderString;
        reorderableListString.onAddCallback += OnAddString;
        reorderableListString.drawElementCallback += OnDrawElementString;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableListInt.DoLayoutList();
        reorderableListFloat.DoLayoutList();
        reorderableListBool.DoLayoutList();
        reorderableListVector3.DoLayoutList();
        reorderableListString.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }


    private float OnGetElementHeight(int index)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    private void OnDrawNoneElement(Rect rect)
    {
        EditorGUI.LabelField(rect, EditorGUIUtility.TrTextContent("Dictionary is empty"));
    }

    private void OnDrawHeader(Rect rect, string label)
    {
        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * 0.3f, EditorGUIUtility.singleLineHeight), "Key");
        EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.32f, rect.y, rect.width * 0.66f, EditorGUIUtility.singleLineHeight), "Value (" + label + ")");
    }

    private void OnDrawHeaderInt(Rect rect)
    {
        OnDrawHeader(rect, "int");
    }

    private void OnDrawHeaderFloat(Rect rect)
    {
        OnDrawHeader(rect, "float");
    }

    private void OnDrawHeaderBool(Rect rect)
    {
        OnDrawHeader(rect, "bool");
    }

    private void OnDrawHeaderVector3(Rect rect)
    {
        OnDrawHeader(rect, "Vector3");
    }
    private void OnDrawHeaderString(Rect rect)
    {
        OnDrawHeader(rect, "string");
    }

    private SerializedProperty OnAdd(ReorderableList list)
    {
        list.serializedProperty.arraySize++;
        SerializedProperty addedElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
        addedElement.FindPropertyRelative("key").stringValue = "";
        return addedElement;
    }

    private void OnAddInt(ReorderableList list)
    {
        SerializedProperty addedElement = OnAdd(list);
        addedElement.FindPropertyRelative("value").intValue = 0;
    }

    private void OnAddFloat(ReorderableList list)
    {
        SerializedProperty addedElement = OnAdd(list);
        addedElement.FindPropertyRelative("value").floatValue = 0.0f;
    }

    private void OnAddBool(ReorderableList list)
    {
        SerializedProperty addedElement = OnAdd(list);
        addedElement.FindPropertyRelative("value").boolValue = false;
    }

    private void OnAddVector3(ReorderableList list)
    {
        SerializedProperty addedElement = OnAdd(list);
        addedElement.FindPropertyRelative("value").vector3Value = Vector3.zero;
    }

    private void OnAddString(ReorderableList list)
    {
        SerializedProperty addedElement = OnAdd(list);
        addedElement.FindPropertyRelative("value").stringValue = "";
    }

    private void OnDrawElement(SerializedProperty element, Blackboard.ValueType valueType, Rect rect, int index)
    {
        SerializedProperty key = element.FindPropertyRelative("key");
        SerializedProperty value = element.FindPropertyRelative("value");

        Color previousColor = GUI.color;

        if (key.stringValue != "")
        {
            var occurences = blackboard.CountOccurences(key.stringValue, valueType);
            if (occurences.Count > 1)
            {
                GUI.color = occurences[0] == index ? Color.yellow : Color.red;
            }
        }
        else
        {
            GUI.color = Color.yellow;
        }

        Rect keyRect = new Rect(rect.x, rect.y, rect.width * 0.28f, EditorGUIUtility.singleLineHeight);
        Rect valueRect = new Rect(rect.x + rect.width * 0.30f, rect.y, rect.width * 0.5f, EditorGUIUtility.singleLineHeight);
        Rect buttonRect = new Rect(rect.x + rect.width * 0.82f, rect.y, rect.width * 0.175f, EditorGUIUtility.singleLineHeight);

        EditorGUI.PropertyField(keyRect, key, GUIContent.none);
        EditorGUI.PropertyField(valueRect, value, GUIContent.none);
        GUI.color = previousColor;

        if (GUI.Button(buttonRect, "Remove"))
        {
            blackboard.Remove(key.stringValue, valueType);
        }
    }

    private void OnDrawElementInt(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = reorderableListInt.serializedProperty.GetArrayElementAtIndex(index);
        OnDrawElement(element, Blackboard.ValueType.Int, rect, index);
    }

    private void OnDrawElementFloat(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = reorderableListFloat.serializedProperty.GetArrayElementAtIndex(index);
        OnDrawElement(element, Blackboard.ValueType.Float, rect, index);
    }

    private void OnDrawElementBool(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = reorderableListBool.serializedProperty.GetArrayElementAtIndex(index);
        OnDrawElement(element, Blackboard.ValueType.Bool, rect, index);
    }

    private void OnDrawElementVector3(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = reorderableListVector3.serializedProperty.GetArrayElementAtIndex(index);
        OnDrawElement(element, Blackboard.ValueType.Vector3, rect, index);
    }

    private void OnDrawElementString(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = reorderableListString.serializedProperty.GetArrayElementAtIndex(index);
        OnDrawElement(element, Blackboard.ValueType.String, rect, index);
    }
}
