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


    public PlayerInteractSystem()
    {
        ObjectName = "Default None";
    }
    public PlayerInteractSystem(string _newDetection)
    {
        ObjectName = _newDetection;
    }
    public void BackToDefault()
    {
        ObjectName = "Default None";
    }
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
            default:
                break;
            
        }
    }
}
