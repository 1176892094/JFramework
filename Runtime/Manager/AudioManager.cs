using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JYJFramework
{
    public class AudioManager: MonoBehaviour
    {
        private static readonly List<AudioSource> soundList = new List<AudioSource>();
        private static AudioSource sound;
        private static GameObject source;

        protected void Awake()
        {
            source = gameObject;
            DontDestroyOnLoad(gameObject);
            sound = gameObject.AddComponent<AudioSource>();
        }

        private void Update()
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
                sound.clip = clip;
                sound.loop = true;
                sound.Play();
            });
        }

        public static void ChangeSound(float value)
        {
            sound.volume = value;
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
                audio.clip = clip;
                audio.Play();
                soundList.Add(audio);
                callback?.Invoke(audio);
            });
        }

        public static void ChangeAudio(float value)
        {
            foreach (AudioSource audio in soundList)
            {
                audio.volume = value;
            }
        }

        public static void StopAudio(AudioSource audio)
        {
            if (soundList.Contains(audio))
            {
                soundList.Remove(audio);
                audio.Stop();
                Object.Destroy(audio);
            }
        }
    }
}