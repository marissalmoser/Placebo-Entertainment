/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 19, 2024
*    Description: This is the script for the first station's screen. It sets the screen's
*    levers to their true or false state depending on the random order.
*******************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screen1 : ScreenBehavior
{
    [SerializeField] private GameObject _miniManager;
    [SerializeField] private List<GameObject> _leverImages;

    [SerializeField] private Sprite _leverFalseImage;
    [SerializeField] private Sprite _leverTrueImage;


    public override void SetOrderToRandom()
    {
        SetOrderToDefault();
        for (int i = 0; i < _leverImages.Count; i++)
        {
            if (_screenObjsOrder[i] == 1)
            {
                _leverImages[i].GetComponent<Image>().sprite = _leverTrueImage;
            }
        }
    }

    public override void SetOrderToDefault()
    {
        for (int i = 0; i < _leverImages.Count; i++)
        {
            _leverImages[i].GetComponent<Image>().sprite = _leverFalseImage;
        }
    }

}
