using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LocalPlayerController : NetworkBehaviour
{
    private LobbyPlayerManager infoManager;

    private CharacterController characterController;

    private Warp warpManager;
    private GunManager gunManager;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float terminalVelocity;

    [SerializeField] private int jumps;
    private int jumpsLeft;

    // Use this for initialization
	void Start ()
    {
        if (!isLocalPlayer)
        {
            enabled = false;
            GetComponentInChildren<PlayerNameTagManager>().enabled = true;
            return;
        }

        infoManager = GetComponent<LobbyPlayerManager>();

        characterController = GetComponent<CharacterController>();

        warpManager = GetComponent<Warp>();
        gunManager = GetComponent<GunManager>();

        jumpsLeft = jumps;
    }

    // Update is called once per frame
    void Update ()
    {
        KeyboardInput();
        MouseMovement();
    }

    private void KeyboardInput()
    {
        KeyboardMovement();

        //Other Inputs
        if (Input.GetKeyDown("r"))
        {
            this.gunManager.ReloadGun();
        }
        if (Input.GetKeyDown("e"))
        {
            this.gunManager.SwapGun();
        }
        if(Input.GetKeyDown("t"))
        {
            this.gunManager.SprayPlayerSpray();
        }
        if(Input.GetKeyDown("k"))
        {

        }
        if(Input.GetKeyDown("q"))
        {
            warpManager.WarpPlayer();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            warpManager.WarpPlayer();
        }

        //Fire Control
        if (Input.GetKeyDown("mouse 0"))
        {
            //this.gunManager.FireGun();
        }

        if(Input.GetKeyDown("mouse 1"))
        {

        }


    }

    private Vector3 moveDirection = Vector3.zero;
    private float verticalMovement = 0;
    private void KeyboardMovement()
    {
        float newX = 0;
        float newZ = 0;

        float xrot = Mathf.Sin(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y);
        float zrot = Mathf.Cos(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y);

        if (Input.GetKey("w"))
        {
            newX += xrot;
            newZ += zrot;
        }
        if (Input.GetKey("s"))
        {
            newX -= xrot;
            newZ -= zrot;
        }
        if (Input.GetKey("a"))
        {
            newX -= zrot;
            newZ += xrot;
        }
        if (Input.GetKey("d"))
        {
            newX += zrot;
            newZ -= xrot;
        }

        moveDirection = new Vector3(newX, 0, newZ);
        moveDirection *= playerSpeed;

        if (!characterController.isGrounded)
            moveDirection *= .75f;

        //moveDirection.y -= 
        
        if (Input.GetKey("space") && characterController.isGrounded)
        {
            verticalMovement = jumpSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void MouseMovement()
    {
        float horizontalSpeed = 2.0f;
        float verticalSpeed = 2.0f;

        float maxViewAngle = .9f;
        float minViewAngle = -.9f;

        Camera camera = this.GetComponentInChildren<Camera>();

        this.transform.Rotate(0, Input.GetAxis("Mouse X") * horizontalSpeed, 0);
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

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            verticalMovement = 0;
            return;
        }

        characterController.Move(new Vector3(0, verticalMovement, 0) * Time.deltaTime);

        if(-1 * (verticalMovement - gravity) < terminalVelocity)
            verticalMovement -= gravity;
    }


    public float Gravity()
    {


        return 0f;
    }
}
