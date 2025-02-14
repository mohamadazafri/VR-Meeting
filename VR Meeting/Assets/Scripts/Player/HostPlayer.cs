using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostPlayer : Player
{
    public WhiteboardSpawner whiteboard;
    public PollSpawner pollPanel;
    public VivoxVoiceManager vivoxVoiceManager;

    public override void startServer()
    {
        NetworkManager.Singleton.StartHost();
    }

    public override void spawnWhiteboard()
    {
        whiteboard.spawnAllWhiteboard();
    }

    public override void spawnPoll() {
        pollPanel.spawnPoll();
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
