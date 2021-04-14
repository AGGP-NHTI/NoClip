using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCScript : MonoBehaviour
{
    Rigidbody rb;

    Vector3 startPos;
    Quaternion startRot;

    bool forward, back, left, right, jump;

    public float speed = 20;
    public bool canJump = false;
    public float jumpForce = 5.0f;

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
        forward = Input.GetKey(KeyCode.W);
        back = Input.GetKey(KeyCode.S);
        left = Input.GetKey(KeyCode.A);
        right = Input.GetKey(KeyCode.D);
        jump = Input.GetKeyDown(KeyCode.Space);

        MoveForward(speed);
        MoveRight(speed);
    }

    void MoveForward(float value)
    {
        if (forward)
        {
            transform.Translate(Vector3.forward * value * Time.deltaTime);
        }

        if (back)
        {
            transform.Translate(-Vector3.forward * value * Time.deltaTime);
        }
    }

    void MoveRight(float value)
    {
        if (left)
        {
            transform.Translate(-Vector3.right * value * Time.deltaTime);
        }

        if (right)
        {
            transform.Translate(Vector3.right * value * Time.deltaTime);
        }
    }

    void Jump()
    {
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        canJump = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Floor")
        {
            canJump = true;
        }
    }
}
