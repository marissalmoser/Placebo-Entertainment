/*****************************************************************************
// File Name :         GearCompletionCheck.cs
// Author :            Mark Hanson
// Contributors :      Nick Grinstead
// Creation Date :     5/27/2024
//
// Brief Description : A checker for when each gear is green then start the next phase of the mini game.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCompletionCheck : MonoBehaviour
{
    [Header("CheckList")]
    [SerializeField] private GameObject[] _gearSlots;
    private int _completedGears = 0;
    [SerializeField] private GameObject _wrench;
    [SerializeField] private GameObject _sparkMode;
    private GameObject _instantiatedWrench;

    [Header("VFX Stuff")]
    [SerializeField] private ParticleSystem _generatorSmoke;

    private void OnEnable()
    {
        GearBehavior.CorrectGear += CheckForGearCompletion;
    }

    private void OnDisable()
    {
        GearBehavior.CorrectGear -= CheckForGearCompletion;
    }

    /// <summary>
    /// Invoked when a gear is correctly slotted into place. Checks if all gears
    /// have been completed.
    /// </summary>
    private void CheckForGearCompletion()
    {
        _completedGears++;

        if (_completedGears >= _gearSlots.Length)
        {
            StartSparksSection();
        }
    }

    /// <summary>
    /// Starts the sparks section of the coward minigame
    /// </summary>
    public void StartSparksSection()
    {
        Vector3 _wrenchPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 2f);
        _sparkMode.SetActive(true);
        _instantiatedWrench = Instantiate(_wrench, _wrenchPoint, Quaternion.identity);
        _generatorSmoke.Stop();
        this.enabled = false;
    }

    /// <summary>
    /// Contains functionality that needs to happen when the game is started at the
    /// sparks section.
    /// </summary>
    public void StartWithBypass()
    {
        //moves the wrench to the players hand
        _instantiatedWrench.GetComponent<WrenchBehavior>().PickUpWrench();

        //Makes gears uninteractable and the right gear size
        foreach (GameObject gear in _gearSlots)
        {
            GearBehavior temp = gear.GetComponent<GearBehavior>();
            if (temp != null)
            {
                temp.SetGearToComplete();
            }
        }
    }
}
