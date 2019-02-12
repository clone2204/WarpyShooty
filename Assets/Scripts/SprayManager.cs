using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SprayManager : MonoBehaviour
{

    public WWW spraySource;
    private Texture spray;


	// Use this for initialization
	void Start ()
    {
        Debug.LogWarning("DING");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (spray != null)
            return;

        if (spraySource != null && spraySource.isDone)
        {
            RawImage preview = GameObject.Find("SprayPreview").GetComponent<RawImage>();
            preview.texture = spraySource.texture;

            spray = spraySource.texture;
            GetComponent<RawImage>().texture = spray;
            GetComponent<Canvas>().enabled = true;
        }
    }

    public void SetSprayPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetSprayRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
