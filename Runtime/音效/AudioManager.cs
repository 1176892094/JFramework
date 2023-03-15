using System;
using System.Collections.Generic;
using JFramework.Core;
using UnityEngine;

namespace JFramework
{
    public static class AudioManager
    {
        /// <summary>
        /// 完成音效队列
        /// </summary>
        internal static Queue<AudioSource> audioQueue;

        /// <summary>
        /// 播放音效队列
        /// </summary>
        internal static List<AudioSource> audioList;

        /// <summary>
        /// 音效挂载系统
        /// </summary>
        internal static GameObject poolManager;

        /// <summary>
        /// 背景音乐组件
        /// </summary>
        internal static AudioSource audioSource;

        /// <summary>
        /// 游戏音效设置
        /// </summary>
        private static AudioSetting audioSetting;

        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(AudioManager);

        /// <summary>
        /// 背景音乐
        /// </summary>
        public static float SoundVolume => audioSetting?.soundVolume ?? 0.5f;

        /// <summary>
        /// 游戏声音
        /// </summary>
        public static float AudioVolume => audioSetting?.audioVolume ?? 0.5f;

        /// <summary>
        /// 音效管理器初始化
        /// </summary>
        internal static void Awake()
        {
            audioList = new List<AudioSource>();
            audioQueue = new Queue<AudioSource>();
            var transform = GlobalManager.Instance.transform;
            poolManager = transform.Find("PoolManager").gameObject;
            audioSetting = JsonManager.Load<AudioSetting>(Name, true);
            audioSource = poolManager.GetComponent<AudioSource>();
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
                Debug.Log($"{Name.Red()} 没有初始化");
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
            if (audioSource == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化");
                return;
            }

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
                Debug.Log($"{Name.Red()} 没有初始化");
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
            if (audioSource == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化");
                return;
            }

            var audio = audioQueue.Count > 0 ? audioQueue.Dequeue() : poolManager.AddComponent<AudioSource>();
            AssetManager.LoadAsync<AudioClip>(path, clip =>
            {
                audioList.Add(audio);
                audio.volume = audioSetting.audioVolume;
                audio.clip = clip;
                audio.Play();
                action?.Invoke(audio);
                TimerManager.Pop(clip.length, () => StopAudio(audio));
            });
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="audioVolume">传入音量大小</param>
        public static void SetAudio(float audioVolume)
        {
            if (audioSource == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化");
                return;
            }

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
            if (audioSource == null)
            {
                Debug.Log($"{Name.Red()} 没有初始化");
                return;
            }

            if (audioList.Contains(audioSource))
            {
                audioSource.Stop();
                audioList.Remove(audioSource);
                audioQueue.Enqueue(audioSource);
            }
        }

        /// <summary>
        /// 释放管理器
        /// </summary>
        internal static void Destroy()
        {
            audioList = null;
            audioQueue = null;
            poolManager = null;
            audioSource = null;
            audioSetting = null;
        }

        /// <summary>
        /// 音效数据
        /// </summary>
        private class AudioSetting
        {
            /// <summary>
            /// 背景音乐大小
            /// </summary>
            public float soundVolume = 0.5f;

            /// <summary>
            /// 游戏音乐大小
            /// </summary>
            public float audioVolume = 0.5f;
        }
    }
}