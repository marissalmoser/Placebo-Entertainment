/*****************************************************************************
// File Name :         GearCompletionCheck.cs
// Author :            Mark Hanson
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
    [SerializeField] private GameObject[] _realGears;
    private bool _isGameComplete;
    private int _greenCount;
    [SerializeField] private Renderer[] _matCheck;
    [SerializeField] private GameObject _wrench;
    [SerializeField] private GameObject _sparkMode;

    [Header("VFX Stuff")]
    [SerializeField] private ParticleSystem _generatorSmoke;

    // Start is called before the first frame update
    void Start()
    {
        _isGameComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _realGears.Length; i++)
        {
           _matCheck[i] = _realGears[i].GetComponent<Renderer>();
        }
        for(int i = 0; i < _matCheck.Length; i++)
        {
            if (_matCheck[i].material.color == Color.green)
            {
                _matCheck[i] = null;
            }
        }
        if (_matCheck[0]== null && _matCheck[1] == null && _matCheck[2] == null && _matCheck[3] == null && _matCheck[4] == null && _matCheck[5].material.color == Color.red)
        {
            _isGameComplete = true;
            _matCheck[5].material.color = Color.green;
        }
        if (_isGameComplete)
        {
            Vector3 _wrenchPoint = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 2f);
            _sparkMode.SetActive(true);
            Instantiate(_wrench, _wrenchPoint, Quaternion.identity);
            _generatorSmoke.Stop();
            Destroy(this);
        }
    }
}
