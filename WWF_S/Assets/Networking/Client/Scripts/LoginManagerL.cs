using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

public class LoginManagerL {
    public delegate void PlayerLoggedInDelegate(ushort clientId, DrDatas.Login.LoginData loginData);
    public event PlayerLoggedInDelegate playerLoggedInEvent;

    public LoginManagerL() {
        UiWindow_login.OnClickEvent_btnLogin += UiWindow_login_OnClickEvent_btnLogin;
    }

    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        switch (message.Tag) {
            case Tags.login_loginSuccess:
                OnMsg_loginSuccess(message, e);
                break;
            case Tags.login_loginFailed:
                OnMsg_loginFailed(message, e);
                break;
            default:
                break;
        }
    }

    private void OnMsg_loginSuccess(Message message, MessageReceivedEventArgs e) {
        DrDatas.Login.LoginSuccessData successData = message.Deserialize<DrDatas.Login.LoginSuccessData>();
        SceneSwapper.i.LoadScene(successData.sceneName);
    }

    private void OnMsg_loginFailed(Message message, MessageReceivedEventArgs e) {
        Debug.Log("LOGIN FAILED");
    }

    private void UiWindow_login_OnClickEvent_btnLogin(string username) {
        if (ClientConnectionL.i.state != ClientConnectionL.State.loggedOut) {
            Debug.LogError("Can't log in: Already logged in");
            return;
        }

        // Generate username if none provided
        if (username == "")
            username = Guid.NewGuid().ToString();

        // Send login request to server
        using (Message msgOut = Message.Create(Tags.login_loginRequestToServer, new DrDatas.Login.LoginData(username))) {
            ClientManagerL.i.localClient.SendMessage(msgOut, SendMode.Reliable);;
        }
    }
}
