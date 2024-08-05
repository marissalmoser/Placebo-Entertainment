/******************************************************************
*    Author: Elijah Vroman
*    Contributors: 
*    Date Created: 6/25/24
*    Description: NPC class containing logic for the Goop NPC.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopNpc : BaseNpc
{
    [SerializeField] private InventoryItemData _targetBookItem;
    private bool _hasBook;
    private Animator _anim;

    protected override void Initialize()
    {
        base.Initialize();

        _anim = GetComponentInChildren<Animator>();
    }

    public override void CheckForStateChange()
    {
        if (_currentState == NpcStates.DefaultIdle)
        {
            EnterMinigameReady();
        }
    }
    protected override int ChooseDialoguePath(PlayerResponse option)
    {
        if (_hasBypassItem)
        {
            return option.NextResponseIndex[1];
        }
        else
        {
            if (option.NextResponseIndex.Length > 0)
            {
                return option.NextResponseIndex[0];
            }
            else
            {
                return 0;
            }
        }
    }

    protected override string ChooseDialogueFromNode(DialogueNode node)
    {
        PlayRandomTalkingAnim();
        return node.Dialogue[0];
    }

    public override void CollectedItem(InventoryItemData item, int quantity)
    {
        base.CollectedItem(item, quantity);

        if (item == _targetBookItem)
        {
            _hasBook = true;
            _hasBypassItem = true;
        }
    }

    private void PlayRandomTalkingAnim()
    {
        int rand = Random.Range(1, 4);
        switch (rand)
        {
            case 1:
                _anim.SetTrigger("Talking1");
                break;
            case 2:
                _anim.SetTrigger("Talking2");
                break;
            case 3:
                _anim.SetTrigger("Talking3");
                break;
        }
    }
}
