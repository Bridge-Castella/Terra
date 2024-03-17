using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public string MasterVolStr = "Master";
    public string BGMVolStr = "BGM";
    public string SFXVolStr = "SFX";
    public string MonsterGrassVolStr = "Monster_Grass";

    public enum AudioChannel
    {
        Master,
        Bgm,
        Sfx,
    }

    public float sfxVolumePercent { get; private set;}
    public float bgmVolumePercent { get; private set; }
    public float masterVolumePercent { get; private set; }

    public static PlayerAudio Player => PlayerAudio.Instance;
    public static UIAudio UI => UIAudio.Instance;
    public static LoginAudio Login => LoginAudio.Instance;
    public static InGameAudio Game => InGameAudio.Instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 0.5f);
            bgmVolumePercent =  PlayerPrefs.GetFloat("bgm vol", 0.5f);
            sfxVolumePercent =  PlayerPrefs.GetFloat("sfx vol", 0.5f);

            SetVolume(masterVolumePercent, AudioChannel.Master);
            SetVolume(bgmVolumePercent, AudioChannel.Bgm);
            SetVolume(sfxVolumePercent, AudioChannel.Sfx);
        }
    }

    //???? ????
    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        if (volumePercent < 0)
        {
            volumePercent = 0f;
        }
        else if (volumePercent > 1)
        {
            volumePercent = 1f;
        }

        switch(channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue(MasterVolStr, masterVolumePercent * 100.0f);
                break;
            case AudioChannel.Bgm:
                bgmVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue(BGMVolStr, bgmVolumePercent * 100.0f);
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue(SFXVolStr, sfxVolumePercent * 100.0f);
                break;
        }

        SaveVolume();
    }

    public void SetMonsterGrassVolume(float volumePercent)
    {
        if (volumePercent < 0)
        {
            volumePercent = 0f;
        }
        else if (volumePercent > 1)
        {
            volumePercent = 1f;
        }

        AkSoundEngine.SetRTPCValue(MonsterGrassVolStr, volumePercent * 100.0f);
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("bgm vol", bgmVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
    }
}

public class AudioRef<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    [SerializeField] protected GameObject soundObject;

    protected virtual void Start()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Debug.LogError("Multiple reference of Login Audio detected");
            Destroy(gameObject);
            return;
        }
    }

    protected virtual void OnDestory()
    {
        Instance = null;
    }

    public static void Post(AK.Wwise.Event source)
    {
        Post(source, (Instance as AudioRef<T>).soundObject);
    }

    public static void Post(AK.Wwise.Event source, GameObject soundObj)
    {
        if (source == null || source.ObjectReference == null)
        {
            Debug.LogWarning("WARNING: audio source is null. " +
                (nameof(source) ?? "Audio") + " will not be played");
            return;
        }

        source.Post(soundObj);
    }

    public static void Stop(AK.Wwise.Event source)
    {
        Stop(source, (Instance as AudioRef<T>).soundObject);
    }

    public static void Stop(AK.Wwise.Event source, GameObject soundObj)
    {
        if (source == null || source.ObjectReference == null)
        {
            Debug.LogWarning("WARNING: audio source is null. " +
                (nameof(source) ?? "Audio") + " will not be stoped");
            return;
        }

        source.Stop(soundObj);
    }
}