using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleNpc : BaseNpc
{
    private void Awake()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();

        // Other stuff unique to this NPC goes here
    }

    public override void CheckPrerequisite()
    {
        throw new System.NotImplementedException();
    }

    protected override void EnterIdle()
    {
        base.EnterIdle();
    }

    protected override void EnterMinigameReady()
    {
        base.EnterMinigameReady();
    }

    protected override void EnterPlayingMinigame()
    {
        base.EnterPlayingMinigame();
    }

    protected override void EnterPostMinigame()
    {
        base.EnterPostMinigame();
    }

    protected override void EnterFailure()
    {
        base.EnterFailure();
    }

}
