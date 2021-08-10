using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ColorScript : NetworkBehaviour
{
    Vector2 dir;
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
    [ClientCallback]
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            dir = Vector2.right;
            transform.Translate(dir);
            CmdMove(transform.position);
        }
    }
    [Command(requiresAuthority = false)]
    void CmdMove(Vector2 pos)
    {
        RpcMove(pos);
    }
    [ClientRpc]
    void RpcMove(Vector2 pos)
    {
        transform.position = pos;
    }

    [Command(requiresAuthority = false)]
    void CmdCallColor()
    {
        newColor = new Color(Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f), 1);
        RpcChangeColor();
    }
    [ClientRpc]
    void RpcChangeColor()
    {
        sr.color = newColor;
    }
}
