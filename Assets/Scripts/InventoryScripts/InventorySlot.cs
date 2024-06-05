/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/20/24
*    Description: A class for inventory slots that inventory system
*    (basically a list of slots) uses. 
*******************************************************************/
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;
    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;


    public InventorySlot()
    {
        EmptyThisSlot();
    }
    public InventorySlot(InventoryItemData thing, int amount)
    {
        itemData = thing;
        stackSize = amount;
    }
    public InventoryItemData GetItemData()
    {
        return itemData;
    }

    /// <summary>
    /// Going to need this if i want an empty inventory slot, so might as well 
    /// redirect the default constructor here.
    /// </summary>
    public void EmptyThisSlot()
    {
        itemData = null;
        stackSize = -1; //Common convention for "null" primitive
    }
    /// <summary>
    /// So we dont have to destroy and remake an inventory slot
    /// </summary>
    public void UpdateThisSlot(InventoryItemData item, int amount)
    {
        itemData = item;
        stackSize = amount;
    }
    //public bool RoomInStack(int amountToAdd, out int amountRemaining)
    //{
    //    amountRemaining = ItemData.MaxStackSize - amountToAdd;
    //    return RoomLeftInStack(amountToAdd);
    //}
    public bool RoomLeftInStackInSlot(int amountToAdd)
    {
        if(StackSize + amountToAdd <= ItemData.MaxStackSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// Could have combined this and remove method but decided not to for 
    /// clarity down the line
    /// </summary>
    public void AddToStackInThisSlot(int amount)
    {
        stackSize += amount;
    }
    public void RemoveFromStackInThisSlot(int amount)
    {
        stackSize -= amount;
        if (stackSize <= 0)
        {
            EmptyThisSlot();
        }
    }
    public int GetRoomLeftInStack()
    {
        if (itemData == null)
        {
            return 0; // No room in an empty slot for items that don't exist.
        }

        return ItemData.MaxStackSize - stackSize;
    }
}
