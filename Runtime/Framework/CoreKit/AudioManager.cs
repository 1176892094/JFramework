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
    public static class AudioManager
    {
        private static readonly List<AudioSource> audios = new List<AudioSource>();
        private static readonly AudioSetting setting = new AudioSetting();
        private static string sourceName;
        private static AudioSource mainSource;
        public static float mainVolume => setting.mainValue;
        public static float audioVolume => setting.audioValue;

        internal static void Register()
        {
            var manager = PoolManager.manager.gameObject;
            mainSource = manager.AddComponent<AudioSource>();
            sourceName = SettingManager.GetAudioPath(nameof(AudioSource));
            JsonManager.Load(setting, nameof(AudioManager));
            manager.AddComponent<AudioListener>();
        }

        public static async void PlayMain(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance || name.IsEmpty()) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            if (clip == null) return;
            mainSource.volume = mainVolume;
            mainSource.clip = clip;
            mainSource.loop = true;
            mainSource.Play();
            action?.Invoke(mainSource);
        }

        public static async void PlayOnce(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance || name.IsEmpty()) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            if (clip == null) return;
            var audio = await LoadSource(clip);
            audios.Remove(audio);
            audio.Play();
            action?.Invoke(audio);
            GlobalManager.Instance.Wait(clip.length).Invoke(() => StopAudio(audio.gameObject));
        }

        public static async void PlayLoop(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance || name.IsEmpty()) return;
            var clip = await AssetManager.Load<AudioClip>(SettingManager.GetAudioPath(name));
            if (clip == null) return;
            var audio = await LoadSource(clip);
            audios.Remove(audio);
            audio.loop = true;
            audio.Play();
            action?.Invoke(audio);
        }
        
        public static async Task<AudioSource> LoadSource(AudioClip clip)
        {
            GameObject obj;
            if (audios.Count == 0)
            {
                obj = new GameObject(sourceName);
                obj.AddComponent<AudioSource>();
                PoolManager.Push(obj);
            }

            obj = await PoolManager.Pop(sourceName);
            var source = obj.GetComponent<AudioSource>();
            source.volume = audioVolume;
            source.clip = clip;
            return source;
        }

        public static void SetMain(float mainVolume)
        {
            setting.mainValue = mainVolume;
            mainSource.volume = mainVolume;
            JsonManager.Save(setting, nameof(AudioManager));
        }

        public static void SetAudio(float audioVolume)
        {
            setting.audioValue = audioVolume;
            foreach (var audio in audios)
            {
                audio.volume = audioVolume;
            }

            JsonManager.Save(setting, nameof(AudioManager));
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
                audios.Add(source);
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
            public float mainValue = 0.5f;
            public float audioValue = 0.5f;
        }
    }
}