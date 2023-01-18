using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFadeArea : MonoBehaviour
{
	public FadeController[] controllers;
	public FadeSettings settings;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
			return;

		foreach (var controller in controllers)
			controller.customArea = this;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
			return;

		foreach (var controller in controllers)
			controller.customArea = null;
	}
}
