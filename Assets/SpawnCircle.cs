using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnCircle : NetworkBehaviour
{
    GameObject colorCube;
    public override void OnStartServer()
    {
        colorCube = Resources.Load<GameObject>("ColorCube");
        Invoke("Spawn", 5f);
    }
    [ServerCallback]
    void Spawn()
    {
        GameObject p = Instantiate(colorCube, Vector3.zero, Quaternion.identity);
        NetworkServer.Spawn(p);
    }
    
}
