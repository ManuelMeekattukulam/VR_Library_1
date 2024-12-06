using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Fusion;
using TMPro;

public class BookService : NetworkBehaviour
{
    [System.Serializable]
    public class Book
    {
        public string title;
        public string author;
        public string isbn;
        public int publishYear;
        public string id;
        public string description;
    }

    private string message_text = "";
    public TMP_Text _messages;

    void Start()
    {
        StartCoroutine(GetRequest("https://rncbv-134-226-214-244.a.free.pinggy.link/v1/books/675304d2f35f15088a62a4d7")); // Replace with actual book API URL
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
                    Book book = JsonUtility.FromJson<Book>(webRequest.downloadHandler.text);
                    if (!string.IsNullOrEmpty(book.title))
                    {
                        message_text = $"Title: {book.title}\nAuthor: {book.author}\nISBN: {book.isbn}\nYear: {book.publishYear}\nID: {book.id}description: {book.description}\n";
                    }
                    else
                    {
                        message_text = "No book details found."; // Fallback if book details are empty
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to parse JSON: {ex.Message}");
                    message_text = "Error loading book details."; // Fallback on parse error
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
        StartCoroutine(GetRequest("https://rncbv-134-226-214-244.a.free.pinggy.link/v1/books/675304d2f35f15088a62a4d7")); // Replace with actual book API URL
        RPC_SendMessage(message_text);
    }
 
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo rpcInfo = default)
    {
        _messages.text += $"{message}\n\n";
    }
}
