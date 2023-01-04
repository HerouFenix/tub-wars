using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixerGroup mixer;
    #region Static Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if(instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool firstMusicIsPlaying;
    #endregion

    private void Awake()
    {
        //Make sure we don't destroy this
        DontDestroyOnLoad(this.gameObject);

        //Create audio sources, and save them as references
        musicSource = this.gameObject.AddComponent<AudioSource>();
        musicSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxSource = this.gameObject.AddComponent<AudioSource>();

        // Loop music tracks
        musicSource.loop = true;
        musicSource2.loop = true;

        //Add mixer to audio
        musicSource.outputAudioMixerGroup = mixer;
        musicSource2.outputAudioMixerGroup = mixer;
        sfxSource.outputAudioMixerGroup = mixer;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (firstMusicIsPlaying) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.volume = 1;
        activeSource.Play();
    }

    public void PlayMusic(AudioClip musicClip, float volume)
    {
        AudioSource activeSource = (firstMusicIsPlaying) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.volume = volume;
        activeSource.Play();
    }

    public void ToggleMusic(bool toggle)
    {
        AudioSource activeSource = (firstMusicIsPlaying) ? musicSource : musicSource2;

        if (toggle) activeSource.UnPause();
        else activeSource.Pause();
    }

    public void PlayMusicWithFade(AudioClip newAudioClip,float volume, float transitionTime = 1.0f)
    {
        //Determina qual audio ta ativo
        AudioSource activeSource = (firstMusicIsPlaying) ? musicSource : musicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource, newAudioClip, transitionTime, volume));

    }
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime, float volume)
    {
        //Make sure the source is active and playing
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        //Fade out
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (volume - (t / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        //Fade in
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (t / transitionTime);
            yield return null;
        }
    }

    public void PlayMusicWithCrossFade(AudioClip musicClip,float volume, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (firstMusicIsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicIsPlaying) ? musicSource2 : musicSource;


        firstMusicIsPlaying = !firstMusicIsPlaying;

        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime, volume));
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime, float volume)
    {
        float t = 0.0f;

        for (t = 0.0f; t<= transitionTime; t+= Time.deltaTime)
        {
            original.volume = (volume - (t / transitionTime));
            newSource.volume = (t / transitionTime * volume);
            yield return null;
        }
        original.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySfXInPos(AudioClip clip, float volume, Vector2 pos)
    {
        AudioSource.PlayClipAtPoint(clip, pos, volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    //Portanto, agora para usar este audiomanager, nos scripts de cada inimigo, player, level, boss e por ai. 
    //Basta chamar: AudioManager.Instance.FuncaoQueQuisermos
    //Para isso os scripts precisam de ter os sons.
}
