using UnityEngine;
using TMPro;
using Mirror;

public class TextScore : NetworkBehaviour
{
    TextMeshProUGUI text;
    static public int winner;
    void Start()
    {
        NetworkManager.singleton.StopClient();
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Player " + winner + " wins!";
    }
}
