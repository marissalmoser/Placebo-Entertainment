using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLever : LeverInteraction
{
    [SerializeField] private StationBehavior _station;

    /// <summary>
    /// Defaults lever to up.
    /// </summary>
    private void Awake()
    {
        SetLever(true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    public override void SetLever(bool input)
    {
        base.SetLever(input);

        if (!input)
        {
            _station.InvokeClearEvent();
            Invoke(nameof(SetLeverDelayHelper), 0.2f);
        }
    }

    /// <summary>
    /// Invoked to reset lever on a delay.
    /// </summary>
    private void SetLeverDelayHelper()
    {
        SetLever(true);
    }
}
