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

namespace JFramework.Core
{
    /// <summary>
    /// 音效管理器
    /// </summary>
    public sealed class AudioManager : Component<GlobalManager>
    {
        /// <summary>
        /// 完成音效列表
        /// </summary>
        [ShowInInspector, LabelText("已完成")] private readonly Stack<AudioSource> stacks = new Stack<AudioSource>();

        /// <summary>
        /// 播放音效列表
        /// </summary>
        [ShowInInspector, LabelText("播放中")] private readonly HashSet<AudioSource> audios = new HashSet<AudioSource>();

        /// <summary>
        /// 是否启用音乐管理器
        /// </summary>
        [SerializeField] private bool isActive;

        /// <summary>
        /// 背景音乐大小
        /// </summary>
        [SerializeField, Range(0, 1f)] private float musicVolume = 0.5f;

        /// <summary>
        /// 游戏音乐大小
        /// </summary>
        [SerializeField, Range(0, 1f)] private float audioVolume = 0.5f;

        /// <summary>
        /// 音效挂载对象
        /// </summary>
        private Transform poolManager;

        /// <summary>
        /// 背景音乐组件
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// 音效管理器初始化
        /// </summary>
        private void Awake()
        {
            isActive = true;
            GlobalManager.Json.Load(this);
            poolManager = owner.transform.Find("PoolManager");
            audioSource = poolManager.GetComponent<AudioSource>();
        }

        /// <summary>
        /// 设置管理器是否活跃
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="name">背景音乐的路径</param>
        /// <param name="action"></param>
        public async void PlayMusic(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Runtime || !isActive) return;
            var clip = await GlobalManager.Asset.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name));
            audioSource.volume = musicVolume;
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
            action?.Invoke(audioSource);
        }

        /// <summary>
        /// 设置背景音乐
        /// </summary>
        /// <param name="musicVolume">音量的大小</param>
        public void SetMusic(float musicVolume)
        {
            if (!GlobalManager.Runtime) return;
            this.musicVolume = musicVolume;
            audioSource.volume = musicVolume;
            GlobalManager.Json.Save(this);
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void StopMusic(bool pause = true)
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
        /// <param name="action"></param>
        public async void PlayOnce(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Runtime || !isActive) return;
            if (!stacks.TryPop(out var audio))
            {
                audio = poolManager.gameObject.AddComponent<AudioSource>();
            }

            var clip = await GlobalManager.Asset.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name));
            audios.Add(audio);
            audio.volume = audioVolume;
            audio.clip = clip;
            audio.Play();
            GlobalManager.Time.Pop(clip.length).Invoke(() => StopAudio(audio));
            action?.Invoke(audio);
        }

        /// <summary>
        /// 播放循环音效
        /// </summary>
        /// <param name="name">传入音效路径</param>
        /// <param name="action"></param>
        public async void PlayLoop(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Runtime || !isActive) return;
            if (!stacks.TryPop(out var audio))
            {
                audio = poolManager.gameObject.AddComponent<AudioSource>();
            }

            var clip = await GlobalManager.Asset.LoadAsync<AudioClip>(GlobalSetting.GetAudioPath(name));
            audios.Add(audio);
            audio.volume = audioVolume;
            audio.clip = clip;
            audio.loop = true;
            audio.Play();
            action?.Invoke(audio);
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="audioVolume">传入音量大小</param>
        public void SetAudio(float audioVolume)
        {
            if (!GlobalManager.Runtime) return;
            this.audioVolume = audioVolume;
            foreach (var audio in audios)
            {
                audio.volume = audioVolume;
            }

            GlobalManager.Json.Save(this);
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioSource">传入音效数据</param>
        public void StopAudio(AudioSource audioSource)
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
        internal void OnDestroy()
        {
            audios.Clear();
            stacks.Clear();
        }
    }
}