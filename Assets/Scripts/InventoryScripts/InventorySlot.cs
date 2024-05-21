/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/20/24
*    Description: [brief description about what the script does]
*******************************************************************/
using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;
    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;



    /// <summary>
    /// Default constructor
    /// </summary>
    public InventorySlot()
    {
        EmptySlot();
    }

    /// <summary>
    /// Constructor w overloads
    /// </summary>
    /// <param name="itemData"></param>
    public InventorySlot(InventoryItemData thing, int amount)
    {
        itemData = thing;
        stackSize = amount;
    }

    /// <summary>
    /// Going to need this if i want an empty inventory slot, so might as well 
    /// redirect the default constructor here.
    /// </summary>
    public void EmptySlot()
    {
        itemData = null;
        stackSize = -1; //Common convention for "null" primitive
    }
    /// <summary>
    /// So we dont have to destroy and remake an inventory slot
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void UpdateSlot(InventoryItemData item, int amount)
    {
        itemData = item;
        stackSize = amount;
    }

    public bool RoomInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = ItemData.MaxStackSize - amountToAdd;
        return RoomLeftInStack(amountToAdd);
    }
    public bool RoomLeftInStack(int amountToAdd)
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
    /// Could have combined these but decided not to for clarity down the line
    /// </summary>
    /// <param name="amount"></param>
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }
    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }
}
