/*****************************************************************************
// File Name :         MGWire.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     05/21/24
//
// Brief Description : Controls the wire logic
*****************************************************************************/

using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class MGWire : MonoBehaviour
{
    public EWireNum WireNum;
    [SerializeField] private Color _wireColor;
    [SerializeField] private Color _interactColor;

    [SerializeField] float _sphereScale;

    /// <summary>
    /// Whether the wire has entered the slot zone
    /// </summary>
    private bool _canSlot = false;

    private MGWireSlot _currentSlot = null;
    private MGWireMovement _wireMovement;
    private bool _correctlySlotted = false;

    public enum EWireNum
    {
        ONE, TWO, THREE
    }

    private void Start()
    {
        if (TryGetComponent<MGWireMovement>(out MGWireMovement wireMove))
        {
            _wireMovement = wireMove;
        }
    }

    private void OnInteract()
    {
        // TODO: When the player interacts with a wire, make the end
        // kinematic and have it follow the players direction
        _wireMovement.ChangeEndKinematic(true);
    }

    private void OnDrop()
    {
        // TODO: If the wire is in a slot, call PlaceWire()
        // Otherwise, make the wire non-kinematic
        _wireMovement.ChangeEndKinematic(true);

        PlaceWire();
    }

    public void EndTriggerEnter(MGWireSlot slot)
    {
        _canSlot = true;
        _currentSlot = slot;

        // TODO: This is not a good spot to call PlaceWire(); Find a better
        // solution where the placewire function properly "slots" the wire
        OnDrop();
    }

    public void EndTriggerExit()
    {
        _canSlot = false;
        _currentSlot = null;
    }

    /// <summary>
    /// What happens when the player puts the wire in a spot?
    /// </summary>
    private void PlaceWire()
    {
        if (_canSlot && _currentSlot && !_correctlySlotted)
        {
            _correctlySlotted = true;
            _currentSlot.CheckWire(this);
        }
    }

    public void CreateSegmentSphere(Transform parentObj, bool isEndSegment = false)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        Destroy(sphere.GetComponent<Collider>());

        sphere.transform.parent = parentObj;
        sphere.transform.position = parentObj.position;
        sphere.transform.localScale = new Vector3(1f, 1f, 1f);

        sphere.transform.localScale *= _sphereScale;

        SetWireColor(sphere.GetComponent<MeshRenderer>().material, isEndSegment);
    }

    private void SetWireColor(Material mat, bool isEndSegment)
    {
        if(!isEndSegment)
        {
            switch (WireNum)
            {
                case EWireNum.ONE:
                    mat.color = _wireColor;
                    break;
                case EWireNum.TWO:
                    mat.color = _wireColor;
                    break;
                case EWireNum.THREE:
                    mat.color = _wireColor;
                    break;
            }
        }
        else
        {
            mat.color = _interactColor;
        }
        
    }
}