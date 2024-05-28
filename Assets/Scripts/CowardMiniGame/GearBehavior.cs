/*****************************************************************************
// File Name :         GearBehavior.cs
// Author :            Mark Hanson
// Creation Date :     5/24/2024
//
// Brief Description : Any function to do for the gears mini game will be found here. Includes swapping slots, Correct slot pattern with all bad ones, and selecting gears for each slot.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBehavior : MonoBehaviour
{
    [Header("Individual Gear")]
    [SerializeField] private GameObject[] _gearSize;
    [SerializeField] private GameObject _gearIndi;
    private int _gearSizeNum;
    private bool _scrollable;
    private bool _doOnce;
    private PlayerController _pc;

    [Header("Correct Gear")]
    [SerializeField] private int _rightGearNum;
    [SerializeField] private Color _matRed;
    [SerializeField] private Color _matGreen;
    private Renderer _rndr;

    // Start is called before the first frame update
    void Start()
    {
        _doOnce = true;
        _gearSizeNum = 1;
        _scrollable = false;
        GameObject _pcObject = GameObject.FindWithTag("Player");
        _pc = _pcObject.GetComponent<PlayerController>();
        _rndr = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale = new Vector3(1f, _gearIndi.transform.localScale.y + 0.5f, 1f);
        if (_pc.interact.IsPressed() && _gearSizeNum != _gearSize.Length && _doOnce == true && _scrollable == true)
        {
            _gearSizeNum++;
            _doOnce = false;
            StartCoroutine(doOnceCooldown());
        }
        if(_pc.interact.IsPressed() && _gearSizeNum == _gearSize.Length && _doOnce == true && _scrollable == true)
        {
            _gearSizeNum = 1;
            _doOnce = false;
            StartCoroutine(doOnceCooldown());
        }
        _gearIndi = _gearSize[_gearSizeNum - 1];
        //Debug.Log(_gearSizeNum);
    }
    void Update()
    {
        if (_gearSizeNum == _rightGearNum)
        {
            _rndr.material.color = Color.green;
            Destroy(this);
        }
        if (_gearSizeNum < _rightGearNum|| _gearSizeNum > _rightGearNum)
        {
            _rndr.material.color = Color.red;
        }
    }
    IEnumerator doOnceCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        _doOnce = true;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            _scrollable = true;
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            _scrollable = false;
        }
    }
}
