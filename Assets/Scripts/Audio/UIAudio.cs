using UnityEngine;

public class UIAudio : AudioRef<UIAudio>
{
    [Header("UI SFX")]
    public AK.Wwise.Event UI_in;
    public AK.Wwise.Event UI_out;
    public AK.Wwise.Event UI_select;
    public AK.Wwise.Event UI_pick;
}
