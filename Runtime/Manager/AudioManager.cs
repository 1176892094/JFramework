using System;
using System.Collections.Generic;
using JFramework.Basic;
using UnityEngine;

namespace JFramework
{
    public class AudioManager : BaseManager<AudioManager>
    {
        private readonly List<AudioSource> soundList = new List<AudioSource>();
        private AudioSource sound;
        private GameObject source;
        private float soundVolume = 1;
        private float audioVolume = 1;

        protected override void Awake()
        {
            base.Awake();
            source = gameObject;
            sound = gameObject.AddComponent<AudioSource>();
            MonoManager.Instance.Listen(OnUpdate);
        }

        protected override void OnUpdate()
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

        public void PlaySound(string name)
        {
            ResourceManager.LoadAsync<AudioClip>(name, clip =>
            {
                sound.volume = soundVolume;
                sound.clip = clip;
                sound.loop = true;
                sound.Play();
            });
        }

        public void ChangeSound(float value)
        {
            soundVolume = value;
            sound.volume = soundVolume;
        }

        public void PauseSound()
        {
            sound.Pause();
        }

        public void StopSound()
        {
            sound.Stop();
        }

        public void PlayAudio(string path, Action<AudioSource> callback = null)
        {
            ResourceManager.LoadAsync<AudioClip>(path, clip =>
            {
                AudioSource local = source.AddComponent<AudioSource>();
                local.volume = audioVolume;
                local.clip = clip;
                local.Play();
                soundList.Add(local);
                callback?.Invoke(local);
            });
        }

        public void ChangeAudio(float value)
        {
            audioVolume = value;
            foreach (AudioSource local in soundList)
            {
                local.volume = audioVolume;
            }
        }

        public void StopAudio(AudioSource audio)
        {
            if (soundList.Contains(audio))
            {
                soundList.Remove(audio);
                audio.Stop();
                Destroy(audio);
            }
        }
    }
}