using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
	Rigidbody rb;
	public float moveSpeed = 5;	// Movement Speed
	public float lookSpeed = 5;	// Mouse Sensitivity
	public float jumpHeight = 2f; // Generic Jump Height
	public GameObject ProjectileSpawn;

	//This is the CharacterController Component --- NOT A SCRIPT
	public CharacterController CC;
	public Camera playerCamera;
	Vector3 velocity;
	public float lookXLimit = 90.0f; //	The max angle for looking up and down - 90f is directly up and down
	private float rotationX = 0f;
	public float gravity = -9.81f;	// The speed in which the pawn will fall

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeRotation;

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		// Detect if player is on ground
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if(isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}
	}

	public override void Look()
	{
		rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
		rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);	//Set max angle for looking up and down
		playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);	//Rotate Player along X 
		transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);	//Rotate Camera along Y
	}

	public override void Move(float x, float z)
	{
		//Original Movement
		//rb.velocity = gameObject.transform.right * x * moveSpeed + gameObject.transform.forward * z * moveSpeed;
		
		//For player movement using CharacterController Component
		Vector3 move = gameObject.transform.right * x + gameObject.transform.forward * z;
		CC.Move(move * moveSpeed * Time.deltaTime);

		//For Falling
		velocity.y += gravity * Time.deltaTime;
		CC.Move(velocity * Time.deltaTime);
	}

	public override void Jump()
	{
		if (isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
	}

	public override void Fire1(bool value)
	{
	
	}

	public override void Fire2(bool value)
	{

	}

	public override void Fire3(bool value)
	{

	}

	protected override void OnDeath()
	{

	}
}
