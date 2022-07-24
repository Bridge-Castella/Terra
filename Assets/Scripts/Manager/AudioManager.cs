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

    AudioSource sfxSource;
    AudioSource musicSources;
    int activeMusicSourceIndex;

    SoundLibrary library;

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

            library = GetComponent<SoundLibrary>();

            GameObject newMusicSource = new GameObject("Music Source ");
            musicSources = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;

            GameObject newSfxSource = new GameObject("sfx source");
            sfxSource = newSfxSource.AddComponent<AudioSource>();
            newSfxSource.transform.parent = transform;

            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1);
            bgmVolumePercent =  PlayerPrefs.GetFloat("bgm vol", 1);
            sfxVolumePercent =  PlayerPrefs.GetFloat("sfx vol", 1);
        }
    }

    //���� ����
    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch(channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Bgm:
                bgmVolumePercent = volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
        }


        musicSources.volume = bgmVolumePercent * masterVolumePercent;
        sfxSource.volume = sfxVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("bgm vol", bgmVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
    }

    //������ ���
    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources.clip = clip;
        musicSources.loop = true;
        musicSources.Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    //�̸����� ���̺귯���� �ִ� ȿ���� ã�� --> ȿ���� ���۵Ǹ� ���̺귯���� �ֱ�
    public void PlaySound(string soundName)
    {
        sfxSource.PlayOneShot(library.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while(percent<1)
        {
            percent += Time.deltaTime * 1/duration;
            musicSources.volume = Mathf.Lerp(0, bgmVolumePercent * masterVolumePercent, percent);
            yield return null;
        }
    }
}
