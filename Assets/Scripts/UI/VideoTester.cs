/******************************************************************
 *    Author: Nick Grinstead
 *    Contributors: 
 *    Date Created: 7/10/2024
 *    Description: Temporary testing script to test out playing ending videos.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoTester : MonoBehaviour
{
    [SerializeField] SlideshowManager _slideshowManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            _slideshowManager.PlayEndingSlideshow(0);
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            _slideshowManager.PlayEndingSlideshow(1);
    }
}
