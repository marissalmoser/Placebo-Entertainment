using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipcordBehavior : MonoBehaviour
{
    [Header("Points")]
    [SerializeField] private GameObject _relocatePoint;
    [SerializeField] private GameObject _targetFollow;
    public bool PressedE;

    // Start is called before the first frame update
    void Start()
    {
        PressedE = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PressedE == true)
        {

        }
    }
}
