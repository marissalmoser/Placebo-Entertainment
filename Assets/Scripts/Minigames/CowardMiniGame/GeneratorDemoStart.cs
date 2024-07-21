using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorDemoStart : MonoBehaviour
{
    [SerializeField] private GameObject _manager;

    // Start is called before the first frame update
    void Start()
    {
        _manager.GetComponent<RipcordBehavior>().ActivateMinigame();
    }
}
