using UnityEngine;

public class TriggerEnter2DTag : AkTriggerBase
{
	public string TriggerTag;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (triggerDelegate != null && collision.CompareTag(TriggerTag))
			triggerDelegate(collision.gameObject);
	}
}
