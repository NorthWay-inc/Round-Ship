using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UniRx;
using UnityEngine;

public class SaveableVariable<T>
{

    public readonly string NameValueForSave;

    private T _value;
    public T Value { get => _value; set => _value = value; }

    public SaveableVariable(string nameForSaveLoad,T value){
        NameValueForSave = nameForSaveLoad;
        _value = value;
    }
}
