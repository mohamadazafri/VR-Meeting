using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class Player 
{
    public abstract void startServer();
    public abstract void spawnWhiteboard();
    public abstract void spawnPoll();
    public abstract void loginVivox();
    // Start is called before the first frame update
    public abstract Player clone();
}
