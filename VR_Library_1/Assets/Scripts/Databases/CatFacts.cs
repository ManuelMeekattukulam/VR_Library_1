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

    //private string defaultText = "Loading..."; // Set a default text
    private string message_text = ""; 
    public TMP_Text _messages;

    //private string cat_message ="Cat Message Here";
    //public TMP_Text input;
    //public TMP_Text usernameInput;
    //public string username = "Default";

    //public void SetUsername()
    //{
    //    username = usernameInput.text;
    //}


    void Start()
    {
        //_messages.text = defaultText; // Ensure default text is displayed initially
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
        // _messages.text = defaultText; // Reset to default text during refresh
        StartCoroutine(GetRequest("https://catfact.ninja/fact"));
        // string message = _messages.text;
        RPC_SendMessage(message_text);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo rpcInfo = default)
    {
        _messages.text += $"{message}\n\n";
    }


    //public void OnRefresh()
    //{
    //    _messages.text = defaultText; // Reset to default text during refresh
    //    StartCoroutine(GetRequest("https://catfact.ninja/fact"));
    //}


}
