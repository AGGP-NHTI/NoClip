using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : Pawn
{
<<<<<<< Updated upstream
    Rigidbody rb;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
=======
    public Rigidbody rb;
    public Ray raycast;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
>>>>>>> Stashed changes
    }
}
