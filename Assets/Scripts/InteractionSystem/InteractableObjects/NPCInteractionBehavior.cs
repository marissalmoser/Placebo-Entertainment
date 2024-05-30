using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class NPCInteractionBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] private string _npcName;
    public void Interact(GameObject player)
    {
        GetComponent<BaseNpc>().Interact();
    }

    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, _npcName);
    }

    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

}
