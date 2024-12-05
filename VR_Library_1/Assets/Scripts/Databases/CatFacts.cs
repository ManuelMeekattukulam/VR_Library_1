using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;

public class CatFacts : NetworkBehaviour
{
    private string cat_message ="Cat Message Here";
    public TMP_Text _messages;
    //public TMP_Text input;
    //public TMP_Text usernameInput;
    //public string username = "Default";

    //public void SetUsername()
    //{
    //    username = usernameInput.text;
    //}

    public void CallMessageRPC()
    {
        //string message = input.text;
        RPC_SendMessage(cat_message);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo rpcInfo = default)
    {
        _messages.text += $"{message}\n";
    }

}
