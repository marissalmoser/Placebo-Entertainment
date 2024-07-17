/*****************************************************************************
// File Name :         WrenchBehavior.cs
// Author :            Mark Hanson
// Contributors :      Marissa Moser, Nick Grinstead
// Creation Date :     5/27/2024
//
// Brief Description : Any function to do with the wrench will be found here. Wrench swinging, spark interaction, and completion of this segment of the minigame.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using PlaceboEntertainment.UI;
using System;

public class WrenchBehavior : MonoBehaviour, IInteractable
{
    /// <summary>
    /// Holds an items description and the "leave" player option for it
    /// </summary>
    [System.Serializable]
    protected struct DescriptionNode
    {
        [SerializeField] private string _description;
        [SerializeField] private string _exitResponse;
        [SerializeField] private NpcEvent _eventToTrigger;
        [SerializeField] private NpcEventTags _eventTag;

        public string Description { get => _description; }
        public string ExitResponse { get => _exitResponse; }
        public NpcEvent EventToTrigger { get => _eventToTrigger; }
        public NpcEventTags EventTag { get => _eventTag; }
    }

    [SerializeField] private NpcEvent _minigameEndEvent;

    [Header("UI Stuff")]
    [SerializeField] private TextMeshPro _smackedText;
    [SerializeField] private DescriptionNode _itemDescription;
    [SerializeField] private string _interactPromptText = "WRENCH";

    [Header("Wrench overall functions")]
    [SerializeField] private GameObject _sparksMode;
    //private PlayerController _pc;
    private int _sparkSmacked;

    [Header("Wrench within hand functions")]
    [SerializeField] private Animator _animate;
    [SerializeField] private GameObject _wrenchSpark;
    //private bool _swing;
    [Header("Wrench outside hand functions")]
    [SerializeField] private GameObject _rightHand;
    private bool _withinProx;
    private bool _isEquipped;

    public static Action SparkSmackedAction;

    private TabbedMenu _tabbedMenu;
    private PlayerController _playerController;
    private Interact _playerInteractBehavior;

    void Awake()
    {
        _rightHand = GameObject.FindWithTag("Righty");
        _sparksMode = GameObject.Find("SparksMode");
        GameObject _smackTextObject = GameObject.Find("Spark num");
        _smackedText = _smackTextObject.GetComponent<TextMeshPro>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _tabbedMenu = TabbedMenu.Instance;
        _playerController = PlayerController.Instance;
        _playerInteractBehavior = _playerController.GetComponent<Interact>();
        _isEquipped = false;
        SparkSmackedAction += SparkSmacked;
    }

    void FixedUpdate()
    {
        if(_isEquipped == true)
        {
            transform.position = new Vector3(_rightHand.transform.position.x, _rightHand.transform.position.y, _rightHand.transform.position.z);
            transform.rotation = _rightHand.transform.rotation;
        }
    }

    /// <summary>
    /// A coroutine to manage the swinging animation.
    /// </summary>
    /// <returns></returns>
    IEnumerator Swinging()
    {
        //print("swing");
        GetComponent<Collider>().enabled = true;
        _animate.SetBool("isSwinging", true);
        //_swing = true;
        yield return new WaitForSeconds(0.1f);
        //_swing = false;
        GetComponent<Collider>().enabled = false;
        _animate.SetBool("isSwinging", false);
    }

    /// <summary>
    /// This function is invoked in SparkInteractBehavior whenever a spark is interacted
    /// with. It keeps track of the number of sparks that have been smacked and ends the game.
    /// </summary>
    private void SparkSmacked()
    {
        _sparkSmacked++;
        _smackedText.text = _sparkSmacked.ToString();
        StartCoroutine(Swinging());
        if (_sparkSmacked >= 5)
        {
            _smackedText.color = Color.green;
            _sparksMode.SetActive(false);
            gameObject.SetActive(false);

            //game ends here?
            _minigameEndEvent.TriggerEvent(NpcEventTags.Coward);
            print("game end");
        }
    }

    /// <summary>
    /// Invoked by dialogue button to stop showing the item's descriptiond
    /// </summary>
    public void CloseItemDescription()
    {
        _tabbedMenu.ToggleDialogue(false);
        _playerController.LockCharacter(false);
        _playerInteractBehavior.StartDetectingInteractions();

        if (_itemDescription.EventToTrigger != null)
        {
            _itemDescription.EventToTrigger.TriggerEvent(_itemDescription.EventTag);
        }
    }

    /// <summary>
    /// This function is called when the player interacts with the wrench.
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        _playerController.LockCharacter(true);
        _playerInteractBehavior.StopDetectingInteractions();
        _tabbedMenu.DisplayDialogue("", _itemDescription.Description);
        _tabbedMenu.ToggleDialogue(true);
        _tabbedMenu.ClearDialogueOptions();
        _tabbedMenu.DisplayDialogueOption(_itemDescription.ExitResponse, click: () => { CloseItemDescription(); });

        if (_isEquipped == false)
        {
            _animate.SetTrigger("pickedUp");
            _isEquipped = true;
            GetComponent<Collider>().enabled = false;
            transform.position = _rightHand.transform.position;
            transform.rotation = _rightHand.transform.rotation;
            transform.parent = _rightHand.transform;
        }
    }

    /// <summary>
    /// Shows UI prompt for wrench
    /// </summary>
    public void DisplayInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(true, _interactPromptText);
    }

    /// <summary>
    /// Hides UI prompt for wrench
    /// </summary>
    public void HideInteractUI()
    {
        TabbedMenu.Instance.ToggleInteractPrompt(false);
    }

    private void OnDisable()
    {
        SparkSmackedAction -= SparkSmacked;
    }
}
