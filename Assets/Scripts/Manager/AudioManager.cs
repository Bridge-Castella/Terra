using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public enum AudioChannel
    {
        Master,
        Bgm,
        Sfx,
    }

    public float sfxVolumePercent { get; private set;}
    public float bgmVolumePercent { get; private set; }
    public float masterVolumePercent { get; private set; }

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
        switch(channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue("Master", masterVolumePercent * 100.0f);
                break;
            case AudioChannel.Bgm:
                bgmVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue("BGM", bgmVolumePercent * 100.0f);
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue("SFX", sfxVolumePercent * 100.0f);
                break;
        }

        SaveVolume();
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("bgm vol", bgmVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
    }
}
