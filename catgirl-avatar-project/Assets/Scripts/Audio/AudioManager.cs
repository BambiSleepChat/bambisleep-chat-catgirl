// Assets/Scripts/Audio/AudioManager.cs
// 🌸 BambiSleep™ Church Audio Management System 🌸
// Centralized pink frilly sound effects and music management

using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace BambiSleep.CatGirl.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0.1f, 3f)]
        public float pitch = 1f;

        public bool loop = false;
        public bool playOnAwake = false;

        [HideInInspector]
        public AudioSource source;
    }

    public class AudioManager : MonoBehaviour
    {
        [Header("🌸 Audio Configuration")]
        [SerializeField] private AudioMixerGroup masterMixerGroup;
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        [SerializeField] private AudioMixerGroup sfxMixerGroup;
        [SerializeField] private AudioMixerGroup voiceMixerGroup;

        [Header("🎵 Sound Effects")]
        [SerializeField] private Sound[] sounds;

        [Header("💎 Pink Frilly Sounds")]
        [SerializeField] private AudioClip[] purringSounds;
        [SerializeField] private AudioClip[] nyanSounds;
        [SerializeField] private AudioClip[] cowMooSounds;
        [SerializeField] private AudioClip[] pinkAuraSounds;

        [Header("🎼 Music Tracks")]
        [SerializeField] private AudioClip[] backgroundMusic;
        [SerializeField] private AudioClip[] combatMusic;
        [SerializeField] private AudioClip[] ambientMusic;

        private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();
        private AudioSource musicSource;
        private AudioSource ambientSource;

        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("AudioManager");
                        instance = go.AddComponent<AudioManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            // Singleton pattern
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            InitializeAudioSources();
        }

        private void Start()
        {
            // Play default background music
            if (backgroundMusic.Length > 0)
            {
                PlayMusic(backgroundMusic[0]);
            }
        }

        private void InitializeAudioSources()
        {
            // Create music source
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.outputAudioMixerGroup = musicMixerGroup;
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = 0.5f;

            // Create ambient source
            ambientSource = gameObject.AddComponent<AudioSource>();
            ambientSource.outputAudioMixerGroup = musicMixerGroup;
            ambientSource.loop = true;
            ambientSource.playOnAwake = false;
            ambientSource.volume = 0.3f;

            // Initialize sound effects
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                sound.source.playOnAwake = sound.playOnAwake;

                // Assign to appropriate mixer group
                sound.source.outputAudioMixerGroup = sfxMixerGroup;

                // Add to dictionary
                soundDictionary[sound.name] = sound;

                if (sound.playOnAwake)
                {
                    sound.source.Play();
                }
            }

            Debug.Log("🎵 Audio Manager initialized with " + sounds.Length + " sounds!");
        }

        // Play a sound by name
        public void Play(string soundName)
        {
            if (soundDictionary.TryGetValue(soundName, out Sound sound))
            {
                sound.source.Play();
            }
            else
            {
                Debug.LogWarning($"⚠️ Sound '{soundName}' not found!");
            }
        }

        // Stop a sound by name
        public void Stop(string soundName)
        {
            if (soundDictionary.TryGetValue(soundName, out Sound sound))
            {
                sound.source.Stop();
            }
        }

        // Play one-shot sound
        public void PlayOneShot(string soundName)
        {
            if (soundDictionary.TryGetValue(soundName, out Sound sound))
            {
                sound.source.PlayOneShot(sound.clip);
            }
        }

        // Play random purring sound
        public void PlayRandomPurr()
        {
            if (purringSounds.Length == 0) return;

            int index = Random.Range(0, purringSounds.Length);
            PlayClip(purringSounds[index], sfxMixerGroup, 0.6f);
        }

        // Play random nyan sound
        public void PlayRandomNyan()
        {
            if (nyanSounds.Length == 0) return;

            int index = Random.Range(0, nyanSounds.Length);
            PlayClip(nyanSounds[index], voiceMixerGroup, 0.8f);
        }

        // Play random cow moo sound
        public void PlayRandomCowMoo()
        {
            if (cowMooSounds.Length == 0) return;

            int index = Random.Range(0, cowMooSounds.Length);
            PlayClip(cowMooSounds[index], voiceMixerGroup, 0.7f);

            Debug.Log("🐄 MOO MOO! 🐄");
        }

        // Play pink aura activation sound
        public void PlayPinkAuraSound()
        {
            if (pinkAuraSounds.Length == 0) return;

            int index = Random.Range(0, pinkAuraSounds.Length);
            PlayClip(pinkAuraSounds[index], sfxMixerGroup, 0.5f);

            Debug.Log("✨ Pink Frilly Aura activated! ✨");
        }

        // Play a clip directly
        private void PlayClip(AudioClip clip, AudioMixerGroup mixerGroup, float volume)
        {
            GameObject tempGO = new GameObject("TempAudio");
            AudioSource tempSource = tempGO.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.outputAudioMixerGroup = mixerGroup;
            tempSource.volume = volume;
            tempSource.Play();

            Destroy(tempGO, clip.length);
        }

        // Music management
        public void PlayMusic(AudioClip music)
        {
            if (musicSource.clip == music && musicSource.isPlaying)
                return;

            musicSource.clip = music;
            musicSource.Play();

            Debug.Log($"🎼 Now playing: {music.name}");
        }

        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void ResumeMusic()
        {
            musicSource.UnPause();
        }

        // Ambient sounds
        public void PlayAmbient(AudioClip ambient)
        {
            if (ambientSource.clip == ambient && ambientSource.isPlaying)
                return;

            ambientSource.clip = ambient;
            ambientSource.Play();
        }

        public void StopAmbient()
        {
            ambientSource.Stop();
        }

        // Volume controls
        public void SetMasterVolume(float volume)
        {
            masterMixerGroup?.audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }

        public void SetMusicVolume(float volume)
        {
            musicMixerGroup?.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        public void SetSFXVolume(float volume)
        {
            sfxMixerGroup?.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }

        public void SetVoiceVolume(float volume)
        {
            voiceMixerGroup?.audioMixer.SetFloat("VoiceVolume", Mathf.Log10(volume) * 20);
        }

        // Crossfade between music tracks
        public void CrossfadeMusic(AudioClip newMusic, float duration = 2f)
        {
            StartCoroutine(CrossfadeMusicCoroutine(newMusic, duration));
        }

        private System.Collections.IEnumerator CrossfadeMusicCoroutine(AudioClip newMusic, float duration)
        {
            float startVolume = musicSource.volume;

            // Fade out current music
            float elapsed = 0f;
            while (elapsed < duration / 2f)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (duration / 2f));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Switch to new music
            musicSource.clip = newMusic;
            musicSource.Play();

            // Fade in new music
            elapsed = 0f;
            while (elapsed < duration / 2f)
            {
                musicSource.volume = Mathf.Lerp(0f, startVolume, elapsed / (duration / 2f));
                elapsed += Time.deltaTime;
                yield return null;
            }

            musicSource.volume = startVolume;
        }

        // Switch to combat music
        public void PlayCombatMusic()
        {
            if (combatMusic.Length == 0) return;

            int index = Random.Range(0, combatMusic.Length);
            CrossfadeMusic(combatMusic[index], 1f);
        }

        // Return to background music
        public void PlayBackgroundMusic()
        {
            if (backgroundMusic.Length == 0) return;

            int index = Random.Range(0, backgroundMusic.Length);
            CrossfadeMusic(backgroundMusic[index], 2f);
        }
    }
}
