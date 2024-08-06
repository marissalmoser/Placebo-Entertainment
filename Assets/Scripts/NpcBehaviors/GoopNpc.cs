/******************************************************************
*    Author: Elijah Vroman
*    Contributors: 
*    Date Created: 6/25/24
*    Description: NPC class containing logic for the Goop NPC.
*******************************************************************/

public class GoopNpc : BaseNpc
{
    public override void CheckForStateChange()
    {
        if (_currentState == NpcStates.DefaultIdle)
        {
            EnterMinigameReady();
        }
    }

    protected override int ChooseDialoguePath(PlayerResponse option)
    {
        if(_hasBypassItem)
        {
            return option.NextResponseIndex[1];
        }
        else
        {
            return base.ChooseDialoguePath(option);
        }
    }
}
