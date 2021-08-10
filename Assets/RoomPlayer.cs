using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomPlayer : NetworkRoomPlayer
{
    void Update()
    {
        if(!isLocalPlayer) return;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(readyToBegin)
            CmdChangeReadyState(false);
            else
            CmdChangeReadyState(true);
        }
    }
}
