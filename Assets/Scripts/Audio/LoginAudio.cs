using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginAudio : AudioRef<LoginAudio>
{
    [Header("BGM")]
    public AK.Wwise.Event LoginBGM;
    public AK.Wwise.Event PrologueBGM;
    
    protected override void Start()
    {
        base.Start();
        LoginBGM.Post(soundObject);
    }

    protected override void OnDestory()
    {
        base.OnDestory();
        LoginBGM.Stop(soundObject);
    }
}
