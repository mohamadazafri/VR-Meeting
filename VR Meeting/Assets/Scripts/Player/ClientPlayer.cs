using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientPlayer : Player
{
    public VivoxVoiceManager vivoxVoiceManager;

    public override void startServer()
    {
        NetworkManager.Singleton.StartClient();
    }
    public override void spawnWhiteboard()
    {
    }

    public override void spawnPoll()
    {
    }
    public override void loginVivox()
    {
        vivoxVoiceManager.LoginToVivoxService();
    }

    public override Player clone()
    {
        return (Player)this.MemberwiseClone();
    }
}
