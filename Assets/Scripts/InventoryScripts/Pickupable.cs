/******************************************************************
*    Author: Elijah Vroman
*    Contributors: Elijah Vroman, Nick Grinstead
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
    /// <summary>
    /// Holds an items description and the "leave" player option for it
    /// </summary>
    [System.Serializable]
    protected struct DescriptionNode
    {
        [SerializeField] private string _description;
        [SerializeField] private string _exitResponse;

        public string Description { get => _description; }
        public string ExitResponse { get => _exitResponse; }
    }

    [SerializeField] private DescriptionNode _itemDescription;
    
    [SerializeField] private float PickUpRadius;
    [SerializeField] private InventoryItemData myData;
    private SphereCollider myCollider;
    private TabbedMenu _tabbedMenu;
    private PlayerController _playerController;
    private Interact _playerInteractBehavior;

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

    private void Start()
    {
        _tabbedMenu = TabbedMenu.Instance;
        _playerController = PlayerController.Instance;
        _playerInteractBehavior = _playerController.GetComponent<Interact>();
    }

    public void DisplayInteractUI()
    {
        _tabbedMenu.ToggleInteractPrompt(true, "The " + myData.DisplayName);
    }

    public void HideInteractUI()
    {
        _tabbedMenu.ToggleInteractPrompt(false);
    }

    public void CloseItemDescription()
    {
        _tabbedMenu.ToggleDialogue(false);
        _playerController.LockCharacter(false);
        _playerInteractBehavior.StartDetectingInteractions();
    }

    public void Interact(GameObject player)
    {
        _playerController.LockCharacter(true);
        _playerInteractBehavior.StopDetectingInteractions();
        _tabbedMenu.DisplayDialogue("", _itemDescription.Description);
        _tabbedMenu.ToggleDialogue(true);
        _tabbedMenu.ClearDialogueOptions();
        _tabbedMenu.DisplayDialogueOption(_itemDescription.ExitResponse, click: () => { CloseItemDescription(); });

        InventoryHolder inventoryHolder = player.GetComponent<InventoryHolder>();

        //if it is not already in inventory
        if (inventoryHolder != null && !inventoryHolder.InventorySystem.ContainsItem(myData, out _))
        {
            //Debug.Log("Got item");
            inventoryHolder.InventorySystem.AddToInventory(myData, 1, out _);
            Destroy(gameObject);
        }
        //if it is already in inventory (prevents doubles in inventory)
        else if(inventoryHolder != null)
        {
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
