using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class Player : NetworkBehaviour
{
    float speed = 0.5f;
    GameObject management;
    SceneManagerr scManager;
    GameObject tailPrefab;
    List<Transform> tail = new List<Transform>();

    Vector2 dir = Vector2.right;
    bool ate;

    void Start()
    {
        management = GameObject.Find("Management");
        scManager = management.GetComponent<SceneManagerr>();
        tailPrefab = Resources.Load<GameObject>("Tail");
        if(!isLocalPlayer) return;
        InvokeRepeating("CmdMove", speed, speed);
    }
    [Client]
    void Update()
    {
        if(!isLocalPlayer) return;
        CmdChangeDir();
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
            Die();
        }
    }
    void Die()
    {
        //scManager.LoadScene("Die");
    }

    [Command]
    void CmdChangeDir()
    {
        RpcChangeDir();
    }
    [ClientRpc]
    void RpcChangeDir()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector2.right;
            Debug.Log("Changed DIR");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector2.down;
            Debug.Log("Changed DIR");
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector2.left;
            Debug.Log("Changed DIR");
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector2.up;
            Debug.Log("Changed DIR");
        }
    }
    [Command]
    void CmdMove()
    {
        RpcMove();
    }
    [ClientRpc]
    void RpcMove()
    {
        Vector2 v = transform.position;
        transform.Translate(dir);
        Debug.Log("Moving");
        if(ate)
        {
            GameObject g = (GameObject)Instantiate(tailPrefab, v,
             Quaternion.identity);
             NetworkServer.Spawn(g);
             tail.Insert(0, g.transform);
             ate = false;
        }
        else if(tail.Count > 0)
        {
            tail.Last().position = v;
            tail.Insert(0, tail.Last());
            tail.RemoveAt(tail.Count-1);
        }
    }
}
