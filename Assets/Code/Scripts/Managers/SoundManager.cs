using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundType
{
    ENEMY_DEATH,
    LIFE_LOST,
    UI_CLICK,
    SHOP_PURCHASE_FINISHED,
    START_WAVE,
    WAVE_COMPLETED,
    MENU_SHOW,
    MENU_CLOSE,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private SoundList[] soundList;
    [SerializeField] private AudioClip[] backgroundMusic;
    [SerializeField][Range(0, 1)] private float maxSoundVolume = 0.65f;
    [SerializeField][Range(0, 1)] private float pitchModulation = 0.1f;

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float soundVolume = 1f;

    private static SoundManager instance;
    private AudioSource audioSource;

    private AudioSource backgroundMusicSource;
    private List<AudioClip> shuffledMusic;
    private int currentTrackIndex = 0;
    private float trackTimer = 0f;

    void Start()
    {
        if (!Application.isPlaying) return;
        if (instance == null) instance = this;

        audioSource = GetComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();

        if (backgroundMusic.Length > 0)
        {
            PreloadMusicClips();
            ShufflePlaylist();
        }
    }

    void Update()
    {
        if (backgroundMusicSource == null) return;
        trackTimer -= Time.deltaTime;
        if (trackTimer <= 0) PlayNextTrack();
    }

    public float GetMusicVolume()
    {
        return masterVolume * Mathf.Max(1, musicVolume);
    }

    public float GetSoundVolume()
    {
        return masterVolume * Mathf.Max(1, instance.soundVolume * instance.maxSoundVolume) * (1 / Time.timeScale);
    }

    private void PlayTrack(int index)
    {
        if (shuffledMusic?.Count == 0) return;

        backgroundMusicSource.clip = shuffledMusic[index];
        backgroundMusicSource.Play();
        backgroundMusicSource.volume = musicVolume;
        trackTimer = backgroundMusicSource.clip.length;
    }

    public static void PlayNextTrack()
    {
        if (instance.shuffledMusic?.Count == 0) return;
        instance.currentTrackIndex += 1;

        if (instance.currentTrackIndex >= instance.shuffledMusic.Count)
        {
            instance.ShufflePlaylist();
        }
        else
        {
            instance.PlayTrack(instance.currentTrackIndex);
        }
    }

    public void ShufflePlaylist()
    {
        if (shuffledMusic == null) shuffledMusic = new List<AudioClip>(backgroundMusic);
        if (backgroundMusicSource == null) backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        AudioClip lastTrackPlayed = backgroundMusicSource?.clip;

        for (int i = 0; i < shuffledMusic.Count; i++)
        {
            AudioClip clip = shuffledMusic[i];
            int randomIndex = UnityEngine.Random.Range(i, shuffledMusic.Count);
            shuffledMusic[i] = shuffledMusic[randomIndex];
            shuffledMusic[randomIndex] = clip;
        }

        // Make sure the same song doesn't play twice in a row
        if (shuffledMusic[0] == lastTrackPlayed) shuffledMusic.Reverse();
        currentTrackIndex = 0;
        PlayTrack(currentTrackIndex);
    }

    private void OnDestroy()
    {
        if (backgroundMusicSource == null) return;
        backgroundMusicSource?.Stop();
    }

    public static void PlayPredefinedSound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        if (clips.Length == 0)
        {
            Debug.Log("No sound found for " + sound.ToString() + " in SoundManager.cs");
            return;
        }
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume * instance.GetSoundVolume());
    }

    public static void PlaySoundEffect(AudioClip clip, float volume = 1)
    {
        instance.audioSource.PlayOneShot(clip, volume * instance.GetSoundVolume());
        instance.audioSource.pitch = UnityEngine.Random.Range(1 - instance.pitchModulation / 2, 1 + instance.pitchModulation / 2);
    }

    public static void SetMusicVolume(float volume)
    {
        instance.musicVolume = volume;
        instance.backgroundMusicSource.volume = volume;
    }

    public static void SetSoundsVolume(float volume)
    {
        instance.soundVolume = volume;
        instance.audioSource.volume = volume;
    }

    void PreloadMusicClips()
    {
        foreach (var clip in instance.backgroundMusic) clip.LoadAudioData();
    }

    private void OnEnable() // changes inspector name to the names from enum
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < names.Length; i++)
            soundList[i].name = names[i];
    }
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}