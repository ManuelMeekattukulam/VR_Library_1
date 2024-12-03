using TMPro;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

public class API_CALL : MonoBehaviour
{
    [System.Serializable]
    public class Fact
    {
        public string fact;
        public int length;
    }

    public TextMeshProUGUI text;

    private string defaultText = "Loading..."; // Set a default text

    void Start()
    {
        text.text = defaultText; // Ensure default text is displayed initially
        StartCoroutine(GetRequest("https://catfact.ninja/fact"));
    }

    public void OnRefresh()
    {
        text.text = defaultText; // Reset to default text during refresh
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
                        text.text = fact.fact; // Update text only with valid data
                    }
                    else
                    {
                        text.text = "No fact found."; // Fallback if fact is empty
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to parse JSON: {ex.Message}");
                    text.text = "Error loading fact."; // Fallback on parse error
                }
            }
            else
            {
                Debug.LogError($"Request failed: {webRequest.error}");
                text.text = "Error fetching data."; // Fallback on request error
            }
        }
    }
}
