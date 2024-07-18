/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/20/24
*    Description: Inventory system is just what inventory holders use
*    for functionality
*******************************************************************/
using System;
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

    public event Action<InventoryItemData, int> AddedToInventory;
    public event Action<InventoryItemData, int> RemovedFromInventory;

    public InventorySystem(int size)
    {
        collectionOfSlots = new List<InventorySlot>(size);
        //Filling up the inventory system with slots
        for (int i = 0; i < size; i++)
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
    /// THe overflowAmount returns the amount of items not added if inventory 
    /// is absolutely full
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="amountToAdd"></param>
    /// <returns></returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd, out int overflowAmount)
    {
        bool addedAnyItems = false;
        overflowAmount = amountToAdd;

        while (overflowAmount > 0)
        {
            //if we have the item, just add to the stack
            if (ContainsItem(itemToAdd, out List<InventorySlot> slotItemIsIn))
            {
                foreach (var slot in slotItemIsIn)
                {
                    int roomLeft = slot.GetRoomLeftInStack();
                    if (roomLeft > 0)
                    {
                        if (overflowAmount <= roomLeft)
                        {
                            slot.AddToStackInThisSlot(overflowAmount);
                            AddedToInventory?.Invoke(itemToAdd, overflowAmount);
                            overflowAmount = 0;
                            addedAnyItems = true;
                            SaveLoadManager.Instance.SaveGameToSaveFile();
                            return true; // All items added
                        }
                        else
                        {
                            slot.AddToStackInThisSlot(roomLeft);
                            overflowAmount -= roomLeft;
                            AddedToInventory?.Invoke(itemToAdd, roomLeft);
                            addedAnyItems = true;
                            SaveLoadManager.Instance.SaveGameToSaveFile();
                        }
                    }
                }
            }
            //if all stacks with similar items are full, find a free slot
            if (HasFreeSlot(out InventorySlot freeSlot))
            {
                int maxStack = itemToAdd.MaxStackSize;
                if (overflowAmount <= maxStack)
                {
                    freeSlot.UpdateThisSlot(itemToAdd, overflowAmount);
                    AddedToInventory?.Invoke(itemToAdd, overflowAmount);
                    overflowAmount = 0;
                    addedAnyItems = true;
                    SaveLoadManager.Instance.SaveGameToSaveFile();
                    return true; // All items added
                }
                else
                {
                    freeSlot.UpdateThisSlot(itemToAdd, maxStack);
                    overflowAmount -= maxStack;
                    AddedToInventory?.Invoke(itemToAdd, maxStack);
                    addedAnyItems = true;
                    SaveLoadManager.Instance.SaveGameToSaveFile();
                }
            }
            else
            {
                // No more free slots available
                break;
            }
        }

        return addedAnyItems;
    }
    /// <summary>
    /// This is a bool also because it might be useful to check if the system
    /// has enough of an item compared to the int amountToRemove (like a store,
    /// shop, or checkpoint.) 
    /// Include a InventoryItemData, and how much of it to remove.
    /// Remove anyways bool (if true) allows a script to
    /// take all identical items to the toRemove irregardless of if the system 
    /// has enough. 
    /// Put "out _" if you want to ignore the returned data. 
    /// </summary>
    /// <param name="toRemove"></param> 
    /// <param name="amountToRemove"></param>
    /// <param name="RemoveAnyways"></param>
    /// <param name="dataRemoved"></param>
    /// <returns></returns>
    public bool RemoveFromInventory(InventoryItemData toRemove, int amountToRemove, bool RemoveAnyways, out InventoryItemData dataRemoved, out int amountRemoved)
    {
        amountRemoved = 0;
        if (ContainsItem(toRemove, out List<InventorySlot> slotsWithThisItem))
        {
            int totalAmountInInventory = slotsWithThisItem.Sum(slot => slot.StackSize);
            if (totalAmountInInventory < amountToRemove && !RemoveAnyways)
            {
                Debug.Log("Not enough items in inventory for required operation");
                dataRemoved = null;
                return false;
            }

            var sortedSlots = slotsWithThisItem.OrderBy(slot => slot.StackSize).ToList();
            int index = 0;

            while (amountToRemove > 0 && index < sortedSlots.Count)
            {
                InventorySlot slot = sortedSlots[index];
                int amountInSlot = slot.StackSize;
                if (amountToRemove >= amountInSlot)
                {
                    slot.RemoveFromStackInThisSlot(amountInSlot);
                    amountRemoved += amountInSlot;
                    amountToRemove -= amountInSlot;
                }
                else
                {
                    slot.RemoveFromStackInThisSlot(amountToRemove);
                    amountRemoved += amountToRemove;
                    amountToRemove = 0;
                }
                index++;
            }
            dataRemoved = toRemove;
            RemovedFromInventory?.Invoke(toRemove, amountRemoved);
            return true;
        }
        dataRemoved = null; //if we didnt remove anything, dataRemoved is null
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
        if (freeSlot != null)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// After passing in an inventoryItemData, you will recieve a list of all InventorySlots 
    /// that contain that item, full or not.
    /// </summary>
    /// <param name="itemToAdd"></param>
    /// <param name="slotToGo"></param>
    /// <returns></returns>
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> slotToGo)
    {
        //here, we look through our collection of slots (inventory or other) and gather
        //all the ones that match the item we picked up/want to compare (itemToAdd) and 
        //add them to a list. Ex. if we want to add an apple, and apples have 5 
        //stack size, we would find all slots with apples to use later, bc we
        //could have 2 slots of apples that both arent filled.
        slotToGo = collectionOfSlots.Where(slotBeingLookedAt => slotBeingLookedAt.ItemData == itemToAdd).ToList();
        //the list.where apparently is really helpful. Researching in progress.
        if (slotToGo.Count > 0) //change to 1 if broken
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
