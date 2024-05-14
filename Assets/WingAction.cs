using UnityEngine;

public class WingAction : MonoBehaviour
{
    public void OnWingAction()
    {
        InGameAudio.Post(InGameAudio.Instance.ITEM_Wing_02);
    }
}