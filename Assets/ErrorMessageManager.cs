using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ErrorMessageManager : MonoBehaviour
{
    private string errorType;
    private string error;
    private bool activeError;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        if (scene.name == "TitleScreen")
        {
            if(activeError)
                RefreshError();
        }
    }

    public void SetErrorMessage(string errorType, string error)
    {
        this.errorType = errorType;
        this.error = error;
        activeError = true;

        RefreshError();
    }

    public void RefreshError()
    {
        GameObject.Find("ErrorType").GetComponent<Text>().text = errorType;
        GameObject.Find("ErrorMessage").GetComponent<Text>().text = error;

        GameObject.Find("ErrorMessageBox").transform.localPosition = new Vector3();
    }

    public void ClearError()
    {
        activeError = false;
        GameObject.Find("ErrorMessageBox").transform.localPosition = new Vector3(0, 100000);
    }
}
