using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCScript : MonoBehaviour
{
    Rigidbody rb;

    Vector3 startPos;
    Quaternion startRot;

    bool jump;

    public float speed = 20;
    public bool canJump = false;
    public float jumpForce = 5.0f;

    public GameObject transCam;

    public float health, currentHealth, stamina, energy;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
    }

    void Update()
    {
        PlayerInput();

        if (jump && canJump)
        {
            Jump();
        }

        if (currentHealth <= 0)
        {
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = startRot;

            currentHealth = health;
        }
    }

    void PlayerInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        movement = transCam.transform.rotation * movement;

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
        }

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        jump = Input.GetKeyDown(KeyCode.Space);
    }

    void Jump()
    {
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        canJump = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }
}
