/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman,
*    Date Created: 5/21/24
*    Description: PUT THIS ON ANY GAMEOBJECT TO BE INVENTORIED
*    If what this script is attached to is hit by an inventory 
*    system, it is added to that inventory.
*******************************************************************/
using PlaceboEntertainment.UI;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Pickupable : MonoBehaviour, IInteractable
{
    [SerializeField] private float PickUpRadius;
    [SerializeField] private InventoryItemData myData;
    private SphereCollider myCollider;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = false;
        //Changed above to false so that the player can run into the gameobject

        if (myData == null)
        {
            Debug.Log("An item in the scene is missing a scriptable object");
        }
        if (PickUpRadius <= 0)
        {
            PickUpRadius = myCollider.radius;
        }
    }

    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, "The " + myData.DisplayName);
    }

    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

    public void Interact(GameObject player)
    {
        InventoryHolder inventoryHolder = player.GetComponent<InventoryHolder>();

        if (inventoryHolder != null)
        {
            Debug.Log("Got item");
            inventoryHolder.InventorySystem.AddToInventory(myData, 1, out _);
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("InventoryHolder component not found on player.");
        }
    }
    /* COMMENTED THIS SO ITEMS MUST BE INTERACTED WITH. KEEPING JUST IN CASE
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
