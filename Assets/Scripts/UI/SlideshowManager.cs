/******************************************************************
 *    Author: Nick Grinstead
 *    Contributors: 
 *    Date Created: 7/9/2024
 *    Description: A manager script for the slideshow player. Has functions that
 *                 play different videos when called.
 *******************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SlideshowManager : MonoBehaviour
{
    private PlayerControls _playerControls;
    private InputAction _playPause;
    private UIDocument _slideshowUI;
    private VideoPlayer _slideshowPlayer;

    [SerializeField] private bool _isIntroVideoPlayer = false;

    [SerializeField] private int _levelSceneBuildIndex;
    [SerializeField] private int _mainMenuBuildIndex;

    [SerializeField] private VideoClip _introVideo;
    [SerializeField] private VideoClip[] _endingVideos;

    /// <summary>
    /// Setting references and pause inputs. Also plays intro slide show if needed.
    /// </summary>
    private void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.BasicControls.Enable();
        _playPause = _playerControls.FindAction("PlayPause");
        _playPause.performed += ctx => TogglePlayPause();
        _playPause.Enable();

        _slideshowUI = GetComponent<UIDocument>();
        _slideshowPlayer = GetComponent<VideoPlayer>();

        _slideshowPlayer.loopPointReached += DonePlaying;
        _slideshowPlayer.prepareCompleted += PlayVideo;

        if (_isIntroVideoPlayer)
        {
            PlayIntroSlideshow();
        }
        else
        {
            _slideshowUI.rootVisualElement.style.display = DisplayStyle.None;
        }
    }

    private void OnDisable()
    {
        _slideshowPlayer.loopPointReached -= DonePlaying;
        _slideshowPlayer.prepareCompleted -= PlayVideo;
    }

    ~SlideshowManager()
    {
        _slideshowPlayer.loopPointReached -= DonePlaying;
        _slideshowPlayer.prepareCompleted -= PlayVideo;

        _playPause.Disable();
    }

    /// <summary>
    /// Invoked when slideshow is over
    /// </summary>
    private void DonePlaying(VideoPlayer vp)
    {
        _slideshowPlayer.Stop();

        if (_isIntroVideoPlayer)
        {
            SceneManager.LoadScene(_levelSceneBuildIndex);
        }
        else
        {
            SceneManager.LoadScene(_mainMenuBuildIndex);
        }
    }

    /// <summary>
    /// Invoked once the video content has been prepared
    /// </summary>
    private void PlayVideo(VideoPlayer vp)
    {
        _slideshowUI.rootVisualElement.style.display = DisplayStyle.Flex;
        _slideshowPlayer.Play();
    }

    /// <summary>
    /// Plays intro slideshow
    /// </summary>
    public void PlayIntroSlideshow()
    {
        _slideshowPlayer.clip = _introVideo;
        _slideshowPlayer.Prepare();
    }

    /// <summary>
    /// Plays an ending slideshow corresponding to the given index
    /// </summary>
    /// <param name="videoIndex">Index of video to play</param>
    public void PlayEndingSlideshow(int videoIndex = 0)
    {
        if (videoIndex < _endingVideos.Length && videoIndex >= 0)
        {
            _slideshowPlayer.clip = _endingVideos[videoIndex];
            _slideshowPlayer.Prepare();
        }
        else
        {
            Debug.LogError("Ending video index " + videoIndex + " is out of bounds", gameObject);
        }
    }

    /// <summary>
    /// Called when play/pause input is given to toggle if video is playing
    /// </summary>
    public void TogglePlayPause()
    {
        if (_slideshowPlayer != null)
        {
            if (_slideshowPlayer.isPlaying)
            {
                _slideshowPlayer.Pause();
            }
            else
            {
                _slideshowPlayer.Play();
            }
        }
    }
}
