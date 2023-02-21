using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
	#region Singleton
	public static SceneFader instance;

	private void Awake()
	{
		if (instance != null)
			Destroy(gameObject);
		else
		{
			instance = this;
		}
	}
	#endregion

	[SerializeField] private Animator animator;

	public void FadeOut()
	{
		animator.SetBool("Fade", true);
	}

	public void FadeIn()
	{
		animator.SetBool("Fade", false);
	}
}
