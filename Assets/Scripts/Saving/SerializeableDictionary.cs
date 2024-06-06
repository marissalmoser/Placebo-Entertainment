/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 6/3/24
*    Description: This class allows Unity to serialize and 
*    deserialize dictionaries, which it does not natively support. 
*    This allows me to save an inventory (item and amount kvp) to 
*    JSON easily
*******************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

//Inherit from the Dictionary class with generic values. Need ISerialization 
//for its methods; below
[Serializable]
public class SerializeableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    //making two serialized lists, one for keys, one for values! Crazy
    [SerializeField]
    private List<TKey> keys = new List<TKey>();
    [SerializeField]
    private List<TValue> values = new List<TValue>();
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        //kvp is key-value-pair
        foreach (KeyValuePair<TKey, TValue> kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keys.Count != values.Count)
        {
            throw new System.Exception(
                "HUGE oopsies, key-value pairs in your serialized dictionary are mismatched");
        }
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
