using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Door_script : MonoBehaviour 
{
	public AudioClip audioClip;
	public Animator animator;
	public string onTriggerEnterParameterName;
		public string onTriggerExitParameterName;	

	void Start()
	{
		if (animator == null) {
			animator = GetComponent<Animator> ();
			if(animator == null)
			{
				Debug.LogError("No animator component on this script!",gameObject);
			}
		}
    }
	
	void OnTriggerEnter()
	{
		if(onTriggerEnterParameterName != null)
		{
		    animator.SetTrigger(onTriggerEnterParameterName);
			GetComponent<AudioSource>().PlayOneShot(audioClip);
		}
	}

	void OnTriggerExit()
	{
		if(onTriggerExitParameterName != null)
		{
		   animator.SetTrigger(onTriggerExitParameterName);
		   GetComponent<AudioSource>().PlayOneShot(audioClip);
		}
	}
}
