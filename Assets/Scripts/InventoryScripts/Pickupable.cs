/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/21/24
*    Description: PUT THIS ON ANY GAMEOBJECT TO BE INVENTORIED
*    If what this script is attached to is hit by an inventory 
*    system, it is added to that inventory.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Pickupable: MonoBehaviour
{
    [SerializeField] private float PickUpRadius;
    [SerializeField] private InventoryItemData myData;
    private SphereCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();  
        myCollider.isTrigger = true;

        if(myData == null)
        {
            Debug.Log("An item in the scene is missing a scriptable object");
        }
        if(PickUpRadius <= 0)
        {
            PickUpRadius = myCollider.radius;
        }
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.transform.GetComponent<InventoryHolder>();
        if (inventory.InventorySystem.AddToInventory(myData, 1, out _))
        {
            //IFF successful, destroy gameobject

            Destroy(gameObject);
        }
    }*/
}
