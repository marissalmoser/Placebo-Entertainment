using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorDemoStart : MonoBehaviour
{
    [SerializeField] private GameObject _manager;
    [SerializeField] private GameObject _lightManager;

    // Start is called before the first frame update
    void Start()
    {
        _manager.GetComponent<RipcordBehavior>().ActivateMinigame();
    }
}
