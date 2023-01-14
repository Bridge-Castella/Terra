using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerCallback : MonoBehaviour
{
	public string tagFilter = null;
    [HideInInspector] public bool isTriggered = false;

    public delegate void CallbackT(Collider2D other);
    public CallbackT enter;
	public CallbackT exit;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (tagFilter != null)
		{
			if (!collision.CompareTag(tagFilter))
				return;
		}

		isTriggered = true;
		if (enter != null)
			enter.Invoke(collision);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (tagFilter != null)
		{
			if (!collision.CompareTag(tagFilter))
				return;
		}

		isTriggered = false;
		if (exit != null)
			exit.Invoke(collision);
	}
}
