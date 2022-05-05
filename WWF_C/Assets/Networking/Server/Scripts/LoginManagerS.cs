using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Server;

[System.Serializable]
public class LoginManagerS {
    public delegate void PlayerLoggedInDelegate(ushort clientId, DrDatas.Login.LoginData loginData);
    public event PlayerLoggedInDelegate playerLoggedInEvent;

    public void MessageRecieved(Message message, MessageReceivedEventArgs e) {
        switch (message.Tag) {
            case Tags.login_loginRequestToServer:
                OnMsg_loginRequest(message, e);
                break;
            default:
                break;
        }
    }

    public void OnMsg_loginRequest(Message message, MessageReceivedEventArgs e) {
        DrDatas.Login.LoginData loginData = message.Deserialize<DrDatas.Login.LoginData>();

        // Make sure player with the username is not already connected
        if (ServerManagerS.i.playerManager.players.ContainsKey(e.Client.ID)) {
            LoginFailed(e.Client, "Name is taken");
            return;
        }

        LoginSuccess(e.Client, loginData);
    }

    private void LoginFailed(IClient client, string failureReason) {

        // Send failure message
        using (Message msgOut = Message.Create(Tags.login_loginFailed, new DrDatas.Login.LoginFailedData(failureReason))) {
            client.SendMessage(msgOut, SendMode.Reliable);
        }
    }

    private void LoginSuccess(IClient client, DrDatas.Login.LoginData loginData) {

        using (Message msgOut = Message.Create(Tags.login_loginSuccess, new DrDatas.Login.LoginSuccessData(SceneSwapper.i.activeSceneName)))
            client.SendMessage(msgOut, SendMode.Reliable);

        playerLoggedInEvent?.Invoke(client.ID, loginData);
    }
}
