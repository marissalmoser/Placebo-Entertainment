/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/20/24
*    Description: Inventory system is just what inventory holders use
*    for functionality
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class InventorySystem 
{
    private int inventorySlotCount => collectionOfSlots.Count;
    //Can do this because the constructor will ask for a count. This sets iSC
    //after the fact, so we dont have to write that code out. 

    [SerializeField] private List<InventorySlot> collectionOfSlots;
    public List<InventorySlot> CollectionOfSlots => collectionOfSlots;


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
    /// to add will act accordingly.
    /// 
    /// If we are shooting for a really simple 
    /// inventory system, we will just set some obscenely high slot count with
    /// an invisisble inventory.
    /// 
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        //If we have one of one item, and pick up the same item, we can just increment count
        if(ContainsItem(itemToAdd, out List<InventorySlot> slotItemIsIn))
        {
            foreach(var slot in slotItemIsIn)
            {
                if(slot.RoomLeftInStackInSlot(amountToAdd))
                {
                    slot.AddToStackInThisSlot(amountToAdd);
                    return true;
                }
            }
           
        }
        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateThisSlot(itemToAdd, amountToAdd);
            return true;
        }
        return false;
    }
    /// <summary>
    /// This will pass back out the first free slot our searcher comes across
    /// </summary>
    /// <param name="freeSlot"></param>
    /// <returns></returns>
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = collectionOfSlots.FirstOrDefault(slotBeingLookedAt => slotBeingLookedAt.ItemData == (null));
        if(freeSlot != null)
        {
            return true;
        }
        return false;
    }
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> slotToGo)
    {
        //here, we look through our collection of slots (inventory or other) and gather
        //all the ones that match the item we picked up/want to compare (itemToAdd) and 
        //add them to a list. Ex. if we want to add an apple, and apples have 5 
        //stack size, we would find all slots with apples to use later, bc we
        //could have 2 slots of apples that both arent filled.
        slotToGo = collectionOfSlots.Where(slotBeingLookedAt => slotBeingLookedAt.ItemData ==itemToAdd).ToList();
        //the list.where apparently is really helpful. Researching in progress.
        if(slotToGo.Count > 0) //change to 1 if broken
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
