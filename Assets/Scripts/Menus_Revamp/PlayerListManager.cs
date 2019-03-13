using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerListManager : MonoBehaviour
{
    private bool teams;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClearPlayerList()
    {
        for (int loop = 0; loop < 12; loop++)
        {
            Text name = transform.Find("PlayerList").Find("player" + loop).Find("Name").GetComponent<Text>();
            name.text = "";
        }
    }

    public void UpdatePlayerList(List<string> players)
    {
        ClearPlayerList();
        
        for(int loop = 0; loop < players.Count; loop++)
        {
            Text name = transform.Find("PlayerList").Find("player" + loop).Find("Name").GetComponent<Text>();
            string displayName = players[loop];

            if(loop == 0)
            {
                displayName += " *";
            }

            name.text = displayName;
        }
    }

    public void ToggleTeams(bool teams)
    {
        this.teams = teams;
        transform.Find("PlayerList").Find("TeamLabels").gameObject.active = teams;
    }
}
