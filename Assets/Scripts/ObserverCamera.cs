using UnityEngine;
using System.Collections;

public class ObserverCamera : MonoBehaviour
{
    Transform observerCam;
    Transform cameraPath;
    Transform cameraFocus;

    Transform targetNavPoint;
    private int currentNavIndex;
    private Warp.Location currentLocation;

    public Warp.Location startLocation;
    public float cameraSpeed;

    public bool playerControl;

    public float randomWarpTime;
    
	// Use this for initialization
	void Start ()
    {
        observerCam = transform.Find("ObservationCamera");
        cameraPath = transform.Find("ObserverCamPath");
        cameraFocus = transform.Find("ObserverCamFocalPoint");
        currentNavIndex = 0;

        currentLocation = startLocation;
        StartCoroutine(AutoWarpCooldownCoroutine(randomWarpTime));
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateCameraPosition();

        if(playerControl)
        {
            MouseMovement();
            WarpInput();
        }
        else
        {
            UpdateNonPlayerCameraRotation();
            
        }
    }

    private void getNextNavPoint()
    {
        if (currentNavIndex >= cameraPath.transform.childCount)
            currentNavIndex = 0;

        targetNavPoint = cameraPath.transform.GetChild(currentNavIndex);
        currentNavIndex++;
    }

    private void UpdateCameraPosition()
    {
        if (targetNavPoint == null)
            getNextNavPoint();

        Vector3 direction = targetNavPoint.position - observerCam.transform.position;
        float distanceThisFrame = cameraSpeed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            targetNavPoint = null;
        }
        else
        {
            observerCam.transform.Translate(direction.normalized * distanceThisFrame);
        }
    }

    private void UpdateNonPlayerCameraRotation()
    {
        if (targetNavPoint == null)
            getNextNavPoint();

        Vector3 direction = cameraFocus.transform.position - observerCam.transform.position;
        Transform cameraRotator = observerCam.transform.Find("CameraRotator");

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        cameraRotator.rotation = Quaternion.Lerp(cameraRotator.transform.rotation, targetRotation, Time.deltaTime);
    }

    private void WarpInput()
    {
        //REDO THIS PLS
        if(Input.GetKeyDown("q"))
        {
            WarpCamera();
        }
    }

    public void WarpCamera()
    {
        Transform cameraRotator = observerCam.transform.Find("CameraRotator");

        int warpOffset = -2000;
        float xPos = cameraRotator.localPosition.x;

        if (currentLocation == Warp.Location.Blue)
        {
            xPos -= warpOffset;
            currentLocation = Warp.Location.Red;
        }
        else if (currentLocation == Warp.Location.Red)
        {
            xPos += warpOffset;
            currentLocation = Warp.Location.Blue;
        }

        cameraRotator.localPosition = new Vector3(xPos, cameraRotator.localPosition.y, cameraRotator.localPosition.z);
    }

    private void MouseMovement()
    {
        float horizontalSpeed = 2.0f;
        float verticalSpeed = 2.0f;

        float maxViewAngle = .9f;
        float minViewAngle = -.9f;

        Transform cameraRotator = observerCam.transform.Find("CameraRotator");
        Camera camera = cameraRotator.GetComponentInChildren<Camera>();

        cameraRotator.transform.Rotate(0, Input.GetAxis("Mouse X") * horizontalSpeed, 0);
        camera.transform.Rotate(-Input.GetAxis("Mouse Y") * verticalSpeed, 0, 0);

        if (Mathf.Sin(Mathf.Deg2Rad * camera.transform.eulerAngles.x) > maxViewAngle)
        {
            camera.transform.localRotation = Quaternion.Euler(Mathf.Asin(maxViewAngle) * Mathf.Rad2Deg, 0, 0);
        }
        else if (Mathf.Sin(Mathf.Deg2Rad * camera.transform.eulerAngles.x) < minViewAngle)
        {
            camera.transform.localRotation = Quaternion.Euler(Mathf.Asin(minViewAngle) * Mathf.Rad2Deg, 0, 0);
        }

    }

    private IEnumerator AutoWarpCooldownCoroutine(float randomWarpTime)
    {
        while(!playerControl)
        {
            yield return new WaitForSecondsRealtime(randomWarpTime);

            WarpCamera();
        }
    }
}
