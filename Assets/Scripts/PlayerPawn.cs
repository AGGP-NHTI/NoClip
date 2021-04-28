using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
	public GameObject ProjectileSpawn;

	[Header("Movement")]
	public float verticalSensitivity = 3.0f;
	public float horizontalSensitivity = 200.0f;
	//public float jumpHeight = 2f; // Generic Jump Height
	//public CharacterController CC; //This is the CharacterController Component --- NOT A SCRIPT
	public GameObject playerCamera;
	public float gravity = -9.81f;  // The speed in which the pawn will fall
	float currentGravity; // If pawn is in the air compound gravity to increase speed
	float jumpForce;
	public float jumpHeight = 15f;
	public float jumpTime = 0.0f;
	public float jumpTimeMax = 10.0f;
	public float jumpTimeCount = 1.0f;
	Rigidbody rb;
	float minCamClamp = -89.9f;
	float maxCamClamp = 89.9f;
	float camPitch = 0.0f;
	bool forwards = false;
	bool backwards = false;
	bool right = false;
	bool left = false;
	bool isJumping = false;
	Vector2 leftStick = Vector2.zero;
	Vector2 rightStick = Vector2.zero;

	[Header("Wallrun")]
	public LayerMask isWall;
	public float wallRunForce, checkDist;
	bool wallRight, wallLeft, isWallRunning;

	[Header("Health")]
	public bool depleteHealthOverTime = true;
	private float energyMultiplier;
	public AudioSource criticalHealth;
	public AudioSource coreStabile;
	public AudioSource coreCritical;

	[Tooltip("As Percentage of Max Health where the player is Low Health")]
	[Range(0f, 1f)]
	public float lowLevel = 0.2f;

	[Tooltip("As Percentage of Max Health where the player is Critical Health")]
	[Range(0f,1f)]
	public float criticalLevel = 0.1f;
	bool healthCritical = false;

	[Header("Spells")]
	public List<Spells> spells;
	public int index = 0;
	[Tooltip("How much energy over the critical amount does the player need minimum to cast spells.")]
	public float energyOverCritical = 0f;
	[Tooltip("Can the player cast a spell into the critical health zone?")]
	public bool castCanKill = false;
	public GameObject[] projectiles;

	[Header("Radar")]
	public bool exactPosition = false;

	public float getEnergy
	{
		get { return energyMultiplier; }
	}

	void Awake()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		//rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.useGravity = true;
		jumpHeight = jumpForce;


		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public override void Update()
	{
		base.Update();

		// Detect if player is on ground
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
		rb.useGravity = false;

		GetInput();

		MoveStrafe(leftStick);

		//if radial menu is not open allow mouse player movement		
		if (radMenuOpen == false)
		{
			RotateRight(rightStick.x);
			CameraPitch(rightStick.y);
		}

		CheckForWall();
		WallRunInput();

		UpdateHealth();

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			isJumping = true;
		}
			/*if(isGrounded && velocity.y < 0)
			{
				velocity.y = -2f;
			}*/

		if (!isGrounded && !isWallRunning)
		{
			currentGravity += (currentGravity / 2) * Time.deltaTime;

			rb.useGravity = true;

			rb.velocity += new Vector3 (0f, currentGravity, 0f);

			//Physics.gravity = new Vector3(0, gravity, 0);
		}

		if (isGrounded || isWallRunning)
		{
			currentGravity = gravity;
		}

		CheckForWall();
		WallRunInput();
	}

	private void UpdateHealth()
	{
		if (depleteHealthOverTime)
		{
			Health -= Time.deltaTime;

			if (exactPosition)
			{
				Health -= spells[2].EnergyCost * Time.deltaTime;
			}
		}

		//Used for casting spells and updating energy bar and ability mx
		energyMultiplier = ((Health - (StartingHealth * criticalLevel)) / spells[index].EnergyCost) - (energyOverCritical / 5);
		if (energyMultiplier < 0)
		{
			energyMultiplier = 0;
		}

		//Detect when player is at a percentage of their health, if true then play critical health sound
		if (Health < (StartingHealth * criticalLevel) && criticalHealth.isPlaying == false)
		{
			criticalHealth.Play();
			healthCritical = true;
			exactPosition = false; 

		}
		else if (Health > (StartingHealth * criticalLevel) && healthCritical == true)
		{
			coreStabile.Play();
			criticalHealth.Stop();
			healthCritical = false;
		}

		//When entering critical health
		if (Health < StartingHealth * criticalLevel && Health > (StartingHealth * criticalLevel) - 0.1)
		{
			coreCritical.Play();
		}

		//Prvent health from going under zero
		if (Health < 0)
		{
			criticalHealth.Stop();
			Health = 0;
		}

		//Prevent health from going over max
		if (Health > StartingHealth)
		{
			Health = StartingHealth;
		}
	}

	void FixedUpdate()
	{
		if(isJumping && (jumpTime < jumpTimeMax))
		{
			Jump();
			jumpTime += jumpTimeCount;
		}
		
		if(jumpTime >= jumpTimeMax)
		{
			jumpTime = 0.0f;
			isJumping = false;
		}

	}

	//Old Look/Jump/Movement Code Using CharacterController Component
	/*public override void Look()
	{
		if (radMenuOpen == false)
		{
			rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
			rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);    //Set max angle for looking up and down
			playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);   //Rotate Player along X 
			transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0); //Rotate Camera along Y
		}

		if (wallRight && isWallRunning)
		{
			if (Mathf.Abs(wallRunCamTilt) < maxCamTilt)
			{
				wallRunCamTilt += Time.deltaTime * maxCamTilt * 2;
			}
		}

		if (wallLeft && isWallRunning)
		{
			if (Mathf.Abs(wallRunCamTilt) < maxCamTilt)
			{
				wallRunCamTilt -= Time.deltaTime * maxCamTilt * 2;
			}
		}

		if (!isWallRunning && wallRunCamTilt > 0 && !isGrounded)
		{
			wallRunCamTilt = 0.0f;
			playerCamera.transform.localRotation = Quaternion.Euler(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, wallRunCamTilt);
		}

		if (!isWallRunning && wallRunCamTilt < 0 && !isGrounded)
		{
			wallRunCamTilt = 0.0f;
			playerCamera.transform.localRotation = Quaternion.Euler(playerCamera.transform.rotation.x, playerCamera.transform.rotation.y, wallRunCamTilt);
		}
	}

	public override void Move(float x, float z)
	{
		//Original Movement
		//rb.velocity = gameObject.transform.right * x * moveSpeed + gameObject.transform.forward * z * moveSpeed;
		
		//For player movement using CharacterController Component
		Vector3 move = gameObject.transform.right * x + gameObject.transform.forward * z;
		CC.Move(move * moveSpeed * Time.deltaTime);

		//For Falling
		if(!isWallRunning)
		{
			velocity.y += gravity * Time.deltaTime;
			//CC.Move(velocity * Time.deltaTime);
		}
		if(isWallRunning)
		{
			velocity.y += wallGrav * Time.deltaTime;
			//CC.Move(velocity * Time.deltaTime);
		}
	}*/

	/*public override void Jump()
	{
		if (isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}

		/*if (isWallRunning)
		{
			if(wallLeft && !Input.GetKey(KeyCode.D) || (wallRight && !Input.GetKey(KeyCode.A)))
			{
				CC.Move(transform.up * (jumpForce / 5) * Time.deltaTime);
				CC.Move(transform.forward * (jumpForce / 5) * Time.deltaTime);
			}

			if (wallLeft && Input.GetKey(KeyCode.D))
			{
				StopWallRun();
				CC.Move(transform.up * (jumpForce / 5) * Time.deltaTime);
				CC.Move(transform.forward * jumpForce * Time.deltaTime);
			}

			if (wallRight && Input.GetKey(KeyCode.A))
			{
				StopWallRun();
				CC.Move(transform.up * (jumpForce / 5) * Time.deltaTime);
				CC.Move(transform.forward * jumpForce * Time.deltaTime);
			}
		}
	}*/

	//New Movement Using Rigidbody
	void GetInput()
	{
		leftStick = Vector2.zero;
		rightStick = Vector2.zero;
		forwards = Input.GetKey(KeyCode.W);
		backwards = Input.GetKey(KeyCode.S);
		left = Input.GetKey(KeyCode.A);
		right = Input.GetKey(KeyCode.D);

		rightStick.x = Input.GetAxis("Mouse X");
		rightStick.y = Input.GetAxis("Mouse Y");
		KeyToAxis();
	}

	void KeyToAxis()
	{
		if (forwards)
		{
			leftStick.y = 1;
		}
		if (backwards)
		{
			leftStick.y = -1;
		}
		if (right)
		{
			leftStick.x = 1;
		}
		if (left)
		{
			leftStick.x = -1;
		}
	}

	void MoveStrafe(Vector2 value)
	{
		MoveStrafe(value.x, value.y);
	}

	void MoveStrafe(float horizontal, float vertical)
	{
		rb.velocity = Vector3.zero;
		rb.velocity += (gameObject.transform.forward * vertical * moveSpeed);
		rb.velocity += (gameObject.transform.right * horizontal * moveSpeed);

		//Will move faster if moving diagonally
	}

	public override void Jump()
	{
		if(!isWallRunning)
		{
			//rb.velocity = new Vector3(0, jumpForce, 0);
			rb.AddForce(Vector3.up * jumpForce * 50);
			rb.AddForce(Vector3.up * jumpForce * 50);
		}
		else
		{
			if ((wallLeft && !Input.GetKey(KeyCode.D)) || (wallRight && !Input.GetKey(KeyCode.A)))
			{
				Debug.Log("normal jump");
				rb.AddForce(Vector2.up * jumpForce * 25);
				rb.AddForce(Vector3.up * jumpForce * 25);
			}

			if (wallRight && Input.GetKey(KeyCode.A))
			{
				Debug.Log("right wall hop");
				rb.AddForce(-transform.right * jumpForce * 250);
				rb.AddForce(transform.forward * jumpForce * 150);
			}

			if (wallLeft && Input.GetKey(KeyCode.D))
			{
				Debug.Log("left wall hop");
				rb.AddForce(transform.right * jumpForce * 250);
				rb.AddForce(transform.forward * jumpForce * 150);
			}

			//Always add forward force
			rb.AddForce(transform.forward * jumpForce * 100);
		}
	}

	void CameraPitch(float value)
	{
		camPitch -= (value * verticalSensitivity);
		camPitch = Mathf.Clamp(camPitch, minCamClamp, maxCamClamp);

		playerCamera.transform.localRotation = Quaternion.Euler(camPitch, 0f, 0f);
		//cameraObj.Rotate(Vector3.up * yRotation);
	}

	void RotateRight(float value)
	{
		gameObject.transform.Rotate(Vector3.up * value * Time.deltaTime * horizontalSensitivity);
	}
	
	private void WallRunInput()
	{
		if(Input.GetKey(KeyCode.D) && wallRight && !isGrounded)
		{
			StartWallRun();
		}

		if (Input.GetKey(KeyCode.A) && wallLeft && !isGrounded)
		{
			StartWallRun();
		}
	}

	private void StartWallRun()
	{
		isWallRunning = true;
		isGrounded = true;

		rb.AddForce(gameObject.transform.forward * wallRunForce);

		if(wallRight)
		{
			rb.AddForce(gameObject.transform.right * wallRunForce / 5 * Time.deltaTime);
		}
		else
		{
			rb.AddForce(-gameObject.transform.right * wallRunForce / 5 * Time.deltaTime);
		}
	}

	private void StopWallRun()
	{
		isWallRunning = false;
	}

	private void CheckForWall()
	{
		wallRight = Physics.Raycast(playerCamera.transform.position, playerCamera.transform.right, checkDist, isWall);
		wallLeft = Physics.Raycast(playerCamera.transform.position, -playerCamera.transform.right, checkDist, isWall);

		if(!wallLeft && !wallRight)
		{
			StopWallRun();
		}
	}

	public override void Fire1(bool value)
	{
	
	}

	//To Cast Spell
	public override void Fire2(bool value)
	{
		if (spells[index].HoldCast == false && energyMultiplier > 1)
		{
			Health -= spells[index].EnergyCost;

			CastSpell(index);
		}
	}

	public override void Fire3(bool value)
	{

	}

	public override void AbilityMenu(bool value)
	{
		if (value == true) 
		{
			radMenuOpen = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		else
		{
			radMenuOpen = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	private void CastSpell(int index)
	{
		if (index == 0)
		{
			ShootFireball(index);
		}
		if (index == 1)
		{
			FreezeArea(index);
		}
		if (index == 2)
		{
			Enhancement(index);
		}
	}

	private void ShootFireball(int index)
	{
		GameObject fireball = Factory(projectiles[0], ProjectileSpawn.transform.position, ProjectileSpawn.transform.rotation, controller);
		Fireball fb = fireball.GetComponent<Fireball>();
		
		fb.GetComponent<Rigidbody>().velocity = ProjectileSpawn.transform.forward * spells[index].Amount2;
		fb.damage = spells[index].Amount;
		fb.lifetime = spells[index].Duration;
		fb.Owner = controller;
	}

	private void FreezeArea(int index)
	{
		GameObject freezeArea = Factory(projectiles[1], transform.position, transform.rotation, controller);
		Freeze freeze = freezeArea.GetComponent<Freeze>();

		freeze.damage = spells[index].Amount;
		freeze.freezeRadius = spells[index].Amount2;
		freeze.lifetime = spells[index].Duration;
		freeze.owner = this;
		freeze.contOwner = controller;
		freeze.Activate();
	}

	private void Enhancement(int index)
	{
		if (exactPosition == true)
		{
			exactPosition = false;
			moveSpeed /= spells[index].Amount;
			jumpForce /= spells[index].Amount2;

		}
		else if (exactPosition == false)
		{
			exactPosition = true;
			moveSpeed *= spells[index].Amount;
			jumpForce *= spells[index].Amount2;
		}
	}

	public override void AddEffectHUD(BaseEffect effect)
	{
		if (effect.typeOfBuff == BaseEffect.buffType.Buff)
		{
			//playerHUD.AddBuff(effect.Name);
		}

		if (effect.typeOfBuff == BaseEffect.buffType.Debuff)
		{
			//playerHUD.AddDebuff(effect.Name);
		}
	}

	public override void RemoveEffectHUD(BaseEffect effect)
	{
		if (effect.typeOfBuff == BaseEffect.buffType.Buff)
		{
			//playerHUD.RemoveBuff(effect.Name);
		}

		if (effect.typeOfBuff == BaseEffect.buffType.Debuff)
		{
			//playerHUD.RemoveDebuff(effect.Name);
		}
	}

	public override void UpdateEffectHUD(BaseEffect effect, float percentageFill)
	{
		//playerHUD.UpdateEffect(effect, percentageFill);
	}

	public override void AddOngingEffect(BaseEffect effect)
	{
		base.AddOngingEffect(effect);
	}

	public override void RemoveOngingEffect(BaseEffect effect)
	{
		base.RemoveOngingEffect(effect);
	}

	protected override void OnDeath()
	{

	}
}
