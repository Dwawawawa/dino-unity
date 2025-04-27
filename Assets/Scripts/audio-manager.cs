using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
    }

    public SoundEffect[] sounds;
    private Dictionary<string, SoundEffect> soundDictionary = new Dictionary<string, SoundEffect>();
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        
        // 사운드 딕셔너리 초기화
        foreach (SoundEffect sound in sounds)
        {
            soundDictionary[sound.name] = sound;
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out SoundEffect sound))
        {
            audioSource.pitch = sound.pitch;
            audioSource.PlayOneShot(sound.clip, sound.volume);
        }
        else
        {
            Debug.LogWarning($"Sound {soundName} not found in AudioManager");
        }
    }

    public void PlayBackgroundMusic(string musicName)
    {
        if (soundDictionary.TryGetValue(musicName, out SoundEffect music))
        {
            audioSource.clip = music.clip;
            audioSource.loop = true;
            audioSource.volume = music.volume;
            audioSource.pitch = music.pitch;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music {musicName} not found in AudioManager");
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
