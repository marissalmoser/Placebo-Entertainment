/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 6/4/24
*    Description: This class allows Unity to serialize and 
*    deserialize dictionaries, which it does not natively support. 
*    Followed tutorial and as of 6/4/24 realized it doesnt need to be,
*    the tutorial is being  (((extra)))  and a normal dictionary would
*    suffice. Live and learn. This was all copy paste anyways, pretty 
*    simple stuff
*******************************************************************/
using System.Collections.Generic;
using UnityEngine;

//Inherit from the Dictionary class with generic values. Need ISerialization 
//for its methods; below
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
