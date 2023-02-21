using System;
using System.Collections.Generic;
using JFramework.Core;
using UnityEngine;

namespace JFramework
{
    public class AudioManager : Singleton<AudioManager>
    {
        internal Queue<AudioSource> audioQueue;
        internal List<AudioSource> audioList;
        internal GameObject audioManager;
        internal AudioSource audioSource;
        
        public float SoundVolume
        {
            get
            {
                if (audioSetting != null)
                {
                    return audioSetting.soundVolume;
                }

                return 0;
            }
        }

        public float AudioVolume
        {
            get
            {
                if (audioSetting != null)
                {
                    return audioSetting.audioVolume;
                }

                return 0;
            }
        }

        private AudioSetting audioSetting;

        private string name => nameof(AudioManager);

        /// <summary>
        /// 音效管理器初始化
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            audioList = new List<AudioSource>();
            audioQueue = new Queue<AudioSource>();
            var obj = GlobalManager.Instance.gameObject;
            audioManager = obj.transform.Find("AudioSystem").gameObject;
            audioSetting = JsonManager.Instance.Load<AudioSetting>(name, true);
            audioSource = audioManager.GetComponent<AudioSource>();
            SetSound(audioSetting.soundVolume);
            SetAudio(audioSetting.audioVolume);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="path">背景音乐的路径</param>
        public void PlaySound(string path)
        {
            if (audioSource == null)
            {
                Debug.Log("音乐管理器没有初始化!");
                return;
            }

            AssetManager.Instance.LoadAsync<AudioClip>(path, clip =>
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
        public void SetSound(float soundVolume)
        {
            audioSetting.soundVolume = soundVolume;
            audioSource.volume = soundVolume;
            JsonManager.Instance.Save(audioSetting, name, true);
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public void StopSound()
        {
            audioSource.Pause();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="path">传入音效路径</param>
        /// <param name="action">获取音效的回调</param>
        public void PlayAudio(string path, Action<AudioSource> action = null)
        {
            if (audioList == null)
            {
                Debug.Log("音乐管理器没有初始化!");
                return;
            }

            if (audioQueue.Count > 0)
            {
                var audio = audioQueue.Dequeue();
                AssetManager.Instance.LoadAsync<AudioClip>(path, audioClip =>
                {
                    audioList.Add(audio);
                    PlayAudio(audio, audioClip);
                    action?.Invoke(audio);
                });
            }
            else
            {
                var audio = audioManager.AddComponent<AudioSource>();
                AssetManager.Instance.LoadAsync<AudioClip>(path, clip =>
                {
                    audioList.Add(audio);
                    PlayAudio(audio, clip);
                    action?.Invoke(audio);
                });
            }
        }

        /// <summary>
        /// 设置音效参数
        /// </summary>
        /// <param name="audioSource">传入音源</param>
        /// <param name="audioClip">传入音乐文件</param>
        private void PlayAudio(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.volume = audioSetting.audioVolume;
            audioSource.Play();
            TimerManager.Instance.Listen(audioClip.length, () => StopAudio(audioSource));
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="audioVolume">传入音量大小</param>
        public void SetAudio(float audioVolume)
        {
            audioSetting.audioVolume = audioVolume;
            foreach (var audio in audioList)
            {
                audio.volume = audioVolume;
            }

            JsonManager.Instance.Save(audioSetting, name, true);
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioSource">传入音效数据</param>
        public void StopAudio(AudioSource audioSource)
        {
            if (audioList.Contains(audioSource))
            {
                audioSource.Stop();
                audioList.Remove(audioSource);
                audioQueue.Enqueue(audioSource);
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            audioList = null;
            audioQueue = null;
            audioSource = null;
            audioSetting = null;
        }
    }
}