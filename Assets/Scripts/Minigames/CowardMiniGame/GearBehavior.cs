/*****************************************************************************
// File Name :         GearBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser
// Creation Date :     5/24/2024
//
// Brief Description : Any function to do for the gears mini game will be found here. Includes swapping slots, Correct slot pattern with all bad ones, and selecting gears for each slot.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlaceboEntertainment.UI;

public class GearBehavior : MonoBehaviour, IInteractable
{
    [SerializeField] private string _interactPromptText = "GEAR";

    [Header("Individual Gear")]
    [SerializeField] private GameObject[] _gearSize;
    [SerializeField] private GameObject _gearIndi;
    private int _gearSizeNum;
    //private bool _scrollable;
    private bool _doOnce;
    //private PlayerController _pc;
    private bool _interact;

    [Header("Correct Gear")]
    [SerializeField] private int _rightGearNum;
    [SerializeField] private Color _matRed;
    [SerializeField] private Color _matGreen;
    private Renderer _rndr;

    // Start is called before the first frame update
    void Start()
    {
        _doOnce = true;
        _gearSizeNum = 1;
        //_scrollable = false;
        //GameObject _pcObject = GameObject.FindWithTag("Player");
        //_pc = _pcObject.GetComponent<PlayerController>();
        _rndr = this.GetComponent<Renderer>();
    }

    /// <summary>
    /// used for gradually cycling through gear sizes through the list of gears
    /// </summary>
    void FixedUpdate()
    {
        transform.localScale = new Vector3(1f, _gearIndi.transform.localScale.y + 0.5f, 1f);
        if (_interact && _gearSizeNum != _gearSize.Length && _doOnce == true)// && _scrollable == true)
        {
            _gearSizeNum++;
            _doOnce = false;
            StartCoroutine(doOnceCooldown());
        }
        if (_interact && _gearSizeNum == _gearSize.Length && _doOnce == true)// && _scrollable == true)
        {
            _gearSizeNum = 1;
            _doOnce = false;
            StartCoroutine(doOnceCooldown());
        }
        _gearIndi = _gearSize[_gearSizeNum - 1];
        //Debug.Log(_gearSizeNum);
    }
    /// <summary>
    /// if right gear number then make green if not keep red
    /// </summary>
    void Update()
    {
        if (_gearSizeNum == _rightGearNum)
        {
            _rndr.material.color = Color.green;
            Destroy(this);
        }
        if (_gearSizeNum < _rightGearNum || _gearSizeNum > _rightGearNum)
        {
            _rndr.material.color = Color.red;
        }
    }
    /// <summary>
    /// Cool down for no spamming
    /// </summary>
    /// <returns></returns>
    IEnumerator doOnceCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        _doOnce = true;
    }
    /// <summary>
    /// Enables gears to be interacted with in update
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        _interact = true;
    }

    /// <summary>
    /// Disables gears to be interacted with in update
    /// </summary>
    public void CancelInteract()
    {
        _interact = false;
    }

    /// <summary>
    /// Shows UI prompt to interact with gears.
    /// </summary>
    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, _interactPromptText);
    }

    /// <summary>
    /// Hides UI prompt to interact with gears.
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }
}
