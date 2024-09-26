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
    [SerializeField][Range(0, 1)] private float soundToMusicVolume = 0.65f;

    private float musicVolume = 1;
    private float soundVolume = 1;

    private static SoundManager instance;
    private AudioSource audioSource;
    private AudioSource backgroundMusicSource;

    private List<AudioClip> shuffledMusic;
    private int currentTrackIndex = 0;

    private void Start()
    {
        if (!Application.isPlaying)
            return;

        if (instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();

        // Handle music
        if (backgroundMusic.Length > 0)
        {
            // Copy and shuffle the background music array
            shuffledMusic = new List<AudioClip>(backgroundMusic);
            Shuffle(shuffledMusic);

            // Add AudioSource component
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
            backgroundMusicSource.loop = false; // We will handle looping manually

            // Start playing the shuffled music
            StartCoroutine(PlayNextMusicTrack());
        }
    }

    IEnumerator PlayNextMusicTrack()
    {
        while (true)
        {
            if (shuffledMusic.Count == 0) yield break;

            backgroundMusicSource.clip = shuffledMusic[currentTrackIndex];
            backgroundMusicSource.Play();
            backgroundMusicSource.volume = musicVolume;
            currentTrackIndex = (currentTrackIndex + 1) % shuffledMusic.Count;

            // Wait for the current track to finish before playing the next one
            yield return new WaitForSeconds(backgroundMusicSource.clip.length);
        }
    }

    private void Shuffle(List<AudioClip> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            AudioClip temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void OnDestroy()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop();
            Destroy(backgroundMusicSource);
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;

        if (clips.Length == 0)
        {
            Debug.Log("No sound found for " + sound.ToString() + " in SoundManager.cs");
            return;
        }

        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume * instance.soundVolume * instance.soundToMusicVolume);
    }

    public static void PlayAudioClip(AudioClip clip, float volume = 1)
    {
        instance.audioSource.PlayOneShot(clip, volume * instance.soundVolume * instance.soundToMusicVolume);
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

    private void OnEnable()
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