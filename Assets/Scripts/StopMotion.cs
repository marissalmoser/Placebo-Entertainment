using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StopMotion : MonoBehaviour 
{
	[SerializeField] private int fps;
	private Animator _animator;
	private float _interval;
	private float _currentTime = 0.0F;
	private float _currentPlayback = 0.0F;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		_interval = 1.0F / fps;
	}

	private void Update()
	{
		if (_currentTime < _interval)
		{
			_currentTime += Time.deltaTime;
		}
		else
		{
			_currentPlayback += _currentTime;
			_currentTime = 0;
		}

		_animator.SetFloat(
			"Time", 
			(_currentPlayback % 1.0F)
		);
	}
}