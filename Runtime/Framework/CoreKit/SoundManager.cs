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
        private static string audioName;
        private static AudioSource audioSource;
        private static readonly SoundSetting setting = new SoundSetting();
        internal static readonly Dictionary<GameObject, AudioSource> audios = new();


        public static float audioValue
        {
            get => setting.audioVolume;
            set => setting.audioVolume = value;
        }

        public static float soundValue
        {
            get => setting.soundVolume;
            set => setting.soundVolume = value;
        }

        internal static void Register()
        {
            audioName = SettingManager.GetAudioPath(nameof(AudioSource));
            audioSource = GlobalManager.Instance.gameObject.AddComponent<AudioSource>();
            JsonManager.Load(setting, nameof(SoundManager));
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
            GameObject obj;
            if (audios.Count == 0)
            {
                obj = new GameObject(audioName);
                obj.AddComponent<AudioSource>();
                PoolManager.Push(obj);
            }

            obj = await PoolManager.Pop(audioName);
            var sound = obj.GetComponent<AudioSource>();
            audios.Remove(obj);
            sound.volume = soundValue;
            sound.clip = clip;
            sound.Play();
            action?.Invoke(sound);
            TimerManager.Pop(clip.length).Invoke(() => StopSound(obj, sound));
        }

        public static async void PlayLoop(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            GameObject obj;
            if (audios.Count == 0)
            {
                obj = new GameObject(audioName);
                obj.AddComponent<AudioSource>();
                PoolManager.Push(obj);
            }

            obj = await PoolManager.Pop(audioName);
            var sound = obj.GetComponent<AudioSource>();
            audios.Remove(obj);
            sound.volume = soundValue;
            sound.clip = clip;
            sound.loop = true;
            sound.Play();
            action?.Invoke(sound);
        }

        public static void SetAudio(float audioVolume)
        {
            setting.audioVolume = audioVolume;
            audioSource.volume = audioVolume;
            JsonManager.Save(setting, nameof(SoundManager));
        }

        public static void SetSound(float soundVolume)
        {
            setting.soundVolume = soundVolume;
            foreach (var sound in audios.Values)
            {
                sound.volume = soundVolume;
            }

            JsonManager.Save(setting, nameof(SoundManager));
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


        public static void StopSound(GameObject obj, AudioSource sound)
        {
            sound.Stop();
            audios.Add(obj, sound);
            PoolManager.Push(obj);
        }

        internal static void UnRegister()
        {
            audios.Clear();
            audioSource = null;
        }
        
        private struct SoundData
        {
            
        }

        [Serializable]
        public class SoundSetting
        {
            public float audioVolume = 0.5f;
            public float soundVolume = 0.5f;
        }
    }
}