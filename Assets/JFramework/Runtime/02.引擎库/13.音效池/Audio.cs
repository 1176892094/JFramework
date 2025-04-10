// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:29
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using UnityEngine;

namespace JFramework.Common
{
    public static partial class AudioManager
    {
        public static float musicValue
        {
            get => GlobalManager.settings.musicValue;
            set
            {
                GlobalManager.settings.musicValue = value;
                GlobalManager.Instance.sounds.volume = value;
                JsonManager.Save(GlobalManager.settings, nameof(AudioManager));
            }
        }

        public static float audioValue
        {
            get => GlobalManager.settings.audioValue;
            set
            {
                GlobalManager.settings.audioValue = value;
                foreach (var audioSource in GlobalManager.audioData)
                {
                    audioSource.volume = value;
                }

                JsonManager.Save(GlobalManager.settings, nameof(AudioManager));
            }
        }

        public static async void PlayMain(string assetName, Action<AudioSource> assetAction = null)
        {
            if (!GlobalManager.Instance) return;
            var audioSource = GlobalManager.Instance.sounds;
            audioSource.clip = await AssetManager.Load<AudioClip>(GlobalSetting.GetAudioPath(assetName));
            audioSource.loop = true;
            audioSource.volume = musicValue;
            assetAction?.Invoke(audioSource);
            audioSource.Play();
        }

        public static async void PlayLoop(string assetName, Action<AudioSource> assetAction = null)
        {
            if (!GlobalManager.Instance) return;
            var assetPath = GlobalSetting.GetAudioPath(assetName);
            var assetData = LoadPool(assetPath).Dequeue();
            GlobalManager.audioData.Add(assetData);
            assetData.transform.SetParent(null);
            assetData.gameObject.SetActive(true);
            assetData.clip = await AssetManager.Load<AudioClip>(assetPath);
            assetData.loop = true;
            assetData.volume = audioValue;
            assetAction?.Invoke(assetData);
            assetData.Play();
        }

        public static async void PlayOnce(string assetName, Action<AudioSource> assetAction = null)
        {
            if (!GlobalManager.Instance) return;
            var assetPath = GlobalSetting.GetAudioPath(assetName);
            var assetData = LoadPool(assetPath).Dequeue();
            GlobalManager.audioData.Add(assetData);
            assetData.transform.SetParent(null);
            assetData.gameObject.SetActive(true);
            assetData.clip = await AssetManager.Load<AudioClip>(assetPath);
            assetData.loop = false;
            assetData.volume = audioValue;
            assetAction?.Invoke(assetData);
            assetData.Play();
            assetData.Wait(assetData.clip.length).OnComplete(() => StopLoop(assetData));
        }

        public static void StopMain(bool pause = true)
        {
            if (!GlobalManager.Instance) return;
            if (pause)
            {
                GlobalManager.Instance.sounds.Pause();
            }
            else
            {
                GlobalManager.Instance.sounds.Stop();
            }
        }

        public static void StopLoop(AudioSource assetData)
        {
            if (!GlobalManager.Instance) return;
            if (!GlobalManager.poolGroup.TryGetValue(assetData.name, out var parent))
            {
                parent = new GameObject(Service.Text.Format("Pool - {0}", assetData.name));
                parent.transform.SetParent(GlobalManager.Instance.transform);
                GlobalManager.poolGroup.Add(assetData.name, parent);
            }

            assetData.Stop();
            assetData.gameObject.SetActive(false);
            assetData.transform.SetParent(parent.transform);
            GlobalManager.audioData.Remove(assetData);
            LoadPool(assetData.name).Enqueue(assetData);
        }

        private static AudioPool LoadPool(string assetPath)
        {
            if (GlobalManager.settings == null)
            {
                GlobalManager.settings = new AudioSetting();
                JsonManager.Load(GlobalManager.settings, nameof(AudioManager));
            }

            if (GlobalManager.poolData.TryGetValue(assetPath, out var poolData))
            {
                return (AudioPool)poolData;
            }

            poolData = new AudioPool(typeof(AudioSource), assetPath);
            GlobalManager.poolData.Add(assetPath, poolData);
            return (AudioPool)poolData;
        }

        internal static void Dispose()
        {
            GlobalManager.audioData.Clear();
        }
    }
}