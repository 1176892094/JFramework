// *********************************************************************************
// # Project: Test
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2024-02-04  18:42
// # Copyright: 2024, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed class AudioManager : ScriptableObject
    {
        [ShowInInspector, LabelText("完成列表")] private List<AudioSource> stops = new();
        [ShowInInspector, LabelText("播放列表")] private List<AudioSource> plays = new();
        [SerializeField, Range(0, 1f)] public float audioVolume = 0.5f;
        [SerializeField, Range(0, 1f)] public float soundVolume = 0.5f;
        private GameObject poolManager;
        private AudioSource audioSource;

        internal void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            poolManager = GlobalManager.Instance.transform.Find("PoolManager").gameObject;
            audioSource = poolManager.GetComponent<AudioSource>();
            GlobalManager.Json.Load(this);
        }

        public async void PlayAudio(string name, Action<AudioSource> action = null)
        {
            var clip = await GlobalManager.Asset.Load<AudioClip>(SettingManager.GetAudioPath(name));
            audioSource.volume = audioVolume;
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
            action?.Invoke(audioSource);
        }

        public async void PlayOnce(string name, Action<AudioSource> action = null)
        {
            var clip = await GlobalManager.Asset.Load<AudioClip>(SettingManager.GetAudioPath(name));
            var sound = stops.Count > 0 ? stops[0] : poolManager.AddComponent<AudioSource>();
            stops.Remove(sound);
            plays.Add(sound);
            sound.volume = soundVolume;
            sound.clip = clip;
            GlobalManager.Time.Pop(clip.length).Invoke(() => StopSound(sound));
            sound.Play();
            action?.Invoke(sound);
        }

        public async void PlayLoop(string name, Action<AudioSource> action = null)
        {
            var clip = await GlobalManager.Asset.Load<AudioClip>(SettingManager.GetAudioPath(name));
            var sound = stops.Count > 0 ? stops[0] : poolManager.AddComponent<AudioSource>();
            stops.Remove(sound);
            plays.Add(sound);
            sound.volume = soundVolume;
            sound.clip = clip;
            sound.loop = true;
            sound.Play();
            action?.Invoke(sound);
        }

        public void SetAudio(float audioVolume)
        {
            this.audioVolume = audioVolume;
            audioSource.volume = audioVolume;
            GlobalManager.Json.Save(this);
        }

        public void SetSound(float soundVolume)
        {
            this.soundVolume = soundVolume;
            foreach (var sound in plays)
            {
                sound.volume = soundVolume;
            }

            GlobalManager.Json.Save(this);
        }

        public void StopAudio(bool pause = true)
        {
            if (pause)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Stop();
            }
        }

        public void StopSound(AudioSource sound)
        {
            if (plays.Contains(sound))
            {
                sound.Stop();
                plays.Remove(sound);
                stops.Add(sound);
            }
        }

        internal void OnDisable()
        {
            plays.Clear();
            stops.Clear();
            poolManager = null;
            audioSource = null;
        }
    }
}