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

    //AudioSource[] sfxSources;                                                 // Outdated audio engine
    //AudioSource musicSources;
    //int activeMusicSourceIndex;
    //SoundLibrary library;

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

            //library = GetComponent<SoundLibrary>();                           // Outdated audio engine

            //GameObject newMusicSource = new GameObject("Music Source ");
            //musicSources = newMusicSource.AddComponent<AudioSource>();
            //newMusicSource.transform.parent = transform;

            //sfxSources = new AudioSource[3];
            //for(int i = 0; i < 3; i++)
            //{
                //GameObject newSfxSource = new GameObject("sfx source " + (i + 1));
                //sfxSources[i] = newSfxSource.AddComponent<AudioSource>();
                //newSfxSource.transform.parent = transform;
            //}

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
                PlayerPrefs.SetFloat("master vol", masterVolumePercent);
                break;
            case AudioChannel.Bgm:
                bgmVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue("BGM", bgmVolumePercent * 100.0f);
                PlayerPrefs.SetFloat("bgm vol", bgmVolumePercent);
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                AkSoundEngine.SetRTPCValue("SFX", sfxVolumePercent * 100.0f);
                PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
                break;
        }
        
        //musicSources.volume = bgmVolumePercent * masterVolumePercent;         // Outdated audio engine
        //sfxSources[0].volume = sfxVolumePercent * masterVolumePercent;
        //sfxSources[1].volume = sfxVolumePercent * masterVolumePercent;
        //sfxSources[2].volume = sfxVolumePercent * masterVolumePercent;
    }

    //?????? ????
    //public void PlayMusic(AudioClip clip, float fadeDuration = 1)             // Outdated audio engine
    //{
    //    activeMusicSourceIndex = 1 - activeMusicSourceIndex;
    //    musicSources.clip = clip;
    //    musicSources.loop = true;
    //    musicSources.Play();

    //    StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    //}

    //public void PlaySound(string soundName)
    //{
    //    sfxSources[0].PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
    //}

    //public void PlayAmbientSound(string soundName)
    //{
    //    sfxSources[1].PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
    //}

    //public void PlayWalkSound(string platformType)
    //{
    //    if (!sfxSources[2].isPlaying)
    //    {
    //        sfxSources[2].enabled = true;
    //        sfxSources[2].loop = true;
    //        sfxSources[2].Play();
    //    }
    //    switch (platformType)
    //    {
    //        case "grass":
    //            sfxSources[2].clip = library.GetClipFromName("step_01");
    //            break;
    //    }
    //}

    //public void StopWalkSound()
    //{
    //    sfxSources[2].enabled = false;
    //}

    //public void StopAmbientSound()
    //{
    //    sfxSources[1].Stop();
    //}

    //IEnumerator AnimateMusicCrossfade(float duration)
    //{
    //    float percent = 0;

    //    while(percent<1)
    //    {
    //        percent += Time.deltaTime * 1/duration;
    //        musicSources.volume = Mathf.Lerp(0, bgmVolumePercent * masterVolumePercent, percent);
    //        yield return null;
    //    }
    //}
}
