using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;

public class MessageManagerL {
    public MessageManagerL() {
        ClientManagerL.i.localClient.MessageReceived += Client_MessageReceived;
    }

    private void Client_MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage()) {

            // Extract tag cagegory from tag
            Tags.Categories tagCategory = (Tags.Categories)Mathf.FloorToInt((float)message.Tag / 1000.0f);

            //// When logged out, client can only recieve connection messages
            //if (ClientConnectionL.i.state == ClientConnectionL.State.loggedOut && tagCategory != Tags.Categories.connection)
            //    return;

            // Send message to be handeled at the right place
            switch (tagCategory) {
                case Tags.Categories.connection:
                    //MessageRecieved(message, e);
                    break;
                case Tags.Categories.login:
                    ClientManagerL.i.loginManager.MessageRecieved(message, e);
                    break;
                case Tags.Categories.player:
                    ClientManagerL.i.playerManager.MessageRecieved(message, e);
                    break;
                case Tags.Categories.game:
                    if (ClientManagerL.i.gameManager != null)
                        ClientManagerL.i.gameManager.MessageRecieved(message, e);
                    break;
                case Tags.Categories.weapons:
                    if (ClientManagerL.i.equipmentManager.weaponManager != null)
                        ClientManagerL.i.equipmentManager.weaponManager.MessageRecieved(message, e);
                    break;
                default:
                    break;
            }
        }
    }
}
