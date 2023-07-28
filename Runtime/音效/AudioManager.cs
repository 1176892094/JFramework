using System.Collections.Generic;
using UnityEngine;

// ReSharper disable All
namespace JFramework.Core
{
    public static class AudioManager
    {
        /// <summary>
        /// 完成音效队列
        /// </summary>
        internal static readonly HashSet<AudioSource> finishList = new HashSet<AudioSource>();

        /// <summary>
        /// 播放音效队列
        /// </summary>
        internal static readonly HashSet<AudioSource> audioList = new HashSet<AudioSource>();
        
        /// <summary>
        /// 游戏音效设置
        /// </summary>
        private static AudioSetting audioSetting = new AudioSetting();

        /// <summary>
        /// 音效挂载系统
        /// </summary>
        private static GameObject gameObject;

        /// <summary>
        /// 背景音乐组件
        /// </summary>
        private static AudioSource audioSource;

        /// <summary>
        /// 管理器名称
        /// </summary>
        private static string Name => nameof(AudioManager);

        /// <summary>
        /// 背景音乐
        /// </summary>
        public static float soundVolume => audioSetting.soundVolume;

        /// <summary>
        /// 游戏声音
        /// </summary>
        public static float audioVolume => audioSetting.audioVolume;

        /// <summary>
        /// 音效管理器初始化
        /// </summary>
        internal static void Awake()
        {
            var transform = GlobalManager.Instance.transform;
            gameObject = transform.Find("PoolManager").gameObject;
            audioSource = gameObject.GetComponent<AudioSource>();
            audioSetting = JsonManager.Decrypt<AudioSetting>(Name);
            SetSound(audioSetting.soundVolume);
            SetAudio(audioSetting.audioVolume);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="path">背景音乐的路径</param>
        public static async void PlaySound(string path)
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Audio,$"播放背景音乐: {path.Green()}");
            var clip = await AssetManager.LoadAsync<AudioClip>(path);
            audioSource.volume = audioSetting.soundVolume;
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }

        /// <summary>
        /// 设置背景音乐
        /// </summary>
        /// <param name="soundVolume">音量的大小</param>
        public static void SetSound(float soundVolume)
        {
            if (!GlobalManager.Runtime) return;
            audioSetting.soundVolume = soundVolume;
            audioSource.volume = soundVolume;
            JsonManager.Encrypt(audioSetting, Name);
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public static void StopSound()
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Audio,$"停止背景音乐");
            audioSource.Pause();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="path">传入音效路径</param>
        public static async void PlayAudio(string path)
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Audio, $"播放音效: {path.Blue()}");
            var audio = finishList.Pop(() => gameObject.AddComponent<AudioSource>());
            var clip = await AssetManager.LoadAsync<AudioClip>(path);
            audioList.Add(audio);
            audio.volume = audioSetting.audioVolume;
            audio.clip = clip;
            audio.Play();
            TimerManager.Pop(clip.length, () => StopAudio(audio));
        }

        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="audioVolume">传入音量大小</param>
        public static void SetAudio(float audioVolume)
        {
            if (!GlobalManager.Runtime) return;
            audioSetting.audioVolume = audioVolume;
            foreach (var audio in audioList)
            {
                audio.volume = audioVolume;
            }

            JsonManager.Encrypt(audioSetting, Name);
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioSource">传入音效数据</param>
        public static void StopAudio(AudioSource audioSource)
        {
            if (!GlobalManager.Runtime) return;
            Log.Info(DebugOption.Audio,$"停止音效: {audioSource.clip.name.Orange()}");
            if (audioList.Contains(audioSource))
            {
                audioSource.Stop();
                audioList.Remove(audioSource);
                finishList.Add(audioSource);
            }
        }

        /// <summary>
        /// 管理器销毁
        /// </summary>
        internal static void Destroy()
        {
            audioList.Clear();
            finishList.Clear();
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