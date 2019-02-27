using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListManager : MonoBehaviour
{
    private Transform playerList;
    private bool teams;

    // Start is called before the first frame update
    void Start()
    {
        playerList = transform.FindChild("PlayerList");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearPlayerList()
    {
        for (int loop = 0; loop < 12; loop++)
        {
            Text name = playerList.FindChild("player" + loop).FindChild("Name").GetComponent<Text>();
            name.text = "";
        }
    }

    public void UpdatePlayerList(List<string> players, string host)
    {
        for(int loop = 0; loop < players.Count; loop++)
        {
            Text name = playerList.FindChild("player" + loop).FindChild("Name").GetComponent<Text>();
            name.text = players[loop];

            if(players[loop].Equals(host))
            {
                //Indicate host here
            }
        }
    }

    public void ToggleTeams(bool teams)
    {
        this.teams = teams;
        playerList.FindChild("TeamLabels").gameObject.active = teams;
    }
}
