// *********************************************************************************
// # Project: JFramework
// # Unity: 2022.3.5f1c1
// # Author: Charlotte
// # Version: 1.0.0
// # History: 2023-10-25  00:01
// # Copyright: 2023, Charlotte
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All
namespace JFramework.Core
{
    public static class AudioManager
    {
        /// <summary>
        /// 完成音效列表
        /// </summary>
        internal static readonly Stack<AudioSource> stacks = new Stack<AudioSource>();

        /// <summary>
        /// 播放音效列表
        /// </summary>
        internal static readonly HashSet<AudioSource> audios = new HashSet<AudioSource>();

        /// <summary>
        /// 游戏音效设置
        /// </summary>
        internal static AudioData audioData = new AudioData();

        /// <summary>
        /// 音效挂载对象
        /// </summary>
        private static Transform poolManager;

        /// <summary>
        /// 背景音乐组件
        /// </summary>
        private static AudioSource audioSource;

        /// <summary>
        /// 是否启用音乐管理器
        /// </summary>
        public static bool isActive;

        /// <summary>
        /// 音效管理器初始化
        /// </summary>
        internal static void Register()
        {
            poolManager = GlobalManager.Instance.transform.Find("PoolManager");
            audioData = JsonManager.Decrypt<AudioData>(nameof(AudioManager));
            audioSource = poolManager.GetComponent<AudioSource>();
            SetMusic(audioData.musicVolume);
            SetAudio(audioData.audioVolume);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name">背景音乐的路径</param>
        public static void PlayMusic(string name)
        {
            if (!GlobalManager.Runtime || !isActive) return;
            AssetManager.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name), clip =>
            {
                audioSource.volume = audioData.musicVolume;
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            });
        }

        /// <summary>
        /// 设置背景音乐
        /// </summary>
        /// <param name="soundVolume">音量的大小</param>
        public static void SetMusic(float soundVolume)
        {
            if (!GlobalManager.Runtime) return;
            audioData.musicVolume = soundVolume;
            audioSource.volume = soundVolume;
            JsonManager.Encrypt(audioData, nameof(AudioManager));
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public static void StopMusic(bool pause = true)
        {
            if (!GlobalManager.Runtime) return;
            if (pause)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Stop();
            }
        }

        /// <summary>
        /// 播放一次音效
        /// </summary>
        /// <param name="name">传入音效路径</param>
        /// <param name="action">音效播放事件</param>
        public static void PlayOnce(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Runtime || !isActive) return;
            if (!stacks.TryPop(out var audio))
            {
                audio = poolManager.gameObject.AddComponent<AudioSource>();
            }

            AssetManager.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name), clip =>
            {
                audios.Add(audio);
                audio.volume = audioData.audioVolume;
                audio.clip = clip;
                audio.Play();
                action?.Invoke(audio);
                TimerManager.Pop(clip.length).Invoke(() => StopAudio(audio));
            });
        }

        /// <summary>
        /// 播放循环音效
        /// </summary>
        /// <param name="name">传入音效路径</param>
        /// <param name="action">音效播放事件</param>
        public static void PlayLoop(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Runtime || !isActive) return;
            if (!stacks.TryPop(out var audio))
            {
                audio = poolManager.gameObject.AddComponent<AudioSource>();
            }

            AssetManager.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name), clip =>
            {
                audios.Add(audio);
                audio.volume = audioData.audioVolume;
                audio.clip = clip;
                audio.loop = true;
                audio.Play();
                action?.Invoke(audio);
            });
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="audioVolume">传入音量大小</param>
        public static void SetAudio(float audioVolume)
        {
            if (!GlobalManager.Runtime) return;
            audioData.audioVolume = audioVolume;
            foreach (var audio in audios)
            {
                audio.volume = audioVolume;
            }

            JsonManager.Encrypt(audioData, nameof(AudioManager));
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioSource">传入音效数据</param>
        public static void StopAudio(AudioSource audioSource)
        {
            if (!GlobalManager.Runtime) return;
            if (audios.Contains(audioSource))
            {
                audioSource.Stop();
                audios.Remove(audioSource);
                stacks.Push(audioSource);
            }
        }

        /// <summary>
        /// 管理器销毁
        /// </summary>
        internal static void UnRegister()
        {
            audios.Clear();
            stacks.Clear();
        }
    }
}