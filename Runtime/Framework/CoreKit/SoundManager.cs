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
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework.Core
{
    public static class SoundManager
    {
        private static string sourceName;
        private static AudioSource mainSource;
        private static readonly AudioSetting setting = new AudioSetting();
        private static readonly Dictionary<GameObject, AudioSource> audios = new();

        public static float audioValue
        {
            get => setting.mainVolume;
            set => setting.mainVolume = value;
        }

        public static float soundValue
        {
            get => setting.audioVolume;
            set => setting.audioVolume = value;
        }

        internal static void Register()
        {
            sourceName = SettingManager.GetAudioPath(nameof(AudioSource));
            mainSource = GlobalManager.Instance.GetComponentInChildren<AudioSource>();
            JsonManager.Load(setting, nameof(SoundManager));
        }

        public static async void PlayMain(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            mainSource.volume = audioValue;
            mainSource.clip = clip;
            mainSource.loop = true;
            mainSource.Play();
            action?.Invoke(mainSource);
        }

        public static async void PlayOnce(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            var audio = await new AudioData().Build(clip);
            audios.Remove(audio.entity);
            audio.source.Play();
            action?.Invoke(audio.source);
            TimerManager.Pop(clip.length).Invoke(() => StopAudio(audio.entity));
        }

        public static async void PlayLoop(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            var data = await new AudioData().Build(clip);
            audios.Remove(data.entity);
            data.source.loop = true;
            data.source.Play();
            action?.Invoke(data.source);
        }

        public static void SetMain(float mainVolume)
        {
            setting.mainVolume = mainVolume;
            mainSource.volume = mainVolume;
            JsonManager.Save(setting, nameof(SoundManager));
        }

        public static void SetAudio(float audioVolume)
        {
            setting.audioVolume = audioVolume;
            foreach (var audio in audios.Values)
            {
                audio.volume = audioVolume;
            }

            JsonManager.Save(setting, nameof(SoundManager));
        }

        public static void StopMain(bool pause = true)
        {
            if (pause)
            {
                mainSource.Pause();
            }
            else
            {
                mainSource.Stop();
            }
        }


        public static void StopAudio(GameObject obj)
        {
            if (obj.TryGetComponent(out AudioSource source))
            {
                source.Stop();
                audios.Add(obj, source);
                PoolManager.Push(obj);
            }
        }

        internal static void UnRegister()
        {
            audios.Clear();
            mainSource = null;
        }

        [Serializable]
        public class AudioSetting
        {
            public float mainVolume = 0.5f;
            public float audioVolume = 0.5f;
        }

        [Serializable]
        private struct AudioData
        {
            public GameObject entity;
            public AudioSource source;

            public async Task<AudioData> Build(AudioClip clip)
            {
                if (audios.Count == 0)
                {
                    entity = new GameObject(sourceName);
                    source = entity.AddComponent<AudioSource>();
                    PoolManager.Push(entity);
                }

                entity = await PoolManager.Pop(sourceName);
                source = entity.GetComponent<AudioSource>();
                source.volume = soundValue;
                source.clip = clip;
                return this;
            }
        }
    }
}