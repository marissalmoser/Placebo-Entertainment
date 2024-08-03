/*****************************************************************************
// File Name :         FireManager.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     08/03/2024
//
// Brief Description : Destroys fire objects
*****************************************************************************/

using PlaceboEntertainment.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    [SerializeField] private NpcEvent _minigameEndEvent;

    [SerializeField] private GameObject _fireAlarmLight;

    private List<FireBehavior> _fires;

    private TabbedMenu _tabbedMenu;

    private void Start()
    {
        _tabbedMenu = TabbedMenu.Instance;
    }

    public void StartMinigame()
    {
        _fires = FindObjectsOfType<FireBehavior>().ToList();
        FindObjectOfType<FishNpc>().gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        FireBehavior.OnFireExtinguished += OnFireDestroyed;
    }

    private void OnDisable()
    {
        FireBehavior.OnFireExtinguished -= OnFireDestroyed;
    }

    private void OnFireDestroyed(FireBehavior fire)
    {
        _fires.Remove(fire);

        Destroy(fire.gameObject);

        // MINIGAME ENDS HERE
        if (_fires.Count <= 0)
        {
            _tabbedMenu.ToggleWaterMeter(false);

            EndMinigame();

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Ends the minigame
    /// </summary>
    private void EndMinigame()
    {
        _minigameEndEvent.TriggerEvent(NpcEventTags.Fish);

        FindObjectOfType<FishNpc>().gameObject.SetActive(true);
        _fireAlarmLight.SetActive(false);

        Destroy(gameObject);
    }
}
