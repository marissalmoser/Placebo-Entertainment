using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RipcordBehavior : MonoBehaviour
{
    [Header("Points of Movement")]
    private Vector3 _relocatePoint;
    [SerializeField] private float _maxReach;
    [SerializeField]private GameObject _targetFollow;
    [SerializeField] private float _speed;
    public bool PressedE;

    // Start is called before the first frame update
    void Start()
    {
        PressedE = false;
        _relocatePoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        if (PressedE == true && transform.position.z > _targetFollow.transform.position.z)
        {
            transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
        }
        if (PressedE == false && transform.position.z < _relocatePoint.z || PressedE == true && transform.position.z < _maxReach)
        {
            transform.position += new Vector3(0, 0, _speed * Time.deltaTime);
        }
    }
}
