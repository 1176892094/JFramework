using System;
using System.Collections.Generic;
using UnityEngine;

namespace JFramework
{
    public class AudioManager: MonoBehaviour
    {
        private static readonly List<AudioSource> soundList = new List<AudioSource>();
        private static AudioSource sound;
        private static GameObject source;
        private static float soundVolume = 1;
        private static float audioVolume = 1;

        protected void Awake()
        {
            DontDestroyOnLoad(source = gameObject);
            sound = gameObject.AddComponent<AudioSource>();
            MonoManager.Instance.AddListener(OnUpdate);
        }

        private void OnUpdate()
        {
            for (int i = soundList.Count - 1; i >= 0; --i)
            {
                if (soundList[i] != null)
                {
                    if (!soundList[i].isPlaying)
                    {
                        Destroy(soundList[i]);
                        soundList.RemoveAt(i);
                    }
                }
                else
                {
                    soundList.RemoveAt(i);
                }
            }
        }

        public static void PlaySound(string name)
        {
            ResourceManager.LoadAsync<AudioClip>(name, clip =>
            {
                sound.volume = soundVolume;
                sound.clip = clip;
                sound.loop = true;
                sound.Play();
            });
        }

        public static void ChangeSound(float value)
        {
            soundVolume = value;
            sound.volume = soundVolume;
        }

        public static void PauseSound()
        {
            sound.Pause();
        }

        public static void StopSound()
        {
            sound.Stop();
        }

        public static void PlayAudio(string path, Action<AudioSource> callback = null)
        {
            ResourceManager.LoadAsync<AudioClip>(path, clip =>
            {
                AudioSource audio = source.AddComponent<AudioSource>();
                audio.volume = audioVolume;
                audio.clip = clip;
                audio.Play();
                soundList.Add(audio);
                callback?.Invoke(audio);
            });
        }

        public static void ChangeAudio(float value)
        {
            audioVolume = value;
            foreach (AudioSource audio in soundList)
            {
                audio.volume = audioVolume;
            }
        }

        public static void StopAudio(AudioSource audio)
        {
            if (soundList.Contains(audio))
            {
                soundList.Remove(audio);
                audio.Stop();
                Destroy(audio);
            }
        }

        private void OnDestroy() => MonoManager.Instance.RemoveListener(OnUpdate);
    }
}