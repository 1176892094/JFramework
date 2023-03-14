using System;
using System.Collections.Generic;
using JFramework.Core;
using UnityEngine;

namespace JFramework
{
    public static class AudioManager
    {
        internal static Queue<AudioSource> audioQueue;
        internal static List<AudioSource> audioList;
        internal static GameObject audioSystem;
        internal static AudioSource audioSource;
        private static AudioSetting audioSetting;
        private static string Name => nameof(AssetManager);
        public static float SoundVolume => audioSetting?.soundVolume ?? 0.5f;
        public static float AudioVolume => audioSetting?.audioVolume ?? 0.5f;
        
        /// <summary>
        /// 音效管理器初始化
        /// </summary>
        internal static void Awake()
        {
            audioList = new List<AudioSource>();
            audioQueue = new Queue<AudioSource>();
            var obj = GlobalManager.Instance.gameObject;
            audioSystem = obj.transform.Find("AudioSystem").gameObject;
            audioSetting = JsonManager.Load<AudioSetting>(Name, true);
            audioSource = audioSystem.GetComponent<AudioSource>();
            SetSound(audioSetting.soundVolume);
            SetAudio(audioSetting.audioVolume);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="path">背景音乐的路径</param>
        public static void PlaySound(string path)
        {
            if (audioSource == null)
            {
                Debug.Log("音乐管理器没有初始化!");
                return;
            }

            AssetManager.LoadAsync<AudioClip>(path, clip =>
            {
                audioSource.volume = audioSetting.soundVolume;
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            });
        }

        /// <summary>
        /// 设置背景音乐
        /// </summary>
        /// <param name="soundVolume">音量的大小</param>
        public static void SetSound(float soundVolume)
        {
            audioSetting.soundVolume = soundVolume;
            audioSource.volume = soundVolume;
            JsonManager.Save(audioSetting, Name, true);
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public static void StopSound()
        {
            if (audioSource == null)
            {
                Debug.Log("音乐管理器没有初始化!");
                return;
            }

            audioSource.Pause();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="path">传入音效路径</param>
        /// <param name="action">获取音效的回调</param>
        public static void PlayAudio(string path, Action<AudioSource> action = null)
        {
            if (audioList == null)
            {
                Debug.Log("音乐管理器没有初始化!");
                return;
            }

            var audio = audioQueue.Count > 0 ? audioQueue.Dequeue() : audioSystem.AddComponent<AudioSource>();
            AssetManager.LoadAsync<AudioClip>(path, clip =>
            {
                audioList.Add(audio);
                audio.volume = audioSetting.audioVolume;
                audio.clip = clip;
                audio.Play();
                TimerManager.Pop(clip.length, () => StopAudio(audio));
                action?.Invoke(audio);
            });
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="audioVolume">传入音量大小</param>
        public static void SetAudio(float audioVolume)
        {
            audioSetting.audioVolume = audioVolume;
            foreach (var audio in audioList)
            {
                audio.volume = audioVolume;
            }

            JsonManager.Save(audioSetting, Name, true);
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioSource">传入音效数据</param>
        public static void StopAudio(AudioSource audioSource)
        {
            if (audioList.Contains(audioSource))
            {
                audioSource.Stop();
                audioList.Remove(audioSource);
                audioQueue.Enqueue(audioSource);
            }
        }

        internal static void Destroy()
        {
            audioList = null;
            audioQueue = null;
            audioSystem = null;
            audioSource = null;
            audioSetting = null;
        }
    }
}