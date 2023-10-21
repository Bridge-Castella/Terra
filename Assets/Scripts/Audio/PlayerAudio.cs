using UnityEngine;

public class PlayerAudio : AudioRef<PlayerAudio>
{
    public string StepAKStateStr;
    public string[] StepStatesStr = { "None", "Grass" };

    public enum StepState
    {
        None = 0,
        Grass,
    }

    // [Header("Step")]
    // public AK.Wwise.Event inGame_STEP_Grass;
    // public AK.Wwise.Event inGame_STEP_02;
    // public AK.Wwise.Event inGame_STEP_03;

    [Header("Jump")]
    public AK.Wwise.Event inGame_JUMP_01;
    public AK.Wwise.Event inGame_JUMP_Land;

    [Header("Life")]
    public AK.Wwise.Event inGame_CH_Life;
    public AK.Wwise.Event inGame_CH_Die;

    [Header("Block")]
    public AK.Wwise.Event inGame_BLOCK_01;
    public AK.Wwise.Event inGame_WARN_01;

    public static void ChangeStepSound(StepState state)
    {
        AkSoundEngine.SetState(Instance.StepAKStateStr,
            Instance.StepStatesStr[(int)state]);
    }
}
