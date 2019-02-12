using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool TogglePauseMenuOnOff()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.enabled = !canvas.enabled;

        return canvas.enabled;
    }

    public void ExitToWindows()
    {
        Debug.LogWarning("PING");
    }
}
