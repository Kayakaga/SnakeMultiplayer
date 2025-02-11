﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerOnline : NetworkBehaviour
{
    [SyncVar]
    int winner;
    [SyncVar]
    List<GameObject> players = new List<GameObject>();
    float speed = 0.1f;
    GameObject management;
    SceneManagerr scManager;
    GameObject tailPrefab;
    List<Transform> tail = new List<Transform>();
    bool ate;

    Vector2 dir = Vector2.right;

    public override void OnStartClient()
    {
        base.OnStartClient();
        InvokeRepeating("Move", speed, speed);
    }
    void Start()
    {
        management = GameObject.Find("Management");
        scManager = management.GetComponent<SceneManagerr>();
        tailPrefab = Resources.Load<GameObject>("Tail");
    }
    [Command]
    public void CmdAddPlayer(GameObject obj)
    {
        players.Add(obj);
    }
    [Command]
    public void CmdRemovePlayer(GameObject obj)
    {
        players.Remove(obj);
    }
    public override void OnStopClient()
    {
        CmdRemovePlayer(gameObject);
        base.OnStopClient();
    }
    [SyncVar]
    int pnum;

    void Update()
    {
        if(isServer)
        {
            pnum = NetworkServer.connections.Count;
        }

        if(!isLocalPlayer) return;
        if(pnum < 2)
        {
            management.GetComponent<SceneManagerr>()
        .LoadScene("Winner");
        }
        if(isClient)
        {
            ChangeDir();
        }
    }
    [ClientCallback]
    void ChangeDir()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector2.up;
        }
    }

    [ClientCallback]
    void Move()
    {
        if(!isLocalPlayer) return;
        Vector2 v = transform.position;
        transform.Translate(dir);
        if(ate)
        {
             //Send the server a request to spawn tail
             CmdSpawnTail(v, Quaternion.identity);
             
        }
        else if(tail.Count > 0)
        {
            //Send Tails' positions to server
            CmdSendTailsPositions(v);
        }
        //Send Player's position to server
        CmdSendPlayerPosition(transform.position);
    }
    [Command]
    void CmdSendTailsPositions(Vector2 pos)
    {
        tail.Last().position = pos;
        tail.Insert(0, tail.Last());
        tail.RemoveAt(tail.Count-1);
        RpcSendTailsPositions(pos, tail.Last());
    }
    [ClientRpc]
    void RpcSendTailsPositions(Vector2 pos, Transform tail)
    {
        tail.position = pos;
    }
    [Command]
    void CmdSpawnTail(Vector2 pos, Quaternion rot)
    {
        GameObject g = (GameObject)Instantiate(tailPrefab ,pos, rot);
        NetworkServer.Spawn(g);
        tail.Insert(0, g.transform);
        ate = false;
        //Send back to target player
        RpcSpawnTail(connectionToClient , g);
    }
    [TargetRpc]
    void RpcSpawnTail(NetworkConnection target, GameObject g)
    {
        tail.Insert(0, g.transform);
        ate = false;
    }
    [Command]
    void CmdSendPlayerPosition(Vector2 playerPos)
    {
        transform.position = playerPos;
        //Send Player Position to Clients
        RpcSendPlayerPosition(playerPos);
    }
    [ClientRpc]
    void RpcSendPlayerPosition(Vector2 playerPos)
    {
        transform.position = playerPos;
    }
    [ClientCallback]
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(!isLocalPlayer) return;
        if(coll.CompareTag("Food"))
        {
            ate = true;
            //Send a request to the server to destroy the food
            CmdDestroy(coll.gameObject);
        }
        else
        {
            Die();
        }
    }
    [ClientCallback]
    void Die()
    {
        winner = GameObject.FindObjectOfType<NetworkRoomPlayer>()
        .index + 1;
        //Send winner to server
        CmdSendWinner(winner);
        Debug.Log("Winner: "+winner);
        management.GetComponent<SceneManagerr>()
        .LoadScene("Winner");
    }
    [Command]
    void CmdSendWinner(int win)
    {
        winner = win;
        TextScore.winner = win;
        Debug.Log("Winner: "+winner);
        //Send winner to clients
        RpcSendWinner(winner);
    }
    [ClientRpc]
    void RpcSendWinner(int win)
    {
        winner = win;
        TextScore.winner = win;
    }
    [Command]
    void CmdDestroy(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }
}
