// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 19:12:32
// # Recently: 2024-12-22 20:12:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JFramework
{
    internal sealed partial class DefaultHelper : IAudioHelper
    {
        object IAudioHelper.Instantiate(string assetPath)
        {
            var assetData = new GameObject(assetPath).AddComponent<AudioSource>();
            Object.DontDestroyOnLoad(assetData.gameObject);
            return assetData;
        }

        void IAudioHelper.MusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        void IAudioHelper.AudioVolume(float volume)
        {
            foreach (var audioSource in audioSources)
            {
                audioSource.volume = volume;
            }
        }

        async Task IAudioHelper.OnDequeue<T>(string assetPath, T assetData, int assetMode)
        {
            if (assetData is AudioSource audioSource)
            {
                audioSource.transform.SetParent(null);
                audioSource.gameObject.SetActive(true);
                audioSource.clip = await Service.Asset.Load<AudioClip>(assetPath);
                switch (assetMode)
                {
                    case 0:
                        if (musicSource != null)
                        {
                            Service.Audio.Pause(musicSource);
                        }

                        audioSource.loop = true;
                        audioSource.volume = Service.Audio.musicValue;
                        musicSource = audioSource;
                        break;
                    case 1:
                        audioSource.loop = true;
                        audioSource.volume = Service.Audio.audioValue;
                        audioSources.Add(audioSource);
                        break;
                    case 2:
                        audioSource.loop = false;
                        audioSource.volume = Service.Audio.audioValue;
                        audioSources.Add(audioSource);
                        GlobalManager.Instance.Watch(audioSource.clip.length).Invoke(() => Service.Audio.Stop(audioSource));
                        break;
                }

                audioSource.Play();
            }
        }

        string IAudioHelper.OnEnqueue<T>(T assetData, bool pause)
        {
            if (assetData is AudioSource audioSource)
            {
                if (audioSource != musicSource)
                {
                    audioSources.Remove(audioSource);
                }
                else
                {
                    musicSource = null;
                }

                if (pause)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.Stop();
                }

                return LoadParent(audioSource.transform);
            }

            return default;
        }
    }
}