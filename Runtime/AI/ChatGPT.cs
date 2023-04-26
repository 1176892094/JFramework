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
    internal static string key => GlobalManager.Instance.key;
    internal static string chat=> GlobalManager.Instance.chat;

    public static void SendData()
    {
        if (chat.IsEmpty()) return;
        GlobalManager.Instance.StartCoroutine(Request(chat));
        GlobalManager.Instance.chat = "";
    }

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

    public class TextResponse
    {
        public string id;
        public string @object;
        public string created;
        public string model;
        public Usage usage;
        public List<Choice> choices;
    }

    public struct Choice
    {
        public Message message;
        public string finish_reason;
        public int index;
    }

    public struct Message
    {
        public string role;
        public string content;
    }

    public struct Usage
    {
        public string prompt_tokens;
        public string completion_tokens;
        public string total_tokens;
    }
}