using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    public InventoryItemData itemData;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SaveLoadManager.Instance.SaveGameToSaveFile();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SaveLoadManager.Instance.LoadGameFromSaveFile();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHolder>().InventorySystem.AddToInventory(itemData, 3, out _);
        }
    }
}
