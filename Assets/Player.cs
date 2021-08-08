using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    GameObject tailPrefab;
    bool ate;
    List<Transform> tail = new List<Transform>();
    Vector2 dir = Vector2.right;
    void Start()
    {
        tailPrefab = Resources.Load<GameObject>("Tail");
        InvokeRepeating("Move", 0.3f, 0.3f);
    }
    void Update()
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
    void Move()
    {
        Vector2 v = transform.position;
        transform.Translate(dir);
        if(ate)
        {
            GameObject g = (GameObject)Instantiate(tailPrefab, v,
             Quaternion.identity);
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
