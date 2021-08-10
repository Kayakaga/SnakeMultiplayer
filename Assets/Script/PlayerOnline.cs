using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerOnline : NetworkBehaviour
{
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
    [ClientCallback]
    void Update()
    {
        if(!isLocalPlayer) return;
        ChangeDir();
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

           // GameObject g = (GameObject)Instantiate(tailPrefab, v,
            // Quaternion.identity);
             //Send the server a request to spawn tail
             CmdSpawnTail(v, Quaternion.identity);
             
        }
        else if(tail.Count > 0)
        {
            tail.Last().position = v;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count-1);
        }
        //Send Player's position to server
        CmdSendPlayerPosition(transform.position);
        //Send Tails' positions to server
        //CmdSendTailsPositions();
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
        //Send Player Position to Clients
        RpcSendPlayerPosition(playerPos);
    }
    [ClientRpc]
    void RpcSendPlayerPosition(Vector2 playerPos)
    {
        transform.position = playerPos;
    }
        void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Food"))
        {
            ate = true;
            Destroy(coll.gameObject);
        }
        else
        {
            //Die();
        }
    }
}
