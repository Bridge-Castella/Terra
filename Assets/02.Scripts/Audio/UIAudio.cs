using UnityEngine;

public class UIAudio : AudioRef<UIAudio>
{
    [Header("In Game UI SFX")]
    public AK.Wwise.Event inGame_UI_Hover;
    public AK.Wwise.Event inGame_UI_Open;
    public AK.Wwise.Event inGame_UI_Inventory_Open;
    public AK.Wwise.Event inGame_UI_Inventory_Hover;
    public AK.Wwise.Event inGame_UI_Inventory_Click;
    public AK.Wwise.Event inGame_UI_Quest_Open;
    public AK.Wwise.Event inGame_UI_Quest_Close;
    public AK.Wwise.Event inGame_UI_House_Open;
    public AK.Wwise.Event inGame_UI_House_Close;
    public AK.Wwise.Event inGame_UI_House_Inventory_Hover;
    public AK.Wwise.Event inGame_UI_House_Inventory_Click;
    [Header("Out Game UI SFX")]
    public AK.Wwise.Event UI_GameStart;
    public AK.Wwise.Event UI_Soundbar;
    public AK.Wwise.Event UI_optionOut;
    public AK.Wwise.Event UI_optionClick;
    [Header("Common UI SFX")]
    public AK.Wwise.Event UI_in;
    public AK.Wwise.Event UI_select;
    public AK.Wwise.Event UI_pick;
}
