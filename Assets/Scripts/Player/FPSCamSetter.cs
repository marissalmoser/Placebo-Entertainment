/*****************************************************************************
// File Name :         FPSCamSetter.cs
// Author :            Nick Grinsteasd
// Creation Date :     
//
// Brief Description : Sets the follow and look at variables on the Cinemachine
                       camera so we don't have to set them in scene.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FPSCamSetter : MonoBehaviour
{
    private void Awake()
    {
        CinemachineVirtualCamera temp = GetComponent<CinemachineVirtualCamera>();
        GameObject look = GameObject.Find("Look");

        temp.Follow = look.transform;
        temp.LookAt = look.transform;
    }
}
