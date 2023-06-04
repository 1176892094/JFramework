using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JFramework;
using UnityEngine;
using UnityEngine.Networking;

internal static class ChatGPT
{
    /// <summary>
    /// 通过携程发送请求
    /// </summary>
    /// <param name="key">API密钥</param>
    /// <param name="prompt">发送数据</param>
    /// <returns></returns>
    public static IEnumerator Request(string key, string prompt)
    {
        using var request = new UnityWebRequest(PostConst.Url, "POST");
        {
            var postData = new PostData
            {
                model = PostConst.Model,
                prompt = prompt,
                max_tokens = PostConst.MaxTokens,
                temperature = PostConst.Temperature,
                top_p = PostConst.TopPage,
                frequency_penalty = PostConst.FrequencyPenalty,
                presence_penalty = PostConst.PresencePenalty,
                stop = PostConst.Stop
            };

            var jsonText = JsonUtility.ToJson(postData);
            var jsonByte = Encoding.UTF8.GetBytes(jsonText);
            request.uploadHandler = new UploadHandlerRaw(jsonByte);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {key}");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    var message = request.downloadHandler.text;
                    var response = JsonUtility.FromJson<Response>(message);
                    if (response != null && response.choices.Count > 0)
                    {
                        var text = response.choices[0].text;
                        text = text.Trim();
                        Debug.Log(text);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"{nameof(ChatGPT).Red()} => 返回数据解析失败!\n" + e);
                }
            }
            else
            {
                Debug.LogError($"{nameof(ChatGPT).Red()} => {request.error}");
            }
        }
    }

    /// <summary>
    /// 一些常量
    /// </summary>
    private struct PostConst
    {
        public const string Url = "https://api.openai.com/v1/completions";
        public const string Model = "text-davinci-003";
        public const int MaxTokens = 1024;
        public const float Temperature = 0.9f;
        public const int TopPage = 1;
        public const float FrequencyPenalty = 0;
        public const float PresencePenalty = 0.6f;
        public const string Stop = "[\"Human:\",\"AI:\"]";
    }

    /// <summary>
    /// 提交的数据
    /// </summary>
    [Serializable]
    public class PostData
    {
        public string model;
        public string prompt;
        public int max_tokens;
        public float temperature;
        public int top_p;
        public float frequency_penalty;
        public float presence_penalty;
        public string stop;
    }

    /// <summary>
    /// 返回的响应文本
    /// </summary>
    [Serializable]
    public class Response
    {
        public string id;
        public string created;
        public string model;
        public List<Message> choices;

        [Serializable]
        public class Message
        {
            public string text;
            public string index;
            public string finish_reason;
        }
    }
}