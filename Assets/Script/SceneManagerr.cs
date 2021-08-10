using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class SceneManagerr : MonoBehaviour
{
    //NetworkManager networkManager;
    void Start()
    {
       // networkManager = GameObject.Find("NetworkManager")
       // .GetComponent<NetworkManager>();
    }
    void Update()
    {
        // if(SceneManager.GetActiveScene().name == "Die")
        // {
        //    if (NetworkServer.active && NetworkClient.isConnected)
        //     {
        //         networkManager.StopHost();
        //     }
        //     else if (NetworkClient.isConnected)
        //     {
        //         networkManager.StopClient();
        //     }
        // }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    // public void HostGame()
    // {
    //     networkManager.StartHost();
    // }
    // public void JoinGame()
    // {
    //     networkManager.StartClient();
    // }
}
