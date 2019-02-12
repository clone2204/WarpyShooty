using UnityEngine;
using UnityEngine.Networking;

using System.Collections;

public class PlayerNameTagManager : NetworkBehaviour {

    string playerName;
    TextMesh nameTag;
    GameObject localPlayer;

	// Use this for initialization
	void Start ()
    {
        playerName = this.GetComponentInParent<PlayerInfoManager>().playerInfo.playerName;
        nameTag = this.GetComponent<TextMesh>();

        nameTag.text = playerName;

        localPlayer = GameObject.FindGameObjectWithTag("localPlayer");
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.nameTag.transform.LookAt(localPlayer.transform);
        this.nameTag.transform.Rotate(0, 180, 0);
	}

}
