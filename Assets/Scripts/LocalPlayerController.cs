using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LocalPlayerController : NetworkBehaviour {

    GameObject player;
    Rigidbody physics;
    ConstantForce forces;

    private Warp_Proxy warpManager;

    private PlayerInfoManager infoManager;
    private PlayerHUDManager hudManager;
    private GunManager gunManager;

    public float gravity;
    public float playerSpeed;
    public float jumpPower;

    public int jumps;
    private int jumpsLeft;

    private bool canJump;
    private Vector3 jumpVector;

    // Use this for initialization
	void Start ()
    {
        if (!isLocalPlayer)
        {
            enabled = false;
            GetComponentInChildren<PlayerNameTagManager>().enabled = true;
            return;
        }

        physics = GetComponent <Rigidbody>();
        forces = GetComponent<ConstantForce>();

        warpManager = GetComponent<Warp_Proxy>();

        infoManager = GetComponent<PlayerInfoManager>();
        hudManager = GetComponentInChildren<PlayerHUDManager>();
        gunManager = GetComponent<GunManager>();

        jumpsLeft = jumps;

        OnStartLocalPlayer();
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("LOCAL PLAYER SETTUP");

        //GetComponent<Warp>().enabled = true;
        GetComponentInChildren<Camera>().enabled = true;
        this.tag = "localPlayer";

        GetComponentInChildren<MeshRenderer>().material.color = Color.green;

        PlayerServerCommands serverCommands = GetComponent<PlayerServerCommands>();
        serverCommands.CmdSyncWeaponParents();
    }

    // Update is called once per frame
    void Update ()
    {
        KeyboardInput();
        MouseMovement();
    }

    void FixedUpdate()
    {
        if (canJump)
        {
            physics.AddForce(new Vector3(0, -500, 0), ForceMode.Acceleration);
        }
        else
        {
            physics.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
        }
    }

    private Vector3 tempJumpVector;

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
            GetComponent<PlayerServerCommands>().CmdKillPlayerOnServer(GetComponent<NetworkIdentity>().netId);
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            warpManager.WarpPlayer();
        }

        //Fire Control
        if (Input.GetKeyDown("mouse 0"))
        {
            this.gunManager.FireGun();
        }

        if(Input.GetKeyDown("mouse 1"))
        {

        }


    }

    private void KeyboardMovement()
    {
        float newX = 0;
        float newZ = 0;


        float xrot = Mathf.Sin(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y);
        float zrot = Mathf.Cos(Mathf.Deg2Rad * this.transform.rotation.eulerAngles.y);

        float currentX = physics.velocity.x;
        float currentZ = physics.velocity.z;

        if (Input.GetKey("w"))
        {
            newX += playerSpeed * xrot;
            newZ += playerSpeed * zrot;
        }
        if (Input.GetKey("s"))
        {
            newX += -playerSpeed * xrot;
            newZ += -playerSpeed * zrot;
        }
        if (Input.GetKey("a"))
        {
            newX += -playerSpeed * zrot;
            newZ += playerSpeed * xrot;
        }
        if (Input.GetKey("d"))
        {
            newX += playerSpeed * zrot;
            newZ += -playerSpeed * xrot;
        }

        
        if (Input.GetKeyDown("space") && canJump)
        {
            physics.velocity += new Vector3(0, jumpPower, 0);
            canJump = false;
        }
        
        //Canjump == true means the player is on the ground
        if(canJump)
        {
            jumpVector = new Vector3(0, 0, 0);
            physics.velocity = new Vector3(newX, physics.velocity.y, newZ);
        }
        else
        {
            if (newX == 0 && newZ == 0)
            {
                physics.velocity = jumpVector + new Vector3(0, physics.velocity.y, 0);
            }
            else
            {
                physics.velocity = new Vector3(newX / 2, physics.velocity.y, newZ / 2);
                jumpVector = new Vector3(newX, 0, newZ) / 4;

            }
        }

        
        
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

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GROUND")
        {
            canJump = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "GROUND")
        {
            canJump = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "GROUND")
        {
            canJump = true;
        }
    }

}
