using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnFood : NetworkBehaviour
{
    bool spawning = false;
    [SyncVar]
    Vector2 spawnPos;
    [SerializeField]float rightPoint, leftPoint, upPoint, downPoint;
    void Update()
    {
        if(GameObject.FindGameObjectWithTag("Food") is null && !spawning) 
        {
            spawning = true;
            Invoke("Spawn", Random.Range(0.1f, 2.0f));
        }
    }
    void Spawn()
    {
        spawning = false;
        GameObject food = 
        (GameObject)Instantiate(Resources.Load<GameObject>("Food"),
        new Vector3(Random.Range(leftPoint, rightPoint),
         Random.Range(downPoint, upPoint), 0f), Quaternion.identity);
         NetworkServer.Spawn(food);
    }
}
