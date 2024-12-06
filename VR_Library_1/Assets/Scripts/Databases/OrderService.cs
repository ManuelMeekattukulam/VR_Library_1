using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Fusion;
using TMPro;

public class OrderService : NetworkBehaviour
{
    [System.Serializable]
    public class User
    {
        public string role;
        public bool isEmailVerified;
        public string name;
        public string email;
        public string id;
    }

    private string message_text = "";
    public TMP_Text _messages;

    void Start()
    {
        StartCoroutine(GetRequest("https://rncbv-134-226-214-244.a.free.pinggy.link/v1/users/674e563c340cac28908cfc08")); // Replace with actual user API URL
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
                    User user = JsonUtility.FromJson<User>(webRequest.downloadHandler.text);
                    if (!string.IsNullOrEmpty(user.name))
                    {
                        message_text = $"Name: {user.name}\nEmail: {user.email}\nRole: {user.role}\nEmail Verified: {user.isEmailVerified}\nID: {user.id}";
                    }
                    else
                    {
                        message_text = "No user details found."; // Fallback if user details are empty
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to parse JSON: {ex.Message}");
                    message_text = "Error loading user details."; // Fallback on parse error
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
        StartCoroutine(GetRequest("https://rncbv-134-226-214-244.a.free.pinggy.link/v1/users/674e563c340cac28908cfc08")); // Replace with actual user API URL
        RPC_SendMessage(message_text);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo rpcInfo = default)
    {
        _messages.text += $"{message}\n\n";
    }
}
