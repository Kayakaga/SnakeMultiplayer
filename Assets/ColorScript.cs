using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColorScript : NetworkBehaviour
{
    SpriteRenderer sr;
    [SyncVar]
    Color newColor;
    [ClientCallback]
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        CmdCallColor();
        InvokeRepeating("CmdCallColor", 1f, 1f);
    }
    [Command]
    void CmdCallColor()
    {
        RpcChangeColor();
    }
    [ClientRpc]
    void RpcChangeColor()
    {
        newColor = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), 1);
        sr.color = newColor;
    }
}
