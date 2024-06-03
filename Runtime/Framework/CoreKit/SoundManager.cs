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
using UnityEngine;

namespace JFramework.Core
{
    public static class SoundManager
    {
        internal static readonly List<AudioSource> stops = new();
        internal static readonly List<AudioSource> plays = new();
        private static readonly AudioSetting audioSetting = new AudioSetting();
        private static GameObject poolManager;
        private static AudioSource audioSource;
        public static float audioValue
        {
            get => audioSetting.audioVolume;
            set => audioSetting.audioVolume = value;
        }
        
        public static float soundValue
        {
            get => audioSetting.soundVolume;
            set => audioSetting.soundVolume = value;
        }

        internal static void Register()
        {
            poolManager = GlobalManager.Instance.transform.Find("PoolManager").gameObject;
            audioSource = poolManager.GetComponent<AudioSource>();
            JsonManager.Load(audioSetting, nameof(SoundManager));
        }

        public static async void PlayAudio(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            audioSource.volume = audioValue;
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
            action?.Invoke(audioSource);
        }

        public static async void PlayOnce(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            var sound = stops.Count > 0 ? stops[0] : poolManager.AddComponent<AudioSource>();
            stops.Remove(sound);
            plays.Add(sound);
            sound.volume = soundValue;
            sound.clip = clip;
            TimerManager.Pop(clip.length).Invoke(() => StopSound(sound));
            sound.Play();
            action?.Invoke(sound);
        }

        public static async void PlayLoop(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            var sound = stops.Count > 0 ? stops[0] : poolManager.AddComponent<AudioSource>();
            stops.Remove(sound);
            plays.Add(sound);
            sound.volume = soundValue;
            sound.clip = clip;
            sound.loop = true;
            sound.Play();
            action?.Invoke(sound);
        }

        public static void SetAudio(float audioVolume)
        {
            audioSetting.audioVolume = audioVolume;
            audioSource.volume = audioVolume;
            JsonManager.Save(audioSetting, nameof(SoundManager));
        }

        public static void SetSound(float soundVolume)
        {
            audioSetting.soundVolume = soundVolume;
            foreach (var sound in plays)
            {
                sound.volume = soundVolume;
            }

            JsonManager.Save(audioSetting, nameof(SoundManager));
        }

        public static void StopAudio(bool pause = true)
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

        public static void StopSound(AudioSource sound)
        {
            if (plays.Contains(sound))
            {
                sound.Stop();
                plays.Remove(sound);
                stops.Add(sound);
            }
        }

        internal static void UnRegister()
        {
            plays.Clear();
            stops.Clear();
            poolManager = null;
            audioSource = null;
        }
        
        [Serializable]
        public class AudioSetting
        {
            public float audioVolume = 0.5f;
            public float soundVolume = 0.5f;
        }
    }
}