/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/20/24
*    Description: THIS IS WHAT YOU PUT ON A GAMEOBJECT. OTHER ISCRIPTS
*        ARE HELPERS/CHILDREN/NOT MONOBEHAVIORS.
*        An inventory holder can be anything - a backpack, hotbar, NPC's
*        pockets, a chest/closet/hole, etc. Yippee, flexibility.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHolder : MonoBehaviour
{

    [SerializeField] protected InventorySystem inventorySystem;
    [SerializeField] private int inventorySize;

    public InventoryItemData data;
    public InventorySystem InventorySystem => inventorySystem;
    public int temp;
    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
        
    }
    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            inventorySystem.AddToInventory(data, 1, out temp);
            print(temp);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            inventorySystem.AddToInventory(data, 2, out temp);
            print(temp);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            inventorySystem.AddToInventory(data, 3, out temp);
            print(temp);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            inventorySystem.AddToInventory(data, 4, out temp);
            print(temp);
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            inventorySystem.RemoveFromInventory(data, 1, true, out _, out temp);
            print("Removed: " + temp);
        }
        if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            inventorySystem.RemoveFromInventory(data, 2, true, out _, out temp);
            print("Removed: " + temp);
        }
        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            inventorySystem.RemoveFromInventory(data, 3, true, out _, out temp);
            print("Removed: " + temp);
        }
        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            inventorySystem.RemoveFromInventory(data, 4, true, out _, out temp);
            print("Removed: " + temp);
        }
    }
}
