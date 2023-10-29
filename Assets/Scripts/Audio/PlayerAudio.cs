using UnityEngine;

public class PlayerAudio : AudioRef<PlayerAudio>
{
    public enum StepState
    {
        None = 0,
        Grass,
        STEP_02,
        STEP_03,
    }

    [Header("Step")]
    public AK.Wwise.Event inGame_STEP;
    public AK.Wwise.Event inGame_STEP_Grass;
    public AK.Wwise.Event inGame_STEP_02;
    public AK.Wwise.Event inGame_STEP_03;

    [Header("Jump")]
    public AK.Wwise.Event inGame_JUMP_01;
    public AK.Wwise.Event inGame_JUMP_Land;

    [Header("Life")]
    public AK.Wwise.Event inGame_CH_Life;
    public AK.Wwise.Event inGame_CH_Die;

    [Header("Block")]
    public AK.Wwise.Event inGame_BLOCK_01;
    public AK.Wwise.Event inGame_WARN_01;

    private StepState state = StepState.None;

    public static void ChangeStepSound(StepState state)
    {
        if (state == Instance.state)
        {
            return;
        }

        Instance.state = state;

        if (state == StepState.None)
        {
            PlayerAudio.Stop(Instance.inGame_STEP);
            return;
        }

        switch (state)
        {
            case StepState.Grass:
                PlayerAudio.Post(Instance.inGame_STEP_Grass, nameof(Instance.inGame_STEP_Grass));
                break;

            case StepState.STEP_02:
                PlayerAudio.Post(Instance.inGame_STEP_02, nameof(Instance.inGame_STEP_02));
                break;

            case StepState.STEP_03:
                PlayerAudio.Post(Instance.inGame_STEP_03, nameof(Instance.inGame_STEP_03));
                break;
        }

        PlayerAudio.Post(Instance.inGame_STEP);
    }
}
