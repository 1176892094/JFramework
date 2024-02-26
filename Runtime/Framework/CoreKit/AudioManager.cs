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
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public sealed class AudioManager : ScriptableObject
    {
        private Transform poolManager;
        private AudioSource audioSource;
        [SerializeField, LabelText("是否启用")] private bool isActive;

        [SerializeField, LabelText("背景音乐"), Range(0, 1f)]
        private float musicVolume = 0.5f;

        [SerializeField, LabelText("游戏音效"), Range(0, 1f)]
        private float audioVolume = 0.5f;

        [ShowInInspector, LabelText("完成列表")] private readonly Stack<AudioSource> stopList = new Stack<AudioSource>();
        [ShowInInspector, LabelText("播放列表")] private readonly HashSet<AudioSource> playList = new HashSet<AudioSource>();

        internal void OnEnable()
        {
            if (!GlobalManager.Instance) return;
            GlobalManager.Json.Load(this);
            poolManager = GlobalManager.Instance.transform.Find("PoolManager");
            audioSource = poolManager.GetComponent<AudioSource>();
            isActive = true;
        }

        public void SetActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public async void PlayMusic(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance || !isActive) return;
            var clip = await GlobalManager.Asset.Load<AudioClip>(SettingManager.GetAudioPath(name));
            audioSource.volume = musicVolume;
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
            action?.Invoke(audioSource);
        }

        public async void PlayOnce(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance || !isActive) return;
            if (!stopList.TryPop(out var audio))
            {
                audio = poolManager.gameObject.AddComponent<AudioSource>();
            }

            var clip = await GlobalManager.Asset.Load<AudioClip>(SettingManager.GetAudioPath(name));
            playList.Add(audio);
            audio.volume = audioVolume;
            audio.clip = clip;
            audio.Play();
            GlobalManager.Time.Pop(clip.length).Invoke(() => StopAudio(audio));
            action?.Invoke(audio);
        }

        public async void PlayLoop(string name, Action<AudioSource> action = null)
        {
            if (!GlobalManager.Instance || !isActive) return;
            if (!stopList.TryPop(out var audio))
            {
                audio = poolManager.gameObject.AddComponent<AudioSource>();
            }

            var clip = await GlobalManager.Asset.Load<AudioClip>(SettingManager.GetAudioPath(name));
            playList.Add(audio);
            audio.volume = audioVolume;
            audio.clip = clip;
            audio.loop = true;
            audio.Play();
            action?.Invoke(audio);
        }

        public void SetMusic(float musicVolume)
        {
            if (!GlobalManager.Instance) return;
            this.musicVolume = musicVolume;
            audioSource.volume = musicVolume;
            GlobalManager.Json.Save(this);
        }

        public void SetAudio(float audioVolume)
        {
            if (!GlobalManager.Instance) return;
            this.audioVolume = audioVolume;
            foreach (var audio in playList)
            {
                audio.volume = audioVolume;
            }

            GlobalManager.Json.Save(this);
        }

        public void StopMusic(bool pause = true)
        {
            if (!GlobalManager.Instance) return;
            if (pause)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Stop();
            }
        }

        public void StopAudio(AudioSource audioSource)
        {
            if (!GlobalManager.Instance) return;
            if (playList.Contains(audioSource))
            {
                audioSource.Stop();
                playList.Remove(audioSource);
                stopList.Push(audioSource);
            }
        }

        internal void OnDisable()
        {
            playList.Clear();
            stopList.Clear();
        }
    }
}