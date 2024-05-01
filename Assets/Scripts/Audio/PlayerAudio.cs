using System.Collections;
using UnityEngine;

public class PlayerAudio : AudioRef<PlayerAudio>
{
    public enum StepState
    {
        None = 0,
        Grass,
        Rock
    }

    [Header("Step")]
    public AK.Wwise.Event inGame_STEP_Grass;
    public AK.Wwise.Event inGame_STEP_Rock;
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

    // private StepState state = StepState.None;
    // private bool shouldStopStepSound = false;
    // private bool isStepSoundPlaying = false;

    // public static void ChangeStepSound(StepState state)
    // {
    //     if (state == Instance.state)
    //     {
    //         return;
    //     }

    //     if (state == StepState.None)
    //     {
    //         Instance.state = state;
    //         Instance.shouldStopStepSound = true;
    //         return;
    //     }
    //     else
    //     {
    //         Instance.shouldStopStepSound = false;
    //     }

    //     switch (state)
    //     {
    //         case StepState.Grass:
    //             PlayerAudio.Post(Instance.inGame_STEP_Grass);
    //             break;

    //         case StepState.Rock:
    //             PlayerAudio.Post(Instance.inGame_STEP_Rock);
    //             break;

    //         // case StepState.STEP_03:
    //         //     PlayerAudio.Post(Instance.inGame_STEP_03, nameof(Instance.inGame_STEP_03));
    //         //     break;
    //     }

    //     if (Instance.state == StepState.None && !Instance.isStepSoundPlaying)
    //     {
    //         Instance.shouldStopStepSound = false;
    //         Instance.PostStepSound();
    //     }

    //     Instance.state = state;
    // }

    // private void StepSoundCallback(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    // {
    //     Instance.isStepSoundPlaying = false;
    //     if (!shouldStopStepSound)
    //     {
    //         Instance.PostStepSound();
    //     }
    // }

    // private void PostStepSound()
    // {
    //     Instance.isStepSoundPlaying = true;
    //     Instance.inGame_STEP.Post(
    //         Instance.soundObject,
    //         new AK.Wwise.CallbackFlags
    //         {
    //             value = (uint)AkCallbackType.AK_EndOfEvent
    //         },
    //         Instance.StepSoundCallback);
    // }
}
