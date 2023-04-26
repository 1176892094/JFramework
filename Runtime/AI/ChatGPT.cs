using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JFramework;
using JFramework.Core;
using JFramework.Table;
using UnityEngine;
using UnityEngine.Networking;

// ReSharper disable All
internal static class ChatGPT
{
    /// <summary>
    /// ChatGPT的密钥
    /// </summary>
    internal static string key => GlobalManager.Instance.key;

    /// <summary>
    /// 发送的文本
    /// </summary>
    internal static string chat => GlobalManager.Instance.chat;

    /// <summary>
    /// 发送数据
    /// </summary>
    public static void SendData()
    {
        if (chat.IsEmpty()) return;
        GlobalManager.Instance.StartCoroutine(Request(chat));
        GlobalManager.Instance.chat = "";
    }

    /// <summary>
    /// 通过携程发送请求
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    private static IEnumerator Request(string prompt)
    {
        using var request = new UnityWebRequest(ChatConst.Url, "POST");
        var jsonText = JsonUtility.ToJson(new PostData
        {
            model = ChatConst.Model,
            prompt = prompt,
            max_tokens = ChatConst.MaxTokens,
            temperature = ChatConst.Temperature,
            top_p = ChatConst.TopPage,
            frequency_penalty = ChatConst.FrequencyPenalty,
            presence_penalty = ChatConst.PresencePenalty,
            stop = ChatConst.Stop
        });
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
                var replyMsg = request.downloadHandler.text;
                var response = JsonUtility.FromJson<TextResponse>(replyMsg);
                if (response != null && response.choices.Count > 0)
                {
                    var index = response.choices.Count - 1;
                    var message = response.choices[index].message;
                    message.content = message.content.Trim();
                    Debug.Log(message.content);
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

    /// <summary>
    /// 一些常量
    /// </summary>
    private struct ChatConst
    {
        public const string Url = "https://api.openai.com/v1/completions";
        public const string Model = "text-davinci-003";
        public const int MaxTokens = 1024;
        public const float Temperature = 0.9f;
        public const int TopPage = 1;
        public const float FrequencyPenalty = 0.0f;
        public const float PresencePenalty = 0.6f;
        public const string Stop = "\\n";
    }

    /// <summary>
    /// 提交的数据
    /// </summary>
    public struct PostData
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
    public class TextResponse
    {
        public string id;
        public string @object;
        public string created;
        public string model;
        public Usage usage;
        public List<Choice> choices;
    }

    /// <summary>
    /// 文本的选项
    /// </summary>
    public struct Choice
    {
        public Message message;
        public string finish_reason;
        public int index;
    }

    /// <summary>
    /// 文本的消息
    /// </summary>
    public struct Message
    {
        public string role;
        public string content;
    }

    /// <summary>
    /// 返回的用户
    /// </summary>
    public struct Usage
    {
        public string prompt_tokens;
        public string completion_tokens;
        public string total_tokens;
    }
}