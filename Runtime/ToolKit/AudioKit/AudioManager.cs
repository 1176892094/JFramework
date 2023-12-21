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
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable All

namespace JFramework
{
    public sealed partial class GlobalManager
    {
        /// <summary>
        /// 音效管理器
        /// </summary>
        public sealed class AudioManager : Controller
        {
            /// <summary>
            /// 完成音效列表
            /// </summary>
            [ShowInInspector] private readonly Stack<AudioSource> stacks = new Stack<AudioSource>();

            /// <summary>
            /// 播放音效列表
            /// </summary>
            [ShowInInspector] private readonly HashSet<AudioSource> audios = new HashSet<AudioSource>();

            /// <summary>
            /// 游戏音效设置
            /// </summary>
            [ShowInInspector] private AudioData audioData = new AudioData();

            /// <summary>
            /// 音效挂载对象
            /// </summary>
            private Transform poolManager;

            /// <summary>
            /// 背景音乐组件
            /// </summary>
            private AudioSource audioSource;

            /// <summary>
            /// 是否启用音乐管理器
            /// </summary>
            public bool isActive;

            /// <summary>
            /// 音效管理器初始化
            /// </summary>
            private void Awake()
            {
                isActive = true;
                poolManager = owner.transform.Find("PoolManager");
                audioData = Json.Decrypt<AudioData>(nameof(AudioManager));
                audioSource = poolManager.GetComponent<AudioSource>();
                SetMusic(audioData.musicVolume);
                SetAudio(audioData.audioVolume);
            }

            /// <summary>
            /// 播放背景音乐
            /// </summary>
            /// <param name="name">背景音乐的路径</param>
            public void PlayMusic(string name)
            {
                if (!Runtime || !isActive) return;
                Asset.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name), clip =>
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
            public void SetMusic(float soundVolume)
            {
                if (!Runtime) return;
                audioData.musicVolume = soundVolume;
                audioSource.volume = soundVolume;
                Json.Encrypt(audioData, nameof(AudioManager));
            }

            /// <summary>
            /// 暂停背景音乐
            /// </summary>
            public void StopMusic(bool pause = true)
            {
                if (!Runtime) return;
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
            public void PlayOnce(string name, Action<AudioSource> action = null)
            {
                if (!Runtime || !isActive) return;
                if (!stacks.TryPop(out var audio))
                {
                    audio = poolManager.gameObject.AddComponent<AudioSource>();
                }

                Asset.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name), clip =>
                {
                    audios.Add(audio);
                    audio.volume = audioData.audioVolume;
                    audio.clip = clip;
                    audio.Play();
                    action?.Invoke(audio);
                    Time.Pop(clip.length).Invoke(() => StopAudio(audio));
                });
            }

            /// <summary>
            /// 播放循环音效
            /// </summary>
            /// <param name="name">传入音效路径</param>
            /// <param name="action">音效播放事件</param>
            public void PlayLoop(string name, Action<AudioSource> action = null)
            {
                if (!Runtime || !isActive) return;
                if (!stacks.TryPop(out var audio))
                {
                    audio = poolManager.gameObject.AddComponent<AudioSource>();
                }

                Asset.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name), clip =>
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
            public void SetAudio(float audioVolume)
            {
                if (!Runtime) return;
                audioData.audioVolume = audioVolume;
                foreach (var audio in audios)
                {
                    audio.volume = audioVolume;
                }

                Json.Encrypt(audioData, nameof(AudioManager));
            }

            /// <summary>
            /// 停止音效
            /// </summary>
            /// <param name="audioSource">传入音效数据</param>
            public void StopAudio(AudioSource audioSource)
            {
                if (!Runtime) return;
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
            internal void OnDestroy()
            {
                audios.Clear();
                stacks.Clear();
            }
        }
    }
}