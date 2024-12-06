using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Fusion;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class CatFacts : NetworkBehaviour
{
    [System.Serializable]
    public class Fact
    {
        public string fact;
        public int length;
    }

    private string message_text = ""; 
    public TMP_Text _messages;

    void Start()
    {
        StartCoroutine(GetRequest("https://catfact.ninja/fact"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    Fact fact = JsonUtility.FromJson<Fact>(webRequest.downloadHandler.text);
                    if (!string.IsNullOrEmpty(fact.fact))
                    {
                        message_text = fact.fact; // Update text only with valid data
                    }
                    else
                    {
                        message_text = "No fact found."; // Fallback if fact is empty
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to parse JSON: {ex.Message}");
                    message_text = "Error loading fact."; // Fallback on parse error
                }
            }
            else
            {
                Debug.LogError($"Request failed: {webRequest.error}");
                message_text = "Error fetching data."; // Fallback on request error
            }
        }
    }

    public void CallMessageRPC()
    {
        StartCoroutine(GetRequest("https://catfact.ninja/fact"));
        RPC_SendMessage(message_text);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo rpcInfo = default)
    {
        _messages.text += $"{message}\n\n";
    }
}
