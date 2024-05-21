/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/20/24
*    Description: [brief description about what the script does]
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventorySystem 
{
    private int inventorySlotCount => collectionOfSlots.Count;
    [SerializeField] private List<InventorySlot> collectionOfSlots;
    public List<InventorySlot> CollectionOfSlots => collectionOfSlots;


    //Can do this because the constructor will ask for a count. This sets iSC
    //after the fact, so we dont have to write that code out. 

    public UnityAction<InventorySlot> slotItemChangedInInventory; 
    //fires when item is added to inventory

    public InventorySystem(int size)
    {
        collectionOfSlots = new List<InventorySlot>(size);
        //Filling up the inventory system with slots
        for(int i = 0; i < size; i++)
        {
            collectionOfSlots.Add(new InventorySlot());
            //uses default constructor from InventorySlot.cs
        }
    }
    /// <summary>
    /// This is a bool cause we want whatever wants to be added to check if 
    /// there's room, and if there isn't, return false and whatever is attempting
    /// to add will act accordingly. If we are shooting for a really simple 
    /// inventory system, we will just set some obscenely high slot count with
    /// an invisisble inventory.
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        //If we have one of one item, and pick up the same item, we can just increment count
        if(ContainsItem(itemToAdd, out InventorySlot slotItemIsIn))
        {
            slotItemIsIn.AddToStack(amountToAdd);
            return true;
        }
        else if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateSlot(itemToAdd, amountToAdd);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// This will pass back out the first slot our searcher comes across
    /// </summary>
    /// <param name="freeSlot"></param>
    /// <returns></returns>
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = null;
        return false;
    }
    public bool ContainsItem(InventoryItemData itemToAdd, out InventorySlot slotToGo)
    {
        slotToGo = null;
        return false;
    }
}
