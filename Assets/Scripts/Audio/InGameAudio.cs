using UnityEngine;

public class InGameAudio : AudioRef<InGameAudio>
{
    // [Header("BGM")]
    // public AK.Wwise.Event BGM_MAP1_loop;
    // public AK.Wwise.Event BGM_MAP2_loop;
    // public AK.Wwise.Event BGM_MAP3_loop;
    // public AK.Wwise.Event BGM_Terra_House_loop;

    [Header("Item")]
    public AK.Wwise.Event ITEM_Get_01;
    public AK.Wwise.Event ITEM_Fire_01;
    public AK.Wwise.Event ITEM_Fire_02;
    public AK.Wwise.Event ITEM_Portion;
    public AK.Wwise.Event ITEM_Plant;
    public AK.Wwise.Event ITEM_Wing_01;
    public AK.Wwise.Event ITEM_Wing_02;

    [Header("Quest")]
    public AK.Wwise.Event inGame_Quest_get;

    [Header("Monster")]
    public AK.Wwise.Event inGame_Monster_grass;
    public AK.Wwise.Event inGame_Monster_mush;
    public AK.Wwise.Event inGame_Monster_butterfly;
    public AK.Wwise.Event inGame_Monster_death;

    [Header("Stage")]
    public AK.Wwise.Event inGame_STAGE_CLEAR;
    public AK.Wwise.Event inGame_STAGE_CheckPoint;

    [Header("UI")]
    public AK.Wwise.Event inGame_UI_01;
    public AK.Wwise.Event inGame_UI_02;
    public AK.Wwise.Event inGame_Caution;

    [Header("NPC")]
    public AK.Wwise.Event inGame_NPC_Rato;
    public AK.Wwise.Event inGame_NPC_Riche;
    public AK.Wwise.Event inGame_NPC_Quest;
    public AK.Wwise.Event inGame_NPC_succ;

    [Header("Home")]
    public AK.Wwise.Event House_SelectPicture;
    public AK.Wwise.Event House_OpenCloset;
    public AK.Wwise.Event House_CloseCloset;
    public AK.Wwise.Event House_OpenBox;
    public AK.Wwise.Event House_CloseBox;
}