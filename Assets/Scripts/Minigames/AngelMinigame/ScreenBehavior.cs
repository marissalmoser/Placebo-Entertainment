/******************************************************************
*    Author: Marissa Moser
*    Contributors: 
*    Date Created: June 19, 2024
*    Description: This is the base class for screens. It will have one child per station.
*******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBehavior : MonoBehaviour
{
    [SerializeField] protected List<int> _screenObjsOrder;

    /// <summary>
    /// This function is called from the minigame manager, to set the list variable
    /// to the random order. It then sets the screen to the random order.
    /// </summary>
    /// <param name="target"></param>
    public virtual void SetRandomOrderList(List<int> target)
    {
        _screenObjsOrder = target;
        SetOrderToRandom();
    }

    /// <summary>
    /// This function is to be overriden in child classes. It is used to alter the 
    /// screen of each station's layout and set the display to the random order the 
    /// lpayer is trying to match.
    /// </summary>
    public virtual void SetOrderToRandom()
    {
    }

    /// <summary>
    /// This function is to be overriden in child classes. It is used to change the
    /// screen's display back to it's default order. Not every station may need this.
    /// </summary>
    public virtual void SetOrderToDefault()
    {
    }
}
