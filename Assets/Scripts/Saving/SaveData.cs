using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    public SerializeableDictionary<string, InventorySystem> inventoryDictionary;
    public SaveData()
    {
        inventoryDictionary = new SerializeableDictionary<string, InventorySystem>();
    }
    public void CollectInventoryData()
    {
        InventoryHolder[] inventoryHolders = GameObject.FindObjectsOfType<InventoryHolder>();

        foreach (var holder in inventoryHolders)
        {
            string objectName = holder.gameObject.name;
            InventorySystem inventorySystem = holder.InventorySystem;

            if (!inventoryDictionary.ContainsKey(objectName))
            {
                inventoryDictionary.Add(objectName, inventorySystem);
            }
        }
    }
}
