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

    public InventorySystem InventorySystem => inventorySystem;
    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);     
    }
    public void SetInventorySystem(InventorySystem system)
    {
        inventorySystem = system;
    }
}
