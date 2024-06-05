/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/20/24
*    Description: This is a scriptable object. Not much here. More 
*       variables and options on Game Designer request, wanted to keep
*       it simple.
*******************************************************************/
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory System/Inventory Collectable")]
public class InventoryItemData : ScriptableObject
{
    public Sprite Icon;
    public int MaxStackSize;
    public string DisplayName;
    [TextArea(4, 4)] public string DisplayDescription;
}
