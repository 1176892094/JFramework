using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JFramework.Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        /// <summary>
        /// 背景音效的组件
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("当前背景音乐"), FoldoutGroup("音效管理视图")]
        private AudioSource audioSource;

        /// <summary>
        /// 背景音乐大小
        /// </summary>
        [ShowInInspector, ReadOnly, Range(0, 1), LabelText("背景音乐大小"), FoldoutGroup("音效管理视图")]
        private float soundVolume = 1;

        /// <summary>
        /// 游戏音效大小
        /// </summary>
        [ShowInInspector, ReadOnly, Range(0, 1), LabelText("游戏音效大小"), FoldoutGroup("音效管理视图")]
        private float audioVolume = 1;

        /// <summary>
        /// 正在播放音效的列表
        /// </summary>
        [ShowInInspector, ReadOnly, LabelText("音效播放列表"), FoldoutGroup("音效管理视图")]
        private List<AudioSource> soundList;

        /// <summary>
        /// 构造函数初始化数据
        /// </summary>
        protected override void OnInit(params object[] args)
        {
            soundList = new List<AudioSource>();
            audioSource = gameObject.AddComponent<AudioSource>();
        }


        /// <summary>
        /// 当音效播放完毕时移除
        /// </summary>
        protected override void OnUpdate()
        {
            for (int i = soundList.Count - 1; i >= 0; --i)
            {
                if (soundList[i] != null)
                {
                    if (!soundList[i].isPlaying)
                    {
                        Destroy(soundList[i]);
                        soundList.RemoveAt(i);
                    }
                }
                else
                {
                    soundList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 音效管理器播放指定的背景音乐
        /// </summary>
        /// <param name="name">背景音乐的名称</param>
        public void PlaySound(string name)
        {
            AssetManager.Instance.LoadAsync<AudioClip>(name, clip =>
            {
                audioSource.volume = soundVolume;
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            });
        }


        /// <summary>
        /// 音效管理器设置背景音乐的音量
        /// </summary>
        /// <param name="value">音量的大小</param>
        public void SetSound(float value)
        {
            soundVolume = value;
            audioSource.volume = soundVolume;
        }


        /// <summary>
        /// 音效管理器获取背景音乐的毁掉
        /// </summary>
        /// <param name="action">背景音乐的回调</param>
        public void GetSound(Action<AudioSource> action)
        {
            action?.Invoke(audioSource);
        }

        /// <summary>
        /// 音效管理器播放指定的音效
        /// </summary>
        /// <param name="name">音效的名称</param>
        /// <param name="action">音效的回调</param>
        public void PlayAudio(string name, Action<AudioSource> action = null)
        {
            AssetManager.Instance.LoadAsync<AudioClip>(name, clip =>
            {
                AudioSource sound = gameObject.AddComponent<AudioSource>();
                sound.volume = audioVolume;
                sound.clip = clip;
                sound.Play();
                soundList.Add(sound);
                action?.Invoke(sound);
            });
        }


        /// <summary>
        /// 音效管理器设置音效的音量
        /// </summary>
        /// <param name="value">音量的大小</param>
        public void SetAudio(float value)
        {
            audioVolume = value;
            foreach (AudioSource local in soundList)
            {
                local.volume = audioVolume;
            }
        }

        /// <summary>
        /// 音效管理器停止指定的音效
        /// </summary>
        /// <param name="audio">需要停止的音效组件</param>
        public void StopAudio(AudioSource audio)
        {
            if (soundList.Contains(audio))
            {
                audio.Stop();
                soundList.Remove(audio);
                Destroy(audio);
            }
        }
    }
}