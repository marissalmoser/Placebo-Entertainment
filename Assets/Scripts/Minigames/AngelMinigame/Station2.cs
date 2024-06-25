using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station2 : StationBehavior
{
    [SerializeField] private DialInteraction[] _dials;
    private List<int> _targetDialState = new List<int>();

    private const int _sequenceLength = 4;

    #region ActionRegistering
    private void OnEnable()
    {
        AngelMinigameManager.TriggerStart += RestartStationState;
        AngelMinigameManager.TriggerFail += RestartStationState;
    }

    private void OnDisable()
    {
        AngelMinigameManager.TriggerStart -= RestartStationState;
        AngelMinigameManager.TriggerFail -= RestartStationState;
    }
    #endregion

    /// <summary>
    /// Checks if the dials match the target sequence.
    /// </summary>
    /// <returns>True if all dials matched</returns>
    public override bool CheckStates()
    {
        for (int i = 0; i < _sequenceLength && i < _dials.Length && i < _targetDialState.Count; i++)
        {
            if ((int) _dials[i]._direction != _targetDialState[i])
            {
                RestartStationState();
                return false;
            }
        }

        RestartStationState();
        return true;
    }

    /// <summary>
    /// Resets all dials to their up position.
    /// </summary>
    public override void RestartStationState()
    {
        foreach (DialInteraction dial in _dials)
        {
            dial.ResetDial();
        }
    }

    /// <summary>
    /// Generates a random sequence of dial positions.
    /// </summary>
    /// <returns>The sequence of dial positions as ints</returns>
    public override List<int> SetRandomToMatch()
    {
        _targetDialState.Clear();

        for (int i = 0; i < _sequenceLength; i++)
        {
            int val = UnityEngine.Random.Range(0, 4);

            _targetDialState.Add(val);
        }

        return _targetDialState;
    }
}
