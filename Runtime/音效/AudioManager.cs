using System.Collections.Generic;
using System.Threading.Tasks;
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
        /// 音效挂载对象
        /// </summary>
        private static Transform poolManager;

        /// <summary>
        /// 背景音乐组件
        /// </summary>
        private static AudioSource audioSource;

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
        internal static async void Register()
        {
            poolManager = GlobalManager.Instance.transform.Find("PoolManager");
            audioSetting = await JsonManager.Decrypt<AudioSetting>(nameof(AudioManager));
            audioSource = poolManager.GetComponent<AudioSource>();
            await SetSound(audioSetting.soundVolume);
            await SetAudio(audioSetting.audioVolume);
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="path">背景音乐的路径</param>
        public static async void PlaySound(string path)
        {
            if (!GlobalManager.Runtime) return;
            Log.Info($"播放背景音乐: {path.Green()}", Option.Audio);
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
        public static async Task SetSound(float soundVolume)
        {
            if (!GlobalManager.Runtime) return;
            audioSetting.soundVolume = soundVolume;
            audioSource.volume = soundVolume;
            await JsonManager.Encrypt(audioSetting, nameof(AudioManager));
        }

        /// <summary>
        /// 暂停背景音乐
        /// </summary>
        public static void StopSound()
        {
            if (!GlobalManager.Runtime) return;
            Log.Info($"停止背景音乐", Option.Audio);
            audioSource.Pause();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="path">传入音效路径</param>
        public static async void PlayAudio(string path)
        {
            if (!GlobalManager.Runtime) return;
            if (!finishList.TryPop(out var audio))
            {
                audio = poolManager.gameObject.AddComponent<AudioSource>();
                Log.Info($"添加音效组件: {path.Pink()}", Option.Audio);
            }

            Log.Info($"播放音效: {path.Blue()}", Option.Audio);
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
        public static async Task SetAudio(float audioVolume)
        {
            if (!GlobalManager.Runtime) return;
            audioSetting.audioVolume = audioVolume;
            foreach (var audio in audioList)
            {
                audio.volume = audioVolume;
            }

            await JsonManager.Encrypt(audioSetting, nameof(AudioManager));
        }

        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioSource">传入音效数据</param>
        public static void StopAudio(AudioSource audioSource)
        {
            if (!GlobalManager.Runtime) return;
            Log.Info($"停止音效: {audioSource.clip.name.Orange()}", Option.Audio);
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
        internal static void UnRegister()
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