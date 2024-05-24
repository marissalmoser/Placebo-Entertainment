/*****************************************************************************
// File Name :         PlayerInteractSystem.cs
// Author :            Mark Hanson
// Creation Date :     5/22/2024
//
// Brief Description : Class used for detecting interactable objects 
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractSystem
{
    public string ObjectName;

    /// <summary>
    /// When adding no variable within a new player interaction goes to the default which is none
    /// </summary>
    public PlayerInteractSystem()
    {
        ObjectName = "Default None";
    }
    /// <summary>
    /// A function used to switch out the object name used for when its actually detecting a new object
    /// </summary>
    /// <param name="_newDetection"></param>
    public PlayerInteractSystem(string _newDetection)
    {
        ObjectName = _newDetection;
    }
    /// <summary>
    /// A function to switch back to default detection which is none
    /// </summary>
    public void BackToDefault()
    {
        ObjectName = "Default None";
    }
    /// <summary>
    /// Whenever pressing E it runs through the list of different interactable objects and which ones zone its in 
    /// </summary>
    public void CallInteract()
    {
        switch (ObjectName)
        {
            case "Test":
                    Debug.Log("E has been pressed");
                break;
            case "Default None":
                Debug.Log("No action was done");
                break;
            case "Ripcord":
                GameObject RcObject = GameObject.Find("Ripcord");
                RipcordBehavior _rc = RcObject.GetComponent<RipcordBehavior>();
                _rc.PressedE = true;
                break;
            default:
                break;
            
        }
    }
}
