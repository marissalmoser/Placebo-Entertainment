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

    [SerializeField] protected InventorySystem _inventorySystem;
    [SerializeField] private int _inventorySize;

    public InventorySystem InventorySystem => _inventorySystem;
    private void Awake()
    {
        _inventorySystem = new InventorySystem(_inventorySize);     
    }
    public void SetInventorySystem(InventorySystem system)
    {
        _inventorySystem = system;
        foreach(InventorySlot slot in _inventorySystem.CollectionOfSlots)
        {
            if(slot.GetItemData() != null)
            {
                if (slot.GetItemData().DoesNotPersist)
                {
                    slot.EmptyThisSlot();
                }
            }
        }
    }
}
