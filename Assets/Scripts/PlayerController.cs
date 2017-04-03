using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Unit {

    public float respawnTime;
    public float speed;
    public float jumpHeight;
//	float MouseRotationY = 0f;

  // private Transform camPivot;
    private float raycastDistance = .1f;
    private int startHealth;

	// Added a float for the coconut speed when throwing the coconuts 
	public float coconutSpeed; 

	public GameObject coconutPrefab;



	// Verrtical rotation float 
	float vertrotation = 0;
	// Camera up and down range float and make this public so we can set or change this float the unity editor. 
	public float CameraUpDownRange = 30f;
   // Set the mouse sensitivity and make this public so we can set or change this float in the  unity editor. 
	public float mousesensitivity = 4f;


    //We can override virtual methods. 
    //This means the Start method in the PlayerController is called,
    //instead of the Start in the Unit
	protected override void Start ()
    {
        //Calling base.Start(), will call Start in the Unit-class
        base.Start();
        startHealth = health;

        //Here I can put whatever I want, which will execute on Start of the PlayerController
        //camPivot = transform.Find("CamPivot");
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.timeScale == 0)
            return;


		// Added a cursor lockstate so that the cursor is locked when the game is run and so you do not see a cursor on your screen as it is disabled.
		Cursor.lockState = CursorLockMode.Locked;

		// THIS IS WHERE WE FIX THE CAMERA ROTATION 

		// This is what controls the camera pivot allowing the user to rotate the camera up and down
		vertrotation -= Input.GetAxis ("Mouse Y") * mousesensitivity;

		// We clamp the rotation of the camera to CameraUpDownRange which is the float defined and pass it in.
		vertrotation = Mathf.Clamp (vertrotation, -CameraUpDownRange, CameraUpDownRange);
		Camera.main.transform.localRotation = Quaternion.Euler (vertrotation, 0, 0);



		// Added the ability to drop coconuts as a player 
		if (Input.GetKeyDown(KeyCode.C))
		{

			// Instantiate the coconut, transform and set the coconut speed with the defined value of "coconutSpeed in the Unity editor".
			GameObject bullet = Instantiate(coconutPrefab, transform.position, Quaternion.identity) as GameObject;
			bullet.GetComponent<Rigidbody>().AddForce(transform.forward * coconutSpeed);
		


		}




        if (health <= 0)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        //We clamp the magnitude of our input vector (= we ensure the input vector has a max length of 'speed')
        //This way, when the player pressed both left and forward, the input vector won't be larger 
        Vector3 input = new Vector3(horizontalInput, 0, verticalInput) * speed;
        input = Vector3.ClampMagnitude(input, speed);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            input.y = jumpHeight;
            anim.SetTrigger("Jump");
        } else
        {
            //We make sure that the y-value of input, is not 0. If it would always be 0, the y velocity is always reset to 0 as well
            //This way (unless Space was pressed) the y velocity remains what it was (and the object falls down properly through gravity)
            input.y = rb.velocity.y;
        }

        //transform.TransformVector rotates a Vector from local to world space
        rb.velocity = transform.TransformVector(input);

        //We get the sideways delta movement of the mouse
        float mouseXInput = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseXInput, 0);

        anim.SetFloat("HorizontalSpeed", horizontalInput);
        anim.SetFloat("VerticalSpeed", verticalInput);

        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //hitInfo is an object that we pass into the Raycast method
            //Inside the raycastMethod, this object is created and given values
            //We use the 'out' keyword, which means it's not a new object that gets created, but a reference to this hitInfo object
            RaycastHit hitInfo;
            if (Physics.Raycast(camRay, out hitInfo)) {
                //if we shoot a ray from our eyes towards the point we hit with the camRay,
                //do we see the same object? If not, maybe the player is behind a wall
                if (CanSee(hitInfo.point, hitInfo.transform))
                {
                    ShootLasers(hitInfo.point, hitInfo.transform);
                }
            }
        }
	}

    bool IsGrounded ()
    {
        //Shooting a raycast from our feet downwards, return true if it hit something, false if it didn't
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance);
    }

    protected override void Die()
    {
        base.Die();
        //Calling the Respawn method after 'respawnTime' seconds
		anim.SetTrigger ("Jump");
        Invoke("Respawn", respawnTime);
    }

    void Respawn ()
    {
        health = startHealth;
        anim.SetBool("Death", false);

    }
}
